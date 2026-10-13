using UnityEngine;

// Put on the dart prefab. Needs: Collider (not trigger) + Rigidbody (Use Gravity OFF).
[RequireComponent(typeof(Rigidbody))]
public class Dart : MonoBehaviour
{
    [Header("Damage")]
    public float damage = 10f;

    [Header("Flight")]
    public float speed = 25f;
    public float lifetime = 5f;

    [Header("Effects (set to 0 to disable)")]
    [Range(0f, 1f)] public float slowMultiplier = 0.5f; // 0.5 = half speed
    public float slowDuration = 2f;
    public float poisonDamagePerSecond = 0f;
    public float poisonDuration = 0f;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic; // stops tunnelling at high speed
    }

    void Start()
    {
        rb.linearVelocity = transform.forward * speed; // use rb.velocity on Unity 2022 or older
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Ignore other enemies
        if (collision.collider.CompareTag("Enemy")) return;

        PlayerHealth player = collision.collider.GetComponentInParent<PlayerHealth>();
        if (player != null)
        {
            player.TakeDamage(damage);

            PlayerStatus status = player.GetComponent<PlayerStatus>();
            if (status != null)
            {
                if (slowDuration > 0f && slowMultiplier < 1f)
                    status.ApplySlow(slowMultiplier, slowDuration);
                if (poisonDuration > 0f && poisonDamagePerSecond > 0f)
                    status.ApplyPoison(poisonDamagePerSecond, poisonDuration);
            }
        }

        Destroy(gameObject);
    }
}
