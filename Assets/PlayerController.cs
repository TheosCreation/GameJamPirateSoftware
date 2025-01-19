using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    // Movement smoothing factor
    public float acceleration = 5f;
    public float deceleration = 5f;
    public float moveSpeed = 10f;
    [HideInInspector] public float originalMoveSpeed = 10f;
    public Sword sword;

    private float currentHealth = 0f;
    public float maxHealth = 0f;
    public float Health
    {
        get { return currentHealth; }
        protected set
        {
            currentHealth = Mathf.Clamp(value, 0f, maxHealth);
            UiManager.Instance.playerHud.healthBar.UpdateBar(currentHealth / maxHealth);
            if (currentHealth <= 0f)
            {
                Die();
            }
        }
    }
    public List<ScalableUpgrade> upgrades = new List<ScalableUpgrade>();

    public Action OnDeath;

    public bool isMoving = false;

    Vector2 axisInput = Vector2.zero;

    private void Awake()
    {
        originalMoveSpeed = moveSpeed;
        currentHealth = maxHealth;

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

    private void Die()
    {
        OnDeath?.Invoke();
        //Destroy(gameObject);
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

    public void GiveItem(ScalableUpgrade upgrade)
    {
        // Check how many upgrades of the same type already exist
        int existingCount = upgrades.Count(u => u.type == upgrade.type);

        // Apply the upgrade with the new stack count
        upgrade.ApplyUpgrade(this, existingCount);

        // Add the new upgrade to the list
        upgrades.Add(upgrade);
    }
}
