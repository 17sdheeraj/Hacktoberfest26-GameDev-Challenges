
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerColorChanger : MonoBehaviour
{
    private SpriteRenderer[] spriteRenderers;

    private readonly Color[] colors =
    {
        Color.red,
        Color.yellow,
        Color.blue,
        Color.white
    };

    private int currentColorIndex = 0;

    void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
    }

    void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.zKey.wasPressedThisFrame)
        {
            ChangeColor();
        }
    }

    void ChangeColor()
    {
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.color = colors[currentColorIndex];
        }

        currentColorIndex = (currentColorIndex + 1) % colors.Length;
    }
}
