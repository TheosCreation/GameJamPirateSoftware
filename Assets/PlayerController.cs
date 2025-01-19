using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    // Movement smoothing factor
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float deceleration = 5f;
    [SerializeField] private float moveSpeed = 10f;

    public bool isMoving = false;

    Vector2 axisInput = Vector2.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        InputManager.Instance.playerInput.Universal.Escape.started += _ctx => PauseManager.Instance.TogglePause();
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
        axisInput = InputManager.Instance.MovementVector;
        if (axisInput.magnitude > 0)
        {
            Vector3 upMovement = Vector3.up * axisInput.y; // Forward/backward
            Vector3 rightMovement = Vector3.right * axisInput.x; // Sideways (strafe)
            Vector2 direction = (upMovement + rightMovement).normalized;

            Vector2 targetVelocity = direction * moveSpeed;

            Vector2 newVelocity = Vector2.MoveTowards(
                rb.velocity,
                targetVelocity,
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
        Vector2 currentVelocity = rb.velocity;

        // Gradually reduce horizontal velocity to zero
        Vector2 newVelocity = Vector2.MoveTowards(
            rb.velocity,
            Vector2.zero,  
            deceleration * Time.fixedDeltaTime                 // Deceleration amount
        );

        // Combine the adjusted horizontal velocity with the current vertical velocity
        rb.velocity = new Vector2(newVelocity.x, newVelocity.y);
    }
}
