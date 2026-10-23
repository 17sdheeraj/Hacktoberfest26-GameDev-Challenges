
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 8f;

    // Reference to the player's shooting script
    [SerializeField] private PlayerShooting playerShooting;

    private float nextShotTime = 0f;

    private void Awake()
    {
        if (playerShooting == null)
            playerShooting = GetComponentInParent<PlayerShooting>();
    }

    public void Shoot()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogError("Gun: Assign Bullet Prefab and Fire Point!");
            return;
        }

        // Get the Super Saiyan fire-rate multiplier
        float multiplier = playerShooting != null
            ? playerShooting.GetFireRateMultiplier()
            : 1f;

        float currentFireRate = fireRate * multiplier;

        if (Time.time < nextShotTime)
            return;

        nextShotTime = Time.time +
            1f / Mathf.Max(0.01f, currentFireRate);

        Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation
        );
    }
}
