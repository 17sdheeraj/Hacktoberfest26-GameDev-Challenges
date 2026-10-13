using System.Collections;
using UnityEngine;

// Handles the "effects" darts can apply: slow and poison.
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerHealth))]
public class PlayerStatus : MonoBehaviour
{
    PlayerController controller;
    PlayerHealth health;
    Coroutine slowRoutine;
    Coroutine poisonRoutine;

    void Awake()
    {
        controller = GetComponent<PlayerController>();
        health = GetComponent<PlayerHealth>();
    }

    public void ApplySlow(float speedMultiplier, float duration)
    {
        if (slowRoutine != null) StopCoroutine(slowRoutine); // refresh instead of stacking
        slowRoutine = StartCoroutine(SlowCoroutine(speedMultiplier, duration));
    }

    public void ApplyPoison(float damagePerSecond, float duration)
    {
        if (poisonRoutine != null) StopCoroutine(poisonRoutine);
        poisonRoutine = StartCoroutine(PoisonCoroutine(damagePerSecond, duration));
    }

    IEnumerator SlowCoroutine(float multiplier, float duration)
    {
        controller.speedMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        controller.speedMultiplier = 1f;
        slowRoutine = null;
    }

    IEnumerator PoisonCoroutine(float dps, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            yield return new WaitForSeconds(1f);
            elapsed += 1f;
            health.TakeDamage(dps);
        }
        poisonRoutine = null;
    }
}
