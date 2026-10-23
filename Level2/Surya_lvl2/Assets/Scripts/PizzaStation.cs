
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PizzaStation : MonoBehaviour
{
    public GameObject player;
    public int maxPizzas = 3;
    public float pizzaLifetime = 10f;
    public float spawnDistance = 1.5f;

    private readonly List<GameObject> activePizzas = new List<GameObject>();

    void Update()
    {
        if (player == null)
            return;

        float distance = Vector2.Distance(
            transform.position,
            player.transform.position
        );

        if (distance <= 2f && Keyboard.current != null
            && Keyboard.current.pKey.wasPressedThisFrame)
        {
            SpawnPizza();
        }
    }

    void SpawnPizza()
    {
        activePizzas.RemoveAll(pizza => pizza == null);

        if (activePizzas.Count >= maxPizzas)
        {
            Debug.Log("Maximum of 3 pizzas reached!");
            return;
        }

        Vector3 spawnPosition = player.transform.position
            + new Vector3(spawnDistance, 0.5f, 0f);

        GameObject pizza = new GameObject("Pizza");
        pizza.transform.position = spawnPosition;

        SpriteRenderer renderer = pizza.AddComponent<SpriteRenderer>();
        renderer.sprite = CreatePizzaSprite();
        renderer.sortingOrder = 2;

        CircleCollider2D collider = pizza.AddComponent<CircleCollider2D>();
        collider.radius = 0.5f;

        Rigidbody2D rb = pizza.AddComponent<Rigidbody2D>();
        rb.gravityScale = 3f;

        activePizzas.Add(pizza);
        StartCoroutine(RemovePizzaAfterTime(pizza));
    }

   
Sprite CreatePizzaSprite()
{
    int size = 64;
    Texture2D texture = new Texture2D(size, size);
    Color[] pixels = new Color[size * size];

    Color transparent = new Color(0f, 0f, 0f, 0f);
    Color crust = new Color(0.65f, 0.28f, 0.08f);
    Color cheese = new Color(1f, 0.65f, 0.1f);
    Color pepperoni = new Color(0.8f, 0.1f, 0.08f);

    for (int y = 0; y < size; y++)
    {
        for (int x = 0; x < size; x++)
        {
            Color color = transparent;

            // Triangle with a wide top and a downward-pointing tip.
            float halfWidth = (y - 8f) * 0.5f;

            if (y >= 8 && y <= 48 &&
                Mathf.Abs(x - 32) <= halfWidth)
            {
                color = y <= 13 ? crust : cheese;

                // Pepperoni toppings.
                if ((x - 22) * (x - 22) + (y - 32) * (y - 32) < 16 ||
                    (x - 42) * (x - 42) + (y - 32) * (y - 32) < 16 ||
                    (x - 32) * (x - 32) + (y - 20) * (y - 20) < 16)
                {
                    color = pepperoni;
                }
            }

            pixels[y * size + x] = color;
        }
    }

    texture.SetPixels(pixels);
    texture.Apply();

    return Sprite.Create(
        texture,
        new Rect(0, 0, size, size),
        new Vector2(0.5f, 0.5f),
        64f
    );
}

    IEnumerator RemovePizzaAfterTime(GameObject pizza)
    {
        yield return new WaitForSeconds(pizzaLifetime);

        activePizzas.Remove(pizza);

        if (pizza != null)
            Destroy(pizza);
    }
}
