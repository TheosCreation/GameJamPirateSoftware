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
    public GameObject swordPrefab; 
    public List<Sword> swords = new List<Sword>();
    public UiBar healthBarRef;

    private float currentHealth = 0f;
    public float maxHealth = 500f;
    public float Health
    {
        get { return currentHealth; }
        protected set
        {
            currentHealth = Mathf.Clamp(value, 0f, maxHealth);
            UiManager.Instance.playerHud.healthBar.UpdateBar(currentHealth / maxHealth, currentHealth.ToString("F0") + "/" + maxHealth);
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
        AddSword();
        UiManager.Instance.playerHud.healthBar.UpdateBar(currentHealth / maxHealth, currentHealth.ToString("F0") + "/" + maxHealth);
        InputManager.Instance.playerInput.Universal.Escape.started += _ctx => PauseManager.Instance.TogglePause();
    }
    protected virtual void Start()
    {
        Health = maxHealth;
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
    public void TakeDamage(float _damage)
    {
        Health -= _damage;
    }
    public void Heal(float _health)
    {
        Health += _health;
    }
    public void AddSword()
    {
        Sword newSword = Instantiate(swordPrefab, transform).GetComponent<Sword>();
        newSword.GetComponent<HingeJoint2D>().connectedBody = rb;
        swords.Add(newSword);
        UpdateSwordPositions();
    }
    private void UpdateSwordPositions()
    {
        int swordCount = swords.Count;
        float angleIncrement = 360f / swordCount;

        for (int i = 0; i < swordCount; i++)
        {
            float angle = i * angleIncrement;
            swords[i].transform.localRotation = Quaternion.Euler(0, 0, angle);
            swords[i].rotationOffset = angle;
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
