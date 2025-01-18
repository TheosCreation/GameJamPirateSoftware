using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float maxHealth = 2000f;
    [SerializeField] private float currentHealth;
    [SerializeField] GameObject healthBarPrefab;
    private UiBar healthBarRef;
    public GameObject player;
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 5f;
    public float Health
    {
        get { return currentHealth; }
        private set
        {
            currentHealth = Mathf.Clamp(value, 0f, maxHealth);
            if (healthBarRef  != null )
            {
                healthBarRef.UpdateBar(currentHealth/maxHealth);
            }
            if (currentHealth <= 0f)
            {
                Die();
            }
        }
    }

    void Start()
    {
        Health = maxHealth;
        player = LevelManager.Instance.player;
        rb = GetComponent<Rigidbody2D>();
        GameObject healthBar =  Instantiate(healthBarPrefab,transform);
        healthBarRef = healthBar.GetComponentInChildren<UiBar>();
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;

            rb.velocity = direction * moveSpeed;
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Sword swordRef;
        if (swordRef = collision.gameObject.GetComponent<Sword>())
        {
        
           Health -= swordRef.GetCurrentSwingSpeed();
        }
    }
}
