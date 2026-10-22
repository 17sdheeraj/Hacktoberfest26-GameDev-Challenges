
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    [Header("Bounce Settings")]
    public float bounceForce = 14f;

    [Header("Visual Feedback")]
    public Color normalColor = Color.green;
    public Color activatedColor = Color.yellow;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            spriteRenderer.color = normalColor;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

        if (playerRb == null)
            return;

        // Launch the player upward.
        playerRb.linearVelocity = new Vector2(
            playerRb.linearVelocity.x,
            bounceForce
        );

        if (spriteRenderer != null)
        {
            spriteRenderer.color = activatedColor;
            CancelInvoke(nameof(ResetColor));
            Invoke(nameof(ResetColor), 0.25f);
        }

        Debug.Log("Bounce pad activated!");
    }

    void ResetColor()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = normalColor;
    }
}
