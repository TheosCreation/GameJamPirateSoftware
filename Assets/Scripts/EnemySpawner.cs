using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyFactory enemyFactory;
    private int m_maxEnemiesAlive = 10;
    private float m_spawnInterval = 2f;
    [SerializeField] private float innerRadius = 5f; // Minimum distance from the player
    [SerializeField] private float outerRadius = 10f; // Maximum distance from the player
    [SerializeField] private float enemyDespawnRange = 50f;

    [Space(20)]
    [SerializeField] private bool debug = false; // Maximum distance from the player

    private int enemiesAlive = 0;
    private int enemiesSpawned = 0;
    private int enemiesToSpawn = 0;
    // Define the inner and outer radius of the donut area

    private Transform playerTransform;

    public System.Action OnWaveComplete;

    public void StartWave(int totalEnemies, int maxEnemiesAlive, float spawnInterval, Transform player)
    {
        enemiesAlive = 0;
        enemiesSpawned = 0;
        enemiesToSpawn = totalEnemies;
        m_spawnInterval = spawnInterval;
        m_maxEnemiesAlive = maxEnemiesAlive;
        playerTransform = player;
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (enemiesSpawned < enemiesToSpawn || enemiesAlive > 0)
        {
            if (enemiesAlive < m_maxEnemiesAlive && enemiesSpawned < enemiesToSpawn)
            {
                SpawnEnemy(true);
            }

            yield return new WaitForSeconds(m_spawnInterval);
        }

        OnWaveComplete?.Invoke();
    }

    private void SpawnEnemy(bool trueSpawn)
    {
        // Generate a random angle and distance
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float distance = Random.Range(innerRadius, outerRadius);

        // Calculate the position relative to the player
        Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * distance;
        Vector3 spawnPosition = playerTransform.position + offset;

        // Spawn the enemy at the calculated position
        Enemy enemy = enemyFactory.CreateEnemy(spawnPosition);
        enemy.target = playerTransform; // Assign the player as the target
        enemy.despawnDistance = enemyDespawnRange; // Assign the player as the target
        enemy.OnDeath += HandleEnemyDeath;
        enemy.OnDespawn += HandleEnemyDespawn;

        if(trueSpawn)
        {
            enemiesSpawned++;
            enemiesAlive++;
        }
    }

    private void HandleEnemyDeath()
    {
        enemiesAlive--;
    }

    private void HandleEnemyDespawn()
    {
        SpawnEnemy(false);
    }

    private void OnDrawGizmos()
    {
        if (playerTransform == null || debug == false) return;

        // Set the Gizmos color for the outer radius
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f); // Semi-transparent red
        Gizmos.DrawWireSphere(playerTransform.position, outerRadius);

        // Set the Gizmos color for the inner radius
        Gizmos.color = new Color(0f, 1f, 0f, 0.5f); // Semi-transparent green
        Gizmos.DrawWireSphere(playerTransform.position, innerRadius);

        // Set the Gizmos color for the despawn range
        Gizmos.color = new Color(0f, 0f, 1f, 0.5f); // Semi-transparent blue
        Gizmos.DrawWireSphere(playerTransform.position, enemyDespawnRange);
    }

}
