using UnityEngine;

[System.Serializable]
public class BoolArrayWrapper
{
    public bool[] row;
    public BoolArrayWrapper(int size)
    {
        row = new bool[size];
    }
}

[CreateAssetMenu(fileName = "GridData", menuName = "Grid/GridData")]
public class GridData : ScriptableObject
{
    public int width = 5;
    public int height = 5;
    public BoolArrayWrapper[] grid;
    public Direction[] directions;

    public void OnValidate()
    {
        if (grid == null || grid.Length != height)
        {
            grid = new BoolArrayWrapper[height];
            for (int i = 0; i < height; i++)
            {
                grid[i] = new BoolArrayWrapper(width);
            }
        }
    }
}