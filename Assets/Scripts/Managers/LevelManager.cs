using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private PlayerController playerPrefab;
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private int initialEnemiesPerWave = 5;
    [SerializeField] private int initialMaxEnemyPerWave = 3;
    [SerializeField] private float initialSpawnIntervalPerWave = 2.0f;
    [SerializeField] private int enemiesPerWaveIncrement = 5;
    [SerializeField] private int maxEnemyPerWaveIncrement = 1;
    [SerializeField] private float spawnIntervalMultiplier = 0.9f;

    public int currentWave = 0;
    public float currentSpawnInterval = 1f;
    public PlayerController Player { get; private set; }

    private void Start()
    {
        Player = Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);
        enemySpawner.OnWaveComplete += ChooseUpgrade;

        currentSpawnInterval = initialSpawnIntervalPerWave;
        StartNextWave();
    }

    private void StartNextWave()
    {
        currentWave++;
        int totalEnemies = initialEnemiesPerWave + (currentWave - 1) * enemiesPerWaveIncrement;
        int maxEnemy = initialMaxEnemyPerWave + (currentWave - 1) * maxEnemyPerWaveIncrement;
        currentSpawnInterval *= spawnIntervalMultiplier;
        enemySpawner.StartWave(totalEnemies, maxEnemy, currentSpawnInterval, Player.transform);

        UiManager.Instance.playerHud.UpdateRoundCounter(currentWave);
    }

    private void ChooseUpgrade()
    {
        UiManager.Instance.OpenUpgradeScreen();
        PauseManager.Instance.PauseNoScreen();
        PauseManager.Instance.canUnpause = false;
        StartNextWave();
    }
}