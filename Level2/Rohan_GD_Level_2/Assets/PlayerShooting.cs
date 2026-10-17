
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Gun gun;
    [SerializeField] private SpriteRenderer playerSprite;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (gun == null)
            gun = GetComponentInChildren<Gun>();

        if (playerSprite == null)
            playerSprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (mainCamera == null || gun == null) return;

        Vector3 mousePosition =
            mainCamera.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction =
            (Vector2)(mousePosition - gun.transform.position);

        if (direction.sqrMagnitude < 0.001f) return;

        // Rotate the gun toward the mouse
        gun.transform.right = direction.normalized;

        // Flip the character when aiming left
        if (playerSprite != null)
            playerSprite.flipX = direction.x < 0f;

        if (Input.GetMouseButton(0))
{
    Debug.Log("Shooting input detected");
    gun.Shoot();
}
    }
}
