
using UnityEngine;

public class SuperSaiyanAbility : MonoBehaviour
{
    [Header("Ability Settings")]
    [SerializeField] private KeyCode activationKey = KeyCode.E;
    [SerializeField] private float duration = 6f;
    [SerializeField] private float cooldown = 12f;

    [Header("Gameplay Boosts")]
    [SerializeField] private float speedMultiplier = 1.5f;
    [SerializeField] private float fireRateMultiplier = 1.5f;

    [Header("Character Sprites")]
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite superSaiyanSprite;

    public bool IsActive { get; private set; }

    public float SpeedMultiplier =>
        IsActive ? speedMultiplier : 1f;

    public float FireRateMultiplier =>
        IsActive ? fireRateMultiplier : 1f;

    private float endTime;
    private float nextReadyTime;

    private void Awake()
    {
        // Find the character's SpriteRenderer automatically
        if (playerSprite == null)
            playerSprite = GetComponentInChildren<SpriteRenderer>();

        // Remember the original sprite if one wasn't assigned
        if (playerSprite != null && normalSprite == null)
            normalSprite = playerSprite.sprite;
    }

    private void Update()
    {
        if (Input.GetKeyDown(activationKey) &&
            !IsActive &&
            Time.time >= nextReadyTime)
        {
            Activate();
        }

        if (IsActive && Time.time >= endTime)
        {
            EndTransformation();
        }
    }

    private void Activate()
    {
        IsActive = true;
        endTime = Time.time + duration;

        // Change to Super Saiyan sprite
        if (playerSprite != null && superSaiyanSprite != null)
        {
            playerSprite.sprite = superSaiyanSprite;
        }
        else
        {
            Debug.LogWarning(
                "Assign the Player Sprite Renderer and Super Saiyan Sprite!"
            );
        }

        Debug.Log("Super Saiyan activated!");
    }

    private void EndTransformation()
    {
        IsActive = false;
        nextReadyTime = Time.time + cooldown;

        // Restore the original sprite
        if (playerSprite != null && normalSprite != null)
        {
            playerSprite.sprite = normalSprite;
        }

        Debug.Log("Transformation ended. Ability is recharging.");
    }

    public float GetCooldownRemaining()
    {
        if (IsActive)
            return 0f;

        return Mathf.Max(0f, nextReadyTime - Time.time);
    }
}
