using NavMeshPlus.Components;
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
    [SerializeField] private float enemySpeedMultiplierIncreaseIncrement = 0.05f;
    [SerializeField] private float spawnIntervalMultiplier = 0.9f;
    [SerializeField] private float spawnIntervalCap = 0.1f;

    [SerializeField] private TotalWaveCollapse levelGenerator;
    [SerializeField] private NavMeshSurface meshSurface;

    private float currentEnemySpeedIncreaseMultiplier = 1f;

    public int currentWave = 1;
    public float currentSpawnInterval = 1f;
    public PlayerController Player { get; private set; }

    private void Start()
    {
        Player = Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);
        enemySpawner.OnWaveComplete += ChooseUpgrade;


        levelGenerator.GenerateLevelWrapped();
 

        StartFirstWave();
    }

    private void StartFirstWave()
    {
        int totalEnemies = initialEnemiesPerWave + (currentWave - 1) * enemiesPerWaveIncrement;
        int maxEnemy = initialMaxEnemyPerWave + (currentWave - 1) * maxEnemyPerWaveIncrement;
        currentSpawnInterval = initialSpawnIntervalPerWave;
        enemySpawner.StartWave(totalEnemies, maxEnemy, currentSpawnInterval, currentEnemySpeedIncreaseMultiplier, Player.transform);

        UiManager.Instance.playerHud.UpdateRoundCounter(currentWave);
    }

    private void StartNextWave()
    {
        currentWave++;
        int totalEnemies = initialEnemiesPerWave + (currentWave - 1) * enemiesPerWaveIncrement;
        int maxEnemy = initialMaxEnemyPerWave + (currentWave - 1) * maxEnemyPerWaveIncrement;
        currentSpawnInterval = Mathf.Max(spawnIntervalCap, currentSpawnInterval * spawnIntervalMultiplier);
        currentEnemySpeedIncreaseMultiplier += enemySpeedMultiplierIncreaseIncrement;
        enemySpawner.StartWave(totalEnemies, maxEnemy, currentSpawnInterval, currentEnemySpeedIncreaseMultiplier, Player.transform);

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