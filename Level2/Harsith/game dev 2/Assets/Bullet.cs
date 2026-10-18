using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 12f;
    public float lifetime = 2f; // auto-delete so bullets don't pile up forever

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}
