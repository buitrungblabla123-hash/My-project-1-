using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("Bullet")]
    public float speed = 5f;

    [Header("Damage")]
    public int damage = 1;

    [Header("Thời gian tồn tại")]
    public float lifeTime = 5f;

    private Vector2 direction = Vector2.down;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    
    public void SetDirection(Vector2 newDirection)
    {
        if (newDirection == Vector2.zero)
        {
            direction = Vector2.down;
        }
        else
        {
            direction =
                newDirection.normalized;
        }
        
        transform.rotation =
            Quaternion.identity;
    }
    
    void Update()
    {
        transform.Translate(
            direction *
            speed *
            Time.deltaTime,
            Space.World
        );
    }
    
    private void OnTriggerEnter2D(
        Collider2D other)
    {
        PlayerController player =
            other.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            player.TakeDamage(damage);
            
            Destroy(gameObject);
        }
    }
    
    private void OnCollisionEnter2D(
        Collision2D collision)
    {
        PlayerController player =
            collision.gameObject
                .GetComponentInParent<PlayerController>();

        if (player != null)
        {
            player.TakeDamage(damage);

            
            Destroy(gameObject);
        }
    }
}