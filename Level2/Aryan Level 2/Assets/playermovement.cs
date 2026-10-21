using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float runMultiplier = 1.5f;

    [Header("Squash and Stretch Settings")]
    public float maxStretch = 0.3f;     // How much the ball deforms at max speed
    public float scaleLerpSpeed = 15f;  // How smoothly it transitions between shapes

    private Rigidbody2D rb;
    private Vector3 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

        // Apply Squash and Stretch
        HandleSquashAndStretch(movement);
    }

    void HandleSquashAndStretch(Vector2 movement)
    {
        if (movement.sqrMagnitude > 0.01f)
        {
            // Calculate stretch intensity based on actual velocity vs max possible speed
            float maxPossibleSpeed = speed * runMultiplier;
            float speedRatio = Mathf.Clamp01(rb.linearVelocity.magnitude / maxPossibleSpeed);
            float stretch = speedRatio * maxStretch;

            // Rotate the ball to face the direction of movement so it stretches along its path
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // Stretch along the X-axis (direction of travel) and squash along the Y-axis
            Vector3 targetScale = new Vector3(
                originalScale.x * (1f + stretch),
                originalScale.y * (1f - stretch),
                1f
            );

            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleLerpSpeed);
        }
        else
        {
            // Smoothly return to normal scale when the ball stops moving
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * scaleLerpSpeed);
        }
    }
}