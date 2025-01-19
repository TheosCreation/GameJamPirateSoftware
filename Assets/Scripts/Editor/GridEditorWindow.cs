// GridEditorWindow.cs
using UnityEngine;
using UnityEditor;

public class GridEditorWindow : EditorWindow
{
    private GridData currentGrid;
    private Vector2 scrollPosition;
    private float cellSize = 30f;

    [MenuItem("Window/Custom/Grid Editor")]
    public static void ShowWindow()
    {
        GetWindow<GridEditorWindow>("Grid Editor");
    }

    void OnGUI()
    {
        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        currentGrid = (GridData)EditorGUILayout.ObjectField("Grid Data", currentGrid, typeof(GridData), false);
        cellSize = EditorGUILayout.Slider("Cell Size", cellSize, 20f, 50f);
        EditorGUILayout.EndHorizontal();

        if (currentGrid == null)
        {
            EditorGUILayout.HelpBox("Please assign a GridData asset", MessageType.Info);
            return;
        }

        EditorGUILayout.BeginHorizontal();
        currentGrid.width = EditorGUILayout.IntField("Width", currentGrid.width);
        currentGrid.height = EditorGUILayout.IntField("Height", currentGrid.height);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Reset Grid"))
        {
            currentGrid.OnValidate();
        }

        DrawGrid();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(currentGrid);
        }
    }

    private void DrawGrid()
    {
        if (currentGrid.grid == null) return;

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        EditorGUILayout.BeginVertical();
        for (int y = currentGrid.height - 1; y >= 0; y--)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < currentGrid.width; x++)
            {
                if (currentGrid.grid[y] == null) continue;

                currentGrid.grid[y].row[x] = EditorGUILayout.Toggle(
                    currentGrid.grid[y].row[x],
                    GUILayout.Width(cellSize),
                    GUILayout.Height(cellSize)
                );
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndScrollView();
    }
}
