using System.Collections.Generic;
using UnityEngine;

public class ShipGrid : MonoBehaviour
{
    public List<List<Cell>> grid = new();

    [SerializeField] float cellSpacement;
    [SerializeField] Transform cellsStart;

    [SerializeField] GameObject emptyCellPrefab;

    public int gridWidth;
    public int gridHeight;

    [System.Serializable]
    public struct StartPart
    {
        public GameObject part;
        public int posX, posY;
    }

    public List<StartPart> startParts;

    public static ShipGrid instance;

    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        InitGrid();
        PlaceStartParts();
    }

    public void InitGrid()
    {
        for (int i = 0; i < gridWidth; i++)
        {
            grid.Add(new());
            for (int j = 0; j < gridHeight; j++)
            {
                GameObject newCell = Instantiate(emptyCellPrefab, cellsStart);
                newCell.name = "Cell " + i + " " + j;
                grid[i].Add(newCell.GetComponent<Cell>());
            }
        }

        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                grid[i][j].Init(this, i, j, cellSpacement);
                grid[i][j].transform.localPosition = new(i * cellSpacement, (j - (gridHeight-1) / 2f) * cellSpacement);
            }
        }
    }

    private void PlaceStartParts()
    {
        foreach (var item in startParts)
        {
            GameObject go = Instantiate(item.part);
            grid[Mathf.Clamp(item.posX, 0, gridWidth - 1)][Mathf.Clamp(item.posY, -(gridHeight / 2), gridHeight / 2) + ((gridHeight - 1) / 2)].Place(go);
        }
    }

    public Cell GetCellOffset(Cell testCell, int offsetX, int offsetY)
    {
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                if (grid[i][j] == testCell)
                    return GetCellAt(i + offsetX, j + offsetY);
            }
        }

        return null;
    }

    public Cell GetCellAt(int i, int j)
    {
        if (i >= 0 && i < gridWidth)
            if (j >= 0 && j < gridHeight)
                return grid[i][j];
        return null;
    }

    public Vector3 GetGridCenter()
    {
        return new((float)((gridWidth - 1) * cellSpacement) / 2f, (float)((gridHeight - 1) * cellSpacement) / 2f);
    }
}

[SerializeField] public enum ECell
{
    Empty,
    Core
}
