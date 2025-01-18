using UnityEngine;

public class Sword : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float rotationSpeed = 10f;

    private float currentSwingSpeed;
    private float maxSwingSpeed;
    private float previousRotation;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        RotateTowardsMouse();
        CalculateSwingSpeed();
    }

    private void CalculateSwingSpeed()
    {
        float rotationDelta = Mathf.DeltaAngle(previousRotation, rb.rotation);

        currentSwingSpeed = Mathf.Abs(rotationDelta / Time.fixedDeltaTime);

        if (currentSwingSpeed > maxSwingSpeed)
        {
            maxSwingSpeed = currentSwingSpeed;
        }

        previousRotation = rb.rotation;
    }


    private void RotateTowardsMouse()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(InputManager.mousePosition);
        Vector2 direction = mousePosition - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        float rotation = Mathf.LerpAngle(rb.rotation, angle, rotationSpeed * Time.deltaTime);
        rb.MoveRotation(rotation);
    }
    public float GetCurrentSwingSpeed()
    {
        return currentSwingSpeed;
    }
    public float GetMaxSwingSpeed()
    {
        return maxSwingSpeed;
    }
}
