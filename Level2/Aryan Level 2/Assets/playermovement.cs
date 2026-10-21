using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float runMultiplier = 1.5f;

    [Header("Squash and Stretch Settings")]
    public float maxStretch = 0.3f;     // How much the ball deforms at max speed
    public float scaleLerpSpeed = 15f;  // How smoothly it transitions between shapes

    [Header("Growth Settings (E Key)")]
    public float growMultiplier = 1.8f; // How much bigger the ball gets when E is held
    public float growLerpSpeed = 12f;   // How smoothly it grows and shrinks

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector3 originalScale;
    private float currentGrowth = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        // Determine current speed based on whether Shift is held down
        float currentSpeed = speed;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed *= runMultiplier;
        }

        Vector2 movement = new Vector2(x, y).normalized;
        rb.linearVelocity = movement * currentSpeed;

        // Handle E key growth input smoothly
        float targetGrowth = Input.GetKey(KeyCode.E) ? growMultiplier : 1f;
        currentGrowth = Mathf.Lerp(currentGrowth, targetGrowth, Time.deltaTime * growLerpSpeed);

        // Handle P key invisibility (turns off SpriteRenderer while P is held down)
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = !Input.GetKey(KeyCode.P);
        }

        // Apply Squash, Stretch, and Growth
        HandleSquashAndStretch(movement);
    }

    void HandleSquashAndStretch(Vector2 movement)
    {
        // The base scale dynamically updates based on whether the E key is held
        Vector3 currentBaseScale = originalScale * currentGrowth;

        if (movement.sqrMagnitude > 0.01f)
        {
            // Calculate stretch intensity based on actual velocity vs max possible speed
            float maxPossibleSpeed = speed * runMultiplier;
            float speedRatio = Mathf.Clamp01(rb.linearVelocity.magnitude / maxPossibleSpeed);
            float stretch = speedRatio * maxStretch;

            // Rotate the ball to face the direction of movement so it stretches along its path
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // Stretch along the X-axis (direction of travel) and squash along the Y-axis, scaling with E growth
            Vector3 targetScale = new Vector3(
                currentBaseScale.x * (1f + stretch),
                currentBaseScale.y * (1f - stretch),
                1f
            );

            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleLerpSpeed);
        }
        else
        {
            // Smoothly return to the current growth scale (or original size if E is released) when stopped
            transform.localScale = Vector3.Lerp(transform.localScale, currentBaseScale, Time.deltaTime * scaleLerpSpeed);
        }
    }
}