using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float CurrentHealth { get; private set; }

    public UnityEvent<float, float> onHealthChanged; // (current, max)
    public UnityEvent onDamaged;                     // hook up a hurt sound / red flash
    public UnityEvent onDeath;

    bool dead;

    void Awake()
    {
        CurrentHealth = maxHealth;
    }

    void Start()
    {
        onHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }

    public void TakeDamage(float amount)
    {
        if (dead) return;

        CurrentHealth = Mathf.Max(0f, CurrentHealth - amount);
        onHealthChanged?.Invoke(CurrentHealth, maxHealth);
        onDamaged?.Invoke();

        if (CurrentHealth <= 0f) Die();
    }

    public void Heal(float amount)
    {
        if (dead) return;
        CurrentHealth = Mathf.Min(maxHealth, CurrentHealth + amount);
        onHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }

    void Die()
    {
        dead = true;
        onDeath?.Invoke();
        if (GameManager.Instance != null) GameManager.Instance.GameOver();
    }
}
