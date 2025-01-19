using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private PlayerController playerPrefab;
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private int initialEnemiesPerWave = 5;
    [SerializeField] private int waveIncrement = 5;

    public int currentWave = 0;

    public PlayerController Player { get; private set; }

    private void Start()
    {
        Player = Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);
        enemySpawner.OnWaveComplete += StartNextWave;
        StartNextWave();
    }

    private void StartNextWave()
    {
        currentWave++;
        int totalEnemies = initialEnemiesPerWave + (currentWave - 1) * waveIncrement;
        enemySpawner.StartWave(totalEnemies, Player.transform);

        UiManager.Instance.playerHud.UpdateRoundCounter(currentWave);
    }
}