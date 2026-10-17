
using UnityEngine;


public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 8f;

    private float nextShotTime = 0f;

    public void Shoot()
    {
        Debug.Log("Gun Shoot() called");
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogError("Gun: Assign Bullet Prefab and Fire Point!");
            return;
        }

        if (Time.time < nextShotTime)
            return;

        nextShotTime = Time.time + 1f / Mathf.Max(0.01f, fireRate);

        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
