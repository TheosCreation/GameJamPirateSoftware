using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private Rigidbody2D rb;
    private Camera mainCamera;

    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        RotateTowardsMouse();
        LimitSpeed();
    }

    private void RotateTowardsMouse()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(InputManager.mousePosition);
        Vector2 direction = mousePosition - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        float rotation = Mathf.LerpAngle(rb.rotation, angle, rotationSpeed * Time.fixedDeltaTime);
        rb.rotation = rotation;
    }


    private void LimitSpeed()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
}


