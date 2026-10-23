
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float sprintMultiplier = 1.75f;
    public Rigidbody2D body;

    // Add this reference to your Super Saiyan ability
    public SuperSaiyanAbility superSaiyan;

    void Awake()
    {
        // Automatically find the ability on this same player
        if (superSaiyan == null)
        {
            superSaiyan = GetComponent<SuperSaiyanAbility>();
        }
    }

    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(xInput, yInput).normalized;

        float currentSpeed = speed;

        // Existing Shift sprint
        if (Input.GetKey(KeyCode.LeftShift) ||
            Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed *= sprintMultiplier;
        }

        // Super Saiyan speed boost
        if (superSaiyan != null)
        {
            currentSpeed *= superSaiyan.SpeedMultiplier;
        }

        body.linearVelocity = direction * currentSpeed;
    }
}
