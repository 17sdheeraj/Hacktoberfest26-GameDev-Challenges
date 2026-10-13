using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 50f;
    float current;

    void Awake()
    {
        current = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        current -= amount;
        if (current <= 0f) Die();
    }

    void Die()
    {
        // Add death particles / sound / score here
        Destroy(gameObject);
    }
}
