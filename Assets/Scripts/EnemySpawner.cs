using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy")]
    public GameObject enemyPrefab;

    [Header("Số Enemy mỗi đợt")]
    public int enemiesPerWave = 3;

    [Header("Khu vực màn hình")]
    public float minX = -8f;
    public float maxX = 8f;
    public float minY = -4f;
    public float maxY = 4f;

    [Header("Khoảng cách spawn ngoài màn hình")]
    public float spawnDistance = 2f;

    [Header("Thời gian giữa các Wave")]
    public float nextWaveDelay = 1f;

    private bool spawningWave = false;

    void Start()
    {
        SpawnWave();
    }

    void Update()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(
            FindObjectsSortMode.None
        );

        if (enemies.Length == 0 && !spawningWave)
        {
            StartCoroutine(SpawnNextWave());
        }
    }

    void SpawnWave()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Chưa gán Enemy Prefab vào EnemySpawner!");
            return;
        }

        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemyOutside();
        }
    }

    void SpawnEnemyOutside()
    {
        Vector3 spawnPosition;
        Vector2 targetPosition;

        // Chọn ngẫu nhiên 1 trong 4 cạnh
        int side = Random.Range(0, 4);

        switch (side)
        {
            // Trái
            case 0:
                spawnPosition = new Vector3(
                    minX - spawnDistance,
                    Random.Range(minY, maxY),
                    0f
                );

                targetPosition = new Vector2(
                    Random.Range(minX + 1f, maxX - 1f),
                    Random.Range(minY + 1f, maxY - 1f)
                );
                break;

            // Phải
            case 1:
                spawnPosition = new Vector3(
                    maxX + spawnDistance,
                    Random.Range(minY, maxY),
                    0f
                );

                targetPosition = new Vector2(
                    Random.Range(minX + 1f, maxX - 1f),
                    Random.Range(minY + 1f, maxY - 1f)
                );
                break;

            // Dưới
            case 2:
                spawnPosition = new Vector3(
                    Random.Range(minX, maxX),
                    minY - spawnDistance,
                    0f
                );

                targetPosition = new Vector2(
                    Random.Range(minX + 1f, maxX - 1f),
                    Random.Range(minY + 1f, maxY - 1f)
                );
                break;

            // Trên
            default:
                spawnPosition = new Vector3(
                    Random.Range(minX, maxX),
                    maxY + spawnDistance,
                    0f
                );

                targetPosition = new Vector2(
                    Random.Range(minX + 1f, maxX - 1f),
                    Random.Range(minY + 1f, maxY - 1f)
                );
                break;
        }

        GameObject enemy = Instantiate(
            enemyPrefab,
            spawnPosition,
            Quaternion.identity
        );

        // Cho Enemy biết điểm cần bay vào
        Enemy enemyScript = enemy.GetComponent<Enemy>();

        if (enemyScript != null)
        {
            enemyScript.SetTarget(targetPosition);
        }
    }

    IEnumerator SpawnNextWave()
    {
        spawningWave = true;

        yield return new WaitForSeconds(nextWaveDelay);

        SpawnWave();

        spawningWave = false;
    }
}