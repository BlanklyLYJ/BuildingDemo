using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridManager<TGridObject>
{
    private int width;
    private int height;
    public float cellSize;
    private TGridObject[,] gridArray;
    private TextMesh[,] debugTextArray;

    public GridManager(int width, int height, float cellSize, Vector3 originPosition,
        Func<GridManager<TGridObject>, int, int, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        gridArray = new TGridObject[width, height];
        debugTextArray = new TextMesh[width, height];
        int count = 0;
        bool showText = true;
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int z = 0; z < gridArray.GetLength(1); z++)
            {
                gridArray[x, z] = createGridObject(this, x, z);
                if (showText) //gridArray[x, z].ToString()
                    debugTextArray[x, z] = CreateWorldText(count.ToString(), Color.white, null,
                        GetWorldPosition(x, z) + new Vector3(cellSize, 1, cellSize) * 0.5f,
                        20, TextAnchor.MiddleCenter);
                count++;
                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.white, 100f);
            }
        }

        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    public Vector3 GetWorldPosition(int x, int z)
    {
        // + originPosition todo 起始位置
        return new Vector3(x, 0, z) * cellSize;
    }

    public void GetXZ(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt(worldPosition.x / cellSize);
        z = Mathf.FloorToInt(worldPosition.z / cellSize);
    }

    public Vector3 GetXZSetPosition(Vector3 worldPosition)
    {
        return new Vector3(Mathf.FloorToInt(worldPosition.x / cellSize) * cellSize, 0
            , Mathf.FloorToInt(worldPosition.z / cellSize) * cellSize);
    }

    public void SetValue(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            if (debugTextArray[x, y])
            {
                // debugTextArray[x, y].text = gridArray[x, y].ToString();
                debugTextArray[x, y].gameObject.SetActive(!debugTextArray[x, y].gameObject.activeSelf);
            }
        }
    }
    public TGridObject GetTGridObject(int x, int z)
    {
        return gridArray[x, z];
    }
    
    public TGridObject GetTGridObject(Vector3 worldPosition) {
        GetXZ(worldPosition, out int x, out int z);
        return GetTGridObject(x, z);
    }


    public static TextMesh CreateWorldText(string text, Color color, Transform parent = null,
        Vector3 localPosition = default(Vector3), int fontSize = 40, TextAnchor textAnchor = TextAnchor.MiddleCenter
        , TextAlignment textAlignment = TextAlignment.Center, int sortingOrder = 0)
    {
        return CreateWorldText(parent, text, localPosition, fontSize, color, textAnchor, textAlignment, sortingOrder);
    }

    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize,
        Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        textMesh.gameObject.SetActive(false);
        return textMesh;
    }
}