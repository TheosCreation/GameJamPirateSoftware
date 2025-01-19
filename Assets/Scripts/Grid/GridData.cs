using System;
using UnityEditor;
using UnityEngine;

public enum Direction
{
    None,
    Up,
    Down,
    Left,
    Right
}

public static class DirectionExtensions
{
    public static Vector2 ToVector2(this Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return new Vector2(0, 1);
            case Direction.Down:
                return new Vector2(0, -1);
            case Direction.Left:
                return new Vector2(-1, 0);
            case Direction.Right:
                return new Vector2(1, 0);
            default:
                return Vector2.zero;
        }
    }
}


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
    public Direction[] directions;
    public BoolArrayWrapper[] grid;

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
        else
        {
            for (int i = 0; i < height; i++)
            {
                if (grid[i] == null || grid[i].row.Length != width)
                {
                    grid[i] = new BoolArrayWrapper(width);
                }
            }
        }
    }
}