using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;

    public Enemy CreateEnemy(Vector3 position)
    {
        return Instantiate(enemyPrefab, position, Quaternion.identity);
    }
}