using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    // Movement smoothing factor
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float moveSpeed = 10f;

    public bool isMoving = false;

    Vector2 axisInput = Vector2.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        MoveWalk();

        if (!isMoving)
        {
            ApplyDeceleration();
        }

    }


    private void MoveWalk()
    {
        Vector2 currentVelocity = rb.velocity;

        axisInput = InputManager.Instance.MovementVector;
        if (axisInput.magnitude > 0)
        {
            Vector3 upMovement = transform.up * axisInput.y; // Forward/backward
            Vector3 rightMovement = transform.right * axisInput.x; // Sideways (strafe)
            Vector2 direction = (upMovement + rightMovement).normalized;

            Vector2 targetVelocity = direction * moveSpeed;

            Vector2 newVelocity = Vector2.MoveTowards(
                new Vector2(currentVelocity.x, currentVelocity.y), // Current horizontal velocity
                new Vector2(targetVelocity.x, currentVelocity.y),   // Target horizontal velocity
                acceleration * Time.fixedDeltaTime                 // Adjust by acceleration
            );

            // Combine with vertical velocity
            rb.velocity = new Vector2(newVelocity.x, newVelocity.y);

            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    private void ApplyDeceleration()
    {
        throw new NotImplementedException();
    }
}
