using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

using Random = UnityEngine.Random;



[Serializable]

public struct Node
{
    public GridData data;
}
public class TotalWaveCollapse : MonoBehaviour
{
    [Header("Generation Settings")]
    public int steps = 20;

    [Header("Run on start")]
    public bool runOnStart = false;

    [Header("Generation")]
    public GridGenerator gridGenerator;
    public bool generateVisualGridOnComplete = true;
    public Node[] nodes;

    [Header("Nav Mesh")]
    public NavMeshPlus.Components.NavMeshSurface meshSurface; 


    private HashSet<Vector2> usedPositions = new HashSet<Vector2>();

    public void GenerateLevelWrapped()
    {
        GenerateLevel(Vector2.zero, new Direction[0], 0, nodes[0].data.directions[0]);
    }

    [ContextMenu("Generate Level (WFC)")]
    public void GenerateLevel(Vector2 cursorPos, Direction[] openDirections, int currentStep, Direction newDirection)
    {
        if (currentStep >= steps)
        {
            Debug.Log("max steps reached");
            return;
        }

        Node? node = GetRandomNodeWithDirections(newDirection);
        if (node == null)
        {
            Debug.Log("null node found");
            return;
        }

        GridData data = node.Value.data;
        if (data == null)
        {
            Debug.Log("null data found");
            return;
        }

        openDirections = data.directions;
        if (cursorPos != Vector2.zero)
        {
            gridGenerator.GenerateGrid(data, cursorPos);
        }

        usedPositions.Add(cursorPos);

        foreach (Direction direction in openDirections)
        {
            Vector2 newCursorPos = cursorPos + direction.ToVector2();
            if (!usedPositions.Contains(newCursorPos))
            {
                GenerateLevel(newCursorPos, openDirections, currentStep + 1, direction);
            }
            else
            {
                Debug.Log("no position found");
            }
        }
    }

    public Node? GetRandomNodeWithDirections(params Direction[] requiredDirections)
    {
        List<Node> matchingNodes = new List<Node>();
        foreach (var node in nodes)
        {
            if (node.data != null && ContainsAllDirections(node.data.directions, requiredDirections))
            {
                matchingNodes.Add(node);
            }
        }

        if (matchingNodes.Count == 0)
        {
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, matchingNodes.Count);
        return matchingNodes[randomIndex];
    }

    private bool ContainsAllDirections(Direction[] nodeDirections, Direction[] requiredDirections)
    {
        foreach (var direction in requiredDirections)
        {
            if (Array.IndexOf(nodeDirections, direction) == -1)
            {
                return false;
            }
        }
        return true;
    }
}

