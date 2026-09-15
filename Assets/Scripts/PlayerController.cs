using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Health")]
    public int maxHealth = 5;
    public int currentHealth = 5;

    [Header("Giới hạn màn hình")]
    public float minX = -8f;
    public float maxX = 8f;
    public float minY = -4f;
    public float maxY = 4f;

    [Header("Game Over")]
    public GameObject gameOverText;

    private bool isInvulnerable = false;
    private bool isDead = false;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHealth = maxHealth;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogWarning("Player chưa có SpriteRenderer!");
        }
        
        if (gameOverText != null)
        {
            gameOverText.SetActive(false);
        }
        
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (isDead)
            return;

        Vector2 movement = Vector2.zero;
        
        if (Keyboard.current.aKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed)
        {
            movement.x = -1;
        }
        
        if (Keyboard.current.dKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed)
        {
            movement.x = 1;
        }
        
        if (Keyboard.current.wKey.isPressed ||
            Keyboard.current.upArrowKey.isPressed)
        {
            movement.y = 1;
        }
        
        if (Keyboard.current.sKey.isPressed ||
            Keyboard.current.downArrowKey.isPressed)
        {
            movement.y = -1;
        }

        movement = movement.normalized;

        transform.Translate(
            movement * moveSpeed * Time.deltaTime,
            Space.World
        );
        
        float x = Mathf.Clamp(
            transform.position.x,
            minX,
            maxX
        );

        float y = Mathf.Clamp(
            transform.position.y,
            minY,
            maxY
        );

        transform.position = new Vector3(
            x,
            y,
            transform.position.z
        );
    }
    
    public void TakeDamage(int damage)
    {
        if (isDead)
            return;
        
        if (isInvulnerable)
            return;

        currentHealth -= damage;
        
        if (currentHealth < 0)
            currentHealth = 0;

        Debug.Log(
            "Player HP: " +
            currentHealth +
            "/" +
            maxHealth
        );
        
        if (currentHealth <= 0)
        {
            GameOver();
            return;
        }

        StartCoroutine(DamageEffect());
    }

   
    IEnumerator DamageEffect()
    {
        isInvulnerable = true;

        float timer = 0f;

        while (timer < 1f)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled =
                    !spriteRenderer.enabled;
            }

            yield return new WaitForSeconds(0.1f);

            timer += 0.1f;
        }
        
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }

        isInvulnerable = false;
    }
    
    
    void GameOver()
    {
        if (isDead)
            return;

        isDead = true;

        Debug.Log("PLAYER GAME OVER");
        
        if (gameOverText != null)
        {
            gameOverText.SetActive(true);
        }
        else
        {
            Debug.LogWarning(
                "Chưa kéo GameOverText vào PlayerController!"
            );
        }
        
        gameObject.SetActive(false);
        
        Time.timeScale = 0f;
    }
}