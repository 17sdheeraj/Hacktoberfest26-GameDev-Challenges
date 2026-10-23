
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShrink : MonoBehaviour
{
    public Vector3 smallScale = new Vector3(0.5f, 0.5f, 1f);

    private Vector3 originalScale;
    private bool isSmall = false;

    void Awake()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (Keyboard.current == null)
            return;

        bool shiftPressed =
            Keyboard.current.leftShiftKey.isPressed ||
            Keyboard.current.rightShiftKey.isPressed;

        if (shiftPressed && Keyboard.current.rKey.wasPressedThisFrame)
        {
            transform.localScale = smallScale;
            isSmall = true;
            Debug.Log("Player shrunk!");
        }
    }
}
