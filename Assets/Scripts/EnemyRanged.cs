using UnityEngine;

public class EnemyRanged : Enemy
{
    public float shootingDistance = 10f;
    public GameObject projectilePrefab;
    public float shootingCooldown = 2f;
    public float reactionTime = 0.5f;
    private float shootTimer = 0f;
    private float timer2 = 0f;

    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (target != null && Vector3.Distance(transform.position, target.position) < shootingDistance)
        {
            // Increment the shoot timer
            shootTimer += Time.fixedDeltaTime;
            timer2 += Time.fixedDeltaTime;
            if (HasLineOfSight())
            {
                if (timer2 > reactionTime && shootTimer > shootingCooldown)
                {
                    shootTimer = 0;
                    Shoot();
                }
            }
            else 
            {
                timer2 = 0;
            }
            
        }
    }


    private void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().Initialize(target.position);
        Destroy(projectile, 20);
    }
}
