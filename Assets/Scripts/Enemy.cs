using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float maxHealth = 2000f;
    [SerializeField] private float currentHealth;
    [SerializeField] GameObject healthBarPrefab;
    private UiBar healthBarRef;
    public float despawnDistance = 1000f;
    public Transform target;
    public GameObject healthBarCanvas;
    private Rigidbody2D rb;
    private NavMeshAgent agent;
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

    public Action OnDeath;
    public Action OnDespawn;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();

        healthBarCanvas =  Instantiate(healthBarPrefab,transform);
        healthBarRef = healthBarCanvas.GetComponentInChildren<UiBar>();

        Health = maxHealth;
    }

    private void Update()
    {
        healthBarCanvas.transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward);
    }

    void FixedUpdate()
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

    private void Die()
    {
        OnDeath.Invoke();
        Destroy(gameObject);
    }

    private void Despawn()
    {
        OnDespawn.Invoke();
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
