using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : Enemy
{
    public float shootingDistance = 10f;
    public GameObject projectilePrefab;
    private Timer timer;
    public float shootingCooldown = 2f;
    private bool canShoot = true;

    protected override void Start()
    {
        base.Start();
        timer = gameObject.AddComponent<Timer>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (target != null && Vector3.Distance(transform.position, target.position) < shootingDistance)
        {
            if (canShoot)
            {
                Vector3 direction = target.position - transform.position;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction);
 
                Debug.DrawRay(transform.position, direction, Color.red);

                if (hit.collider != null)
                {
                    Debug.Log("Raycast hit: " + hit.collider.name);

                    if (hit.collider.CompareTag("Player"))
                    {
                        Shoot();
                        canShoot = false;
                        timer.StopTimer();
                        timer.SetTimer(shootingCooldown, () => canShoot = true);
                    }
                }

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
