using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    public float minSpeed = 1.5f;
    public float maxSpeed = 3f;

    [Header("Giới hạn màn hình")]
    public float minX = -8f;
    public float maxX = 8f;
    public float minY = -4f;
    public float maxY = 4f;

    [Header("Health")]
    public int maxHealth = 3;
    public int currentHealth = 3;

    [Header("Explosion")]
    public GameObject explosionPrefab;

    private Vector2 moveDirection;
    private float moveSpeed;

    private SpriteRenderer spriteRenderer;
    private bool isDying = false;

    // Điểm Enemy sẽ bay tới khi vừa xuất hiện
    private Vector2 targetPosition;
    private bool movingToTarget = true;

    void Start()
    {
        currentHealth = maxHealth;

        spriteRenderer =
            GetComponentInChildren<SpriteRenderer>();

        moveSpeed = Random.Range(minSpeed, maxSpeed);

        // Nếu không được Spawner gán target
        // thì tự di chuyển bình thường
        if (!movingToTarget)
        {
            moveDirection =
                Random.insideUnitCircle.normalized;

            if (moveDirection == Vector2.zero)
                moveDirection = Vector2.right;
        }
    }

    public void SetTarget(Vector2 target)
    {
        targetPosition = target;
        movingToTarget = true;
    }

    void Update()
    {
        if (isDying)
            return;

        // =========================
        // GIAI ĐOẠN 1:
        // BAY TỪ NGOÀI VÀO TRONG
        // =========================

        if (movingToTarget)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            // Đã tới vị trí bên trong
            if (Vector2.Distance(
                transform.position,
                targetPosition
            ) < 0.1f)
            {
                movingToTarget = false;

                // Sau khi vào màn hình
                // bắt đầu bay random
                moveDirection =
                    Random.insideUnitCircle.normalized;

                if (moveDirection == Vector2.zero)
                    moveDirection = Vector2.right;
            }

            return;
        }

        // =========================
        // GIAI ĐOẠN 2:
        // DI CHUYỂN RANDOM TRONG MÀN
        // =========================

        transform.Translate(
            moveDirection *
            moveSpeed *
            Time.deltaTime,
            Space.World
        );

        Vector3 pos = transform.position;

        // Chạm trái
        if (pos.x <= minX)
        {
            pos.x = minX;
            moveDirection.x =
                Mathf.Abs(moveDirection.x);
        }

        // Chạm phải
        else if (pos.x >= maxX)
        {
            pos.x = maxX;
            moveDirection.x =
                -Mathf.Abs(moveDirection.x);
        }

        // Chạm dưới
        if (pos.y <= minY)
        {
            pos.y = minY;
            moveDirection.y =
                Mathf.Abs(moveDirection.y);
        }

        // Chạm trên
        else if (pos.y >= maxY)
        {
            pos.y = maxY;
            moveDirection.y =
                -Mathf.Abs(moveDirection.y);
        }

        transform.position = pos;
    }

    public void TakeDamage(int damage)
    {
        if (isDying)
            return;

        currentHealth -= damage;

        Debug.Log(
            "Enemy HP: " +
            currentHealth +
            "/" +
            maxHealth
        );

        StartCoroutine(HitEffect());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDying)
            return;

        isDying = true;

        // Cộng điểm
        ScoreManager scoreManager =
            FindFirstObjectByType<ScoreManager>();

        if (scoreManager != null)
        {
            scoreManager.AddScore(100);
        }

        // Hiệu ứng nổ
        if (explosionPrefab != null)
        {
            Instantiate(
                explosionPrefab,
                transform.position,
                Quaternion.identity
            );
        }

        Destroy(gameObject);
    }

    IEnumerator HitEffect()
    {
        if (spriteRenderer == null)
            yield break;

        spriteRenderer.enabled = false;

        yield return new WaitForSeconds(0.05f);

        if (!isDying)
            spriteRenderer.enabled = true;
    }

    private void OnCollisionEnter2D(
        Collision2D collision
    )
    {
        PlayerController player =
            collision.gameObject
            .GetComponentInParent<PlayerController>();

        if (player != null)
        {
            player.TakeDamage(1);
        }
    }

    private void OnTriggerEnter2D(
        Collider2D other
    )
    {
        PlayerController player =
            other.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            player.TakeDamage(1);
        }
    }
}