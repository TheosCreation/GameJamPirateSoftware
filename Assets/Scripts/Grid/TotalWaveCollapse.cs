using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public struct Node
{
    public GridData data;
}

public class TotalWaveCollapse : MonoBehaviour
{
    [Header("Generation Settings")]
    public int steps = 20;

    [Header("Start Parameters")]
    public bool debugMode = false;

    [Header("Generation")]
    public GridGenerator gridGenerator;
    public bool generateVisualGridOnComplete = true;
    public Node[] nodes;

    private HashSet<Vector2> usedPositions = new HashSet<Vector2>();

    public bool isGenerating = false;

    public void GenerateLevelWrapped()
    {
        GenerateLevelAsync(Vector2.zero, new Direction[0], 0, nodes[0].data.directions[0]);
    }

    [ContextMenu("Generate Level (WFC)")]
    public async void GenerateLevelAsync(Vector2 cursorPos, Direction[] openDirections, int currentStep, Direction newDirection)
    {
        if (isGenerating)
        {
            Debug.LogWarning("Level generation is already in progress.");
            return;
        }

        isGenerating = true;

        try
        {
            await GenerateLevelCoroutine(cursorPos, openDirections, currentStep, newDirection);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error during level generation: {ex.Message}");
        }
        finally
        {
            isGenerating = false;
        }
    }

    private async Task GenerateLevelCoroutine(Vector2 cursorPos, Direction[] openDirections, int currentStep, Direction newDirection)
    {
        if (currentStep >= steps)
        {
            if (debugMode) Debug.Log("Generation stopped or max steps reached.");
            return;
        }

        Node? node = GetRandomNodeWithDirections(newDirection);
        if (node == null)
        {
            if (debugMode) Debug.Log("No valid node found.");
            return;
        }

        GridData data = node.Value.data;
        if (data == null)
        {
            if(debugMode) Debug.Log("Node data is null.");
            return;
        }

        openDirections = data.directions;
        if (cursorPos != Vector2.zero)
        {
            gridGenerator.GenerateGrid(data, cursorPos);
        }

        usedPositions.Add(cursorPos);

        // Wait for a frame to allow halting or visualization
        await Task.Yield();

        foreach (Direction direction in openDirections)
        {
            Vector2 newCursorPos = cursorPos + direction.ToVector2();
            if (!usedPositions.Contains(newCursorPos))
            {
                await GenerateLevelCoroutine(newCursorPos, openDirections, currentStep + 1, direction);
            }
            else
            {
                if (debugMode) Debug.Log("Position already used.");
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