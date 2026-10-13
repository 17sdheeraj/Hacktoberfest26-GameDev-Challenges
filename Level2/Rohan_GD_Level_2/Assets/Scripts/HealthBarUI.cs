using UnityEngine;
using UnityEngine.UI;

// Put on a UI Slider. Auto-connects to the player's health.
[RequireComponent(typeof(Slider))]
public class HealthBarUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void OnEnable()
    {
        playerHealth.onHealthChanged.AddListener(UpdateBar);
    }

    void OnDisable()
    {
        playerHealth.onHealthChanged.RemoveListener(UpdateBar);
    }

    void UpdateBar(float current, float max)
    {
        slider.value = current / max;
    }
}
