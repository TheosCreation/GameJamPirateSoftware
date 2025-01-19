using UnityEngine;

public class EnemyRanged : Enemy
{
    public float shootingDistance = 10f;
    public GameObject projectilePrefab;
    public float shootingCooldown = 2f;
    private float shootTimer = 0f;

    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        shootTimer += Time.fixedDeltaTime;

        if (target != null && Vector3.Distance(transform.position, target.position) < shootingDistance)
        {
            if (shootTimer > shootingCooldown && HasLineOfSight())
            {
                shootTimer = 0;

                Shoot();
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
