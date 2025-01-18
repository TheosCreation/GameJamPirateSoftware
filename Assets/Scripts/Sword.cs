using UnityEngine;

public class Sword : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        RotateTowardsMouse();
    }

    private void RotateTowardsMouse()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(InputManager.mousePosition);
        Vector2 direction = mousePosition - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        float rotation = Mathf.LerpAngle(rb.rotation, angle, rotationSpeed * Time.deltaTime);
        rb.MoveRotation(rotation);
    }
}