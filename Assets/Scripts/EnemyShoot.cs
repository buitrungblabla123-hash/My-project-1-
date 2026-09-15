using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [Header("Bullet")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootInterval = 2f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= shootInterval)
        {
            Shoot();
            timer = 0f;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null)
            return;

        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.identity
        );

        EnemyBullet bulletScript =
            bullet.GetComponent<EnemyBullet>();

        if (bulletScript != null)
        {
            bulletScript.SetDirection(Vector2.down);
        }
    }
}

