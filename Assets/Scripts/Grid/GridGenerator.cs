using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject squarePrefab;
    public float spacing = 1f;
    public float distance = 5f;

    void Start()
    {
    }

    public void GenerateGrid(GridData _data, Vector2 startPosition)
    {
        if (_data == null || squarePrefab == null) return;
       // GameObject daddy = Instantiate(gameObject, transform);
        for (int y = 0; y < _data.height; y++)
        {
            for (int x = 0; x < _data.width; x++)
            {
      
                if (_data.grid[y].row[x])
                {
                    Vector3 position = new Vector3((startPosition.x* distance) + x * spacing, (startPosition.y* distance) + y * spacing, 0);
                    Instantiate(squarePrefab, position, Quaternion.identity, transform).name = _data.name;
                }
            }
        }
    }
}
