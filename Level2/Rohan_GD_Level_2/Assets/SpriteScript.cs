using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float sprintMultiplier = 1.75f;
    public Rigidbody2D body;

    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");
        Vector2 direction = new Vector2(xInput, yInput).normalized;

        float currentSpeed = speed;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed *= sprintMultiplier;
        }

        body.linearVelocity = direction * currentSpeed;
    }
}