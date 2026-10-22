
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RandomObjectSummoner : MonoBehaviour
{
    [Header("Interaction")]
    public Key interactionKey = Key.F;
    public Vector2 spawnOffset = new Vector2(1.5f, 0.5f);

    [Header("Object Settings")]
    public int maxObjects = 3;
    public float objectLifetime = 10f;

    private bool playerNearby;
    private readonly List<GameObject> spawnedObjects = new List<GameObject>();

    void Update()
    {
        if (playerNearby &&
            Keyboard.current != null &&
            Keyboard.current[interactionKey].wasPressedThisFrame)
        {
            SummonRandomObject();
        }
    }

    void SummonRandomObject()
    {
        spawnedObjects.RemoveAll(item => item == null);

        if (spawnedObjects.Count >= maxObjects)
        {
            Debug.Log("Summon limit reached! Maximum: " + maxObjects);
            return;
        }

        string[] objectNames = { "Magic Star", "Bouncy Ball", "Mystery Crate" };
        Color[] objectColors = { Color.yellow, Color.cyan, Color.magenta };

        int choice = Random.Range(0, objectNames.Length);
        Vector3 spawnPosition = transform.position + (Vector3)spawnOffset;

        GameObject item = new GameObject(objectNames[choice]);
        item.transform.position = spawnPosition;

        SpriteRenderer renderer = item.AddComponent<SpriteRenderer>();
        renderer.sprite = CreateSquareSprite();
        renderer.color = objectColors[choice];

        // Give the mystery object a distinct shape or size.
        if (choice == 1)
        {
            item.transform.localScale = new Vector3(0.65f, 0.65f, 1f);
        }
        else
        {
            item.transform.localScale = new Vector3(0.85f, 0.85f, 1f);
        }

        Rigidbody2D rb = item.AddComponent<Rigidbody2D>();
        rb.gravityScale = 2f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        BoxCollider2D collider = item.AddComponent<BoxCollider2D>();
        collider.size = Vector2.one;

        spawnedObjects.Add(item);
        Destroy(item, objectLifetime);

        Debug.Log("Summoned: " + objectNames[choice] +
                  " (" + spawnedObjects.Count + "/" + maxObjects + ")");
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
            Debug.Log("Press F to summon a random object!");
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
