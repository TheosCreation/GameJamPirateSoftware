using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 50f;
    private Vector3 direction;
    public bool reflected = false;
    CircleCollider2D circleCollider;
    public void Initialize(Vector3 targetPosition)
    {
        direction = (targetPosition - transform.position).normalized;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(direction * speed);
        circleCollider = rb.GetComponent<CircleCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        if (reflected)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Enemy enemyRef = collision.gameObject.GetComponent<Enemy>();
                if (enemyRef != null)
                {
                    Debug.Log("DO0 DAMAGE " + damage);
                    enemyRef.TakeDamage(damage);
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
                Destroy(gameObject);
            }
            else if (collision.gameObject.CompareTag("Sword"))
            {
                reflected = true;
                GetComponent<SpriteRenderer>().color = Color.green;
                circleCollider.excludeLayers = 0; //Did consider not allowing player collisions but projectile mostly goes through the player and not reflected
            }
        }
    }
}
