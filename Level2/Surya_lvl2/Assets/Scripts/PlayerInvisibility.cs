
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInvisibility : MonoBehaviour
{
    private SpriteRenderer[] spriteRenderers;
    private bool isInvisible = false;

    void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
    }

    void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.iKey.wasPressedThisFrame)
        {
            isInvisible = !isInvisible;

            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                spriteRenderer.enabled = !isInvisible;
            }

            Debug.Log(isInvisible ? "Player is invisible!" : "Player is visible!");
        }
    }
}
