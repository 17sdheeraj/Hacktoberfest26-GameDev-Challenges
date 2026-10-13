using UnityEngine;

// Simple hitscan gun. Attach to the Player; assign the camera.
public class PlayerShooting : MonoBehaviour
{
    public Camera playerCamera;
    public float damage = 25f;
    public float range = 100f;
    public float fireRate = 0.2f;
    public AudioSource shootSound;       // optional

    float nextFireTime;

    void Update()
    {
        if (Time.timeScale == 0f) return;

        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        if (shootSound != null) shootSound.Play();

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            EnemyHealth enemy = hit.collider.GetComponentInParent<EnemyHealth>();
            if (enemy != null) enemy.TakeDamage(damage);
        }
    }
}
