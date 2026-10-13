using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float gravity = -20f;
    public float lookSensitivity = 2f;
    public Transform cameraTransform;   // drag your child Camera here

    // PlayerStatus changes this (e.g. 0.5 = half speed while slowed)
    [HideInInspector] public float speedMultiplier = 1f;

    CharacterController cc;
    float yVelocity;
    float pitch;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        // Look
        transform.Rotate(0f, Input.GetAxis("Mouse X") * lookSensitivity, 0f);
        pitch = Mathf.Clamp(pitch - Input.GetAxis("Mouse Y") * lookSensitivity, -80f, 80f);
        cameraTransform.localEulerAngles = new Vector3(pitch, 0f, 0f);

        // Move
        Vector3 move = transform.right * Input.GetAxis("Horizontal")
                     + transform.forward * Input.GetAxis("Vertical");

        if (cc.isGrounded && yVelocity < 0f) yVelocity = -2f;
        yVelocity += gravity * Time.deltaTime;

        Vector3 velocity = move * moveSpeed * speedMultiplier + Vector3.up * yVelocity;
        cc.Move(velocity * Time.deltaTime);
    }
}
