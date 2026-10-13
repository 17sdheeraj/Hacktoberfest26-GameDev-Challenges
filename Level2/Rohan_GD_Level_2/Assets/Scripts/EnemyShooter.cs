using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("References")]
    public GameObject dartPrefab;
    public Transform firePoint;          // empty child object at the "gun" position
    public Transform target;             // leave empty to auto-find object tagged "Player"

    [Header("Behaviour")]
    public float detectionRange = 20f;
    public float fireRate = 1.5f;        // seconds between shots
    public float turnSpeed = 5f;
    public float aimInaccuracy = 2f;     // degrees of random spread
    public LayerMask lineOfSightBlockers = ~0; // walls etc.

    float nextFireTime;

    void Start()
    {
        if (target == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) target = p.transform;
        }
    }

    void Update()
    {
        if (target == null) return;

        Vector3 toTarget = target.position - transform.position;
        if (toTarget.magnitude > detectionRange) return;
        if (!HasLineOfSight()) return;

        // Turn toward player (horizontal only)
        Vector3 flat = new Vector3(toTarget.x, 0f, toTarget.z);
        if (flat.sqrMagnitude > 0.01f)
        {
            Quaternion look = Quaternion.LookRotation(flat);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, turnSpeed * Time.deltaTime);
        }

        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    bool HasLineOfSight()
    {
        Vector3 origin = firePoint != null ? firePoint.position : transform.position + Vector3.up;
        Vector3 aimPoint = target.position + Vector3.up * 1f;
        Vector3 dir = aimPoint - origin;

        if (Physics.Raycast(origin, dir.normalized, out RaycastHit hit, dir.magnitude, lineOfSightBlockers))
            return hit.transform == target || hit.transform.IsChildOf(target);

        return true;
    }

    void Shoot()
    {
        Vector3 origin = firePoint != null ? firePoint.position : transform.position + Vector3.up;
        Vector3 aimPoint = target.position + Vector3.up * 1f;
        Quaternion rot = Quaternion.LookRotation(aimPoint - origin);

        // Random spread so the enemy isn't a perfect shot
        rot *= Quaternion.Euler(Random.Range(-aimInaccuracy, aimInaccuracy),
                                Random.Range(-aimInaccuracy, aimInaccuracy), 0f);

        Instantiate(dartPrefab, origin, rot);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
