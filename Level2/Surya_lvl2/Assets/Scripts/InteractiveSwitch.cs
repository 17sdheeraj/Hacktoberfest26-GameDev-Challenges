
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractiveSwitch : MonoBehaviour
{
    [Header("Switch Settings")]
    public Key interactionKey = Key.M;
    public float interactionRange = 2f;

    [Header("Objects to Toggle")]
    public GameObject targetObject;

    [Header("Visual Feedback")]
    public Color offColor = Color.gray;
    public Color onColor = Color.yellow;

    private Transform player;
    private SpriteRenderer switchRenderer;
    private bool isOn = true;

    void Start()
    {
        switchRenderer = GetComponent<SpriteRenderer>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;

        if (targetObject != null)
            targetObject.SetActive(isOn);
    }

    void Update()
    {
        if (player == null || Keyboard.current == null)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= interactionRange &&
            Keyboard.current[interactionKey].wasPressedThisFrame)
        {
            isOn = !isOn;

            if (targetObject != null)
                targetObject.SetActive(isOn);

            if (switchRenderer != null)
                switchRenderer.color = isOn ? onColor : offColor;

            Debug.Log(isOn ? "Switch ON" : "Switch OFF");
        }
    }
}
