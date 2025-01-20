using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float maxHealth = 2000f;
    [SerializeField] protected float currentHealth;
    [SerializeField] GameObject healthBarPrefab;
    public float damage = 100;
    protected UiBar healthBarRef;
    public float despawnDistance = 1000f;
    public Transform target;
    public GameObject healthBarCanvas;
    protected Rigidbody2D rb;
    protected NavMeshAgent agent;
    [SerializeField] protected GameObject enemyVisuals;
    [SerializeField] protected float moveSpeed = 5f;
    public float Health
    {
        get { return currentHealth; }
        protected set
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

    public Action OnDeath;
    public Action OnDespawn;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();

        healthBarCanvas =  Instantiate(healthBarPrefab, enemyVisuals.transform);
        healthBarRef = healthBarCanvas.GetComponentInChildren<UiBar>();

        Health = maxHealth;
    }

    protected void LateUpdate()
    {
        enemyVisuals.transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward);
    }

    protected virtual void FixedUpdate()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
            if(Vector3.Distance(transform.position, target.position) > despawnDistance)
            {
                Despawn();
            }
        }
    }
    public void TakeDamage(float _damage)
    {
        Health -= _damage;   
    }
    protected void Die()
    {
        OnDeath.Invoke();
        Destroy(gameObject);
    }

    protected void Despawn()
    {
        OnDespawn.Invoke();
        Destroy(gameObject);
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        Sword swordRef;
        PlayerController playerRef;
        if (swordRef = collision.gameObject.GetComponent<Sword>())
        {

            Health -= swordRef.GetCurrentSwingSpeed();
        }
        else if (playerRef = collision.gameObject.GetComponent<PlayerController>())
        {
            playerRef.TakeDamage(damage);
            rb.AddForce(playerRef.transform.position - transform.position*100);
        }
    }

    protected bool HasLineOfSight()
    {

        Vector3 direction = target.position - transform.position;
        Debug.DrawRay(transform.position, direction, Color.red);
        if (Physics2D.Raycast(transform.position, direction))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
