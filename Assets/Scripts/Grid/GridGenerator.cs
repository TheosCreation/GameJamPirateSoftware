using UnityEngine;

public class GridGenerator : MonoBehaviour
{

    public GameObject squarePrefab;
    public float spacing = 1f;

    void Start()
    {

    }

    public void GenerateGrid(GridData _data)
    {
    
        if (_data == null || squarePrefab == null) return;
  

        for (int y = 0; y < _data.height; y++)
        {
            for (int x = 0; x < _data.width; x++)
            {
                if (_data.grid[y].row[x])
                {
                    Vector3 position = new Vector3(x * spacing, y * spacing, 0);
                    Instantiate(squarePrefab, position, Quaternion.identity, transform);
                }
            }
        }
    }
}
