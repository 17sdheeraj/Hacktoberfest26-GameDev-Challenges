using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float fireRate = 0.25f; // seconds between shots

    float nextFireTime;

    void Update()
    {
        // Hold left mouse button to keep shooting
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mouseWorld - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, angle));
    }
}
