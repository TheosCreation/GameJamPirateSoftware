using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField] private Enemy[] enemiesPrefab;

    public Enemy CreateEnemy(Vector3 position)
    {
        if (enemiesPrefab == null || enemiesPrefab.Length == 0)
        {
            Debug.LogError("no enemy prefabs");
            return null;
        }

        int randomIndex = Random.Range(0, enemiesPrefab.Length);
        return Instantiate(enemiesPrefab[randomIndex], position, Quaternion.identity); ;
    }
}