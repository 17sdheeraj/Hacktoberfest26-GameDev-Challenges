
using UnityEngine;
using System.Collections;

public class PlayerDash : MonoBehaviour
{
    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    [Header("Dash Effects")]
    public ParticleSystem dashParticles;
    public SpriteRenderer playerSprite;
    public float ghostInterval = 0.04f;

    private Rigidbody2D rb;
    private bool isDashing;
    private bool canDash = true;
    private Vector2 dashDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (playerSprite == null)
            playerSprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && canDash && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        dashDirection = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        if (dashDirection == Vector2.zero)
        {
            dashDirection = transform.right;

            if (playerSprite != null && playerSprite.flipX)
                dashDirection = -dashDirection;
        }

        if (dashParticles != null)
        {
            dashParticles.transform.position = transform.position;
            dashParticles.Play();
        }

        float timer = 0f;
        float ghostTimer = 0f;

        while (timer < dashDuration)
        {
            rb.linearVelocity = dashDirection * dashSpeed;

            ghostTimer += Time.deltaTime;
            if (ghostTimer >= ghostInterval)
            {
                CreateGhost();
                ghostTimer = 0f;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    void CreateGhost()
    {
        if (playerSprite == null)
            return;

        GameObject ghost = new GameObject("DashGhost");
        ghost.transform.position = playerSprite.transform.position;
        ghost.transform.rotation = playerSprite.transform.rotation;
        ghost.transform.localScale = playerSprite.transform.lossyScale;

        SpriteRenderer ghostRenderer = ghost.AddComponent<SpriteRenderer>();
        ghostRenderer.sprite = playerSprite.sprite;
        ghostRenderer.flipX = playerSprite.flipX;
        ghostRenderer.flipY = playerSprite.flipY;
        ghostRenderer.sortingLayerID = playerSprite.sortingLayerID;
        ghostRenderer.sortingOrder = playerSprite.sortingOrder - 1;
        ghostRenderer.color = new Color(1f, 1f, 1f, 0.65f);

        StartCoroutine(FadeGhost(ghostRenderer, ghost));
    }

    IEnumerator FadeGhost(SpriteRenderer ghostRenderer, GameObject ghost)
    {
        float duration = 0.18f;
        float timer = 0f;
        Color startColor = ghostRenderer.color;

        while (timer < duration)
        {
            if (ghostRenderer == null)
                yield break;

            float alpha = Mathf.Lerp(startColor.a, 0f, timer / duration);
            ghostRenderer.color = new Color(
                startColor.r, startColor.g, startColor.b, alpha
            );

            timer += Time.deltaTime;
            yield return null;
        }

        if (ghost != null)
            Destroy(ghost);
    }
}
