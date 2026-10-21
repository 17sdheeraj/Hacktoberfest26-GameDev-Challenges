
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class BurgerInteraction : MonoBehaviour
{
    [Header("Interaction")]
    public Key interactionKey = Key.E;
    public Vector2 spawnOffset = new Vector2(1.5f, 0.5f);

    [Header("Burger Settings")]
    public int maxBurgers = 3;
    public float burgerLifetime = 10f;

    private bool playerNearby;
    private readonly List<GameObject> burgers = new List<GameObject>();

    void Update()
    {
        if (playerNearby &&
            Keyboard.current != null &&
            Keyboard.current[interactionKey].wasPressedThisFrame)
        {
            SpawnBurger();
        }
    }

    void SpawnBurger()
    {
        // Remove references to burgers that have been destroyed.
        burgers.RemoveAll(burger => burger == null);

        if (burgers.Count >= maxBurgers)
        {
            Debug.Log("Burger limit reached! Maximum: " + maxBurgers);
            return;
        }

        Vector3 spawnPosition = transform.position + (Vector3)spawnOffset;

        GameObject burger = new GameObject("Spawned Burger");
        burger.transform.position = spawnPosition;

        CreateBurgerPart(burger.transform, "Bottom Bun",
            new Vector2(0f, -0.12f), new Vector2(0.8f, 0.2f),
            new Color(0.76f, 0.42f, 0.12f));

        CreateBurgerPart(burger.transform, "Patty",
            new Vector2(0f, 0.02f), new Vector2(0.75f, 0.16f),
            new Color(0.32f, 0.14f, 0.06f));

        CreateBurgerPart(burger.transform, "Cheese",
            new Vector2(0f, 0.12f), new Vector2(0.85f, 0.08f),
            Color.yellow);

        CreateBurgerPart(burger.transform, "Top Bun",
            new Vector2(0f, 0.24f), new Vector2(0.8f, 0.22f),
            new Color(0.92f, 0.62f, 0.22f));

        // Add physics to make the burger fall.
        Rigidbody2D rb = burger.AddComponent<Rigidbody2D>();
        rb.gravityScale = 2f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // Give the burger a collider so it can land on the ground.
        BoxCollider2D collider = burger.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(0.85f, 0.5f);

        burgers.Add(burger);

        // Remove old burgers after a short time.
        Destroy(burger, burgerLifetime);

        Debug.Log("Burger spawned! (" + burgers.Count + "/" + maxBurgers + ")");
    }

    void CreateBurgerPart(Transform parent, string partName,
        Vector2 localPosition, Vector2 size, Color color)
    {
        GameObject part = new GameObject(partName);
        part.transform.SetParent(parent, false);
        part.transform.localPosition = localPosition;

        SpriteRenderer renderer = part.AddComponent<SpriteRenderer>();
        renderer.sprite = CreateSquareSprite();
        renderer.color = color;
        part.transform.localScale = size;
    }

    Sprite CreateSquareSprite()
    {
        Texture2D texture = Texture2D.whiteTexture;

        return Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f),
            Mathf.Max(texture.width, texture.height)
        );
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            Debug.Log("Press E to spawn a burger!");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}
