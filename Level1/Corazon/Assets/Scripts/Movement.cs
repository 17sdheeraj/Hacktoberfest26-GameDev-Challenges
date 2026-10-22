using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;

    [Header("Jump")]
    public float jumpForce = 12f;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.12f;
    public LayerMask groundLayer;

    [Header("Optional Visual")]
    public SpriteRenderer spriteRenderer;

    public float SpeedMultiplier { get; set; } = 1f;
    public float FallGravityMultiplier { get; set; } = 1f;
    public int MaxJumps { get; set; } = 1;

    public int FacingDirection { get; private set; } = 1;
    public bool IsGrounded { get; private set; }

    public float InputX => horizontalInput;
    public float VerticalSpeed => Velocity.y;

    private Rigidbody2D rb;
    private float normalGravityScale;
    private float horizontalInput;

    private bool jumpRequested;
    private int jumpsUsed;

    private float dashTimeRemaining;
    private float dashSpeed;
    private int dashDirection;

    private Vector2 Velocity
    {
        get
        {
#if UNITY_6000_0_OR_NEWER
            return rb.linearVelocity;
#else
            return rb.velocity;
#endif
        }
        set
        {
#if UNITY_6000_0_OR_NEWER
            rb.linearVelocity = value;
#else
            rb.velocity = value;
#endif
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        normalGravityScale = rb.gravityScale;

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput > 0f)
            FacingDirection = 1;
        else if (horizontalInput < 0f)
            FacingDirection = -1;

        if (spriteRenderer != null)
            spriteRenderer.flipX = FacingDirection < 0;

        if (Input.GetKeyDown(KeyCode.Space))
            jumpRequested = true;
    }

    private void FixedUpdate()
    {
        Vector2 velocity = Velocity;

        bool groundDetected = groundCheck != null
            && Physics2D.OverlapCircle(
                groundCheck.position,
                groundCheckRadius,
                groundLayer
            ) != null;

        // Prevent the ground-check overlap immediately after
        // takeoff from resetting the jump count.
        IsGrounded = groundDetected && velocity.y <= 0.1f;

        if (IsGrounded)
            jumpsUsed = 0;

        // Dash replaces horizontal movement temporarily.
        // Vertical motion and gravity remain active.
        if (dashTimeRemaining > 0f)
        {
            velocity.x = dashDirection * dashSpeed;
            dashTimeRemaining -= Time.fixedDeltaTime;
        }
        else
        {
            velocity.x =
                horizontalInput * moveSpeed * SpeedMultiplier;
        }

        if (jumpRequested)
        {
            // Walking off a platform consumes the ground jump.
            int effectiveJumpsUsed = IsGrounded
                ? jumpsUsed
                : Mathf.Max(jumpsUsed, 1);

            if (effectiveJumpsUsed < MaxJumps)
            {
                velocity.y = jumpForce;
                jumpsUsed = effectiveJumpsUsed + 1;
                IsGrounded = false;
            }

            jumpRequested = false;
        }

        rb.gravityScale = normalGravityScale
            * (velocity.y < 0f ? FallGravityMultiplier : 1f);

        Velocity = velocity;
    }

    public void StartDash(int direction, float speed, float duration)
    {
        dashDirection = direction >= 0 ? 1 : -1;
        dashSpeed = speed;
        dashTimeRemaining = duration;
    }

    public void StopDash()
    {
        dashTimeRemaining = 0f;
    }

    public void TeleportTo(Vector2 destination)
    {
        StopDash();

        rb.position = destination;
        Velocity = Vector2.zero;

        jumpRequested = false;
        IsGrounded = false;

        // Teleporting into the air does not grant a free ground jump.
        jumpsUsed = 1;
    }

    private void OnDisable()
    {
        if (rb != null)
        {
            rb.gravityScale = normalGravityScale;
            Velocity = Vector2.zero;
        }

        jumpRequested = false;
        StopDash();
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            groundCheck.position,
            groundCheckRadius
        );
    }
}