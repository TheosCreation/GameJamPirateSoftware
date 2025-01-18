using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;



#if UNITY_EDITOR
using UnityEditor;
#endif

public enum Direction : byte
{
    NONE = 0b0000,  // 0
    UP = 0b0001,    // 1
    RIGHT = 0b0010, // 2
    DOWN = 0b0100,  // 4
    LEFT = 0b1000   // 8
}


[Serializable]
public struct Node
{
    public Vector2[] directions;
    public GridData data;
}

public class TotalWaveCollapse : MonoBehaviour
{
    [Header("Arrays for each direction (existing fields)")]
    public GridData[] upGrids;
    public GridData[] rightGrids;
    public GridData[] downGrids;
    public GridData[] leftGrids;

    [Header("Wave Function Collapse Settings")]
    public int levelWidth = 5;     // Define grid size (width)
    public int levelHeight = 5;    // Define grid size (height)
    public int totalStates = 16;    // Number of possible states for each cell

    [Tooltip("Check to run WFC on Start() automatically.")]
    public bool runOnStart = false;


    [Header("Generation")]
    [Tooltip("Reference your GridGenerator here. The collapsed wave will be visually generated if assigned.")]
    public GridGenerator gridGenerator;

    [Tooltip("If true, will automatically generate a visual grid after WFC completes.")]
    public bool generateVisualGridOnComplete = true;
    // ---------------------------------------------------------------------------------------------
    public Node[] nodes;
    void Start()
    {
        if (runOnStart)
        {

            GenerateLevel();
        }
    }


    [ContextMenu("Generate Level (WFC)")]
    public void GenerateLevel()
    {
        Vector2 cursorPos = Vector2.zero;
        Direction lastCursorDirection = Direction.NONE; // next open pos
        switch (lastCursorDirection)
        {
            case Direction.NONE:

                break;
            case Direction.UP:
                break;
            case Direction.RIGHT:
                break;
            case Direction.DOWN:
                break;
            case Direction.LEFT:
                break;
        }

    }

}