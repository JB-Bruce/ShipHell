using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    float cellSize;
    int posX, posY;

    [SerializeField] SpriteRenderer sp;
    public GameObject shipPartAttached;

    ShipGrid gridManager;

    public Cell upCell { get; private set; }
    public Cell downCell { get; private set; }
    public Cell leftCell { get; private set; }
    public Cell rightCell { get; private set; }

    public void Init(ShipGrid gm, int x, int y, float size)
    {
        gridManager = gm;
        cellSize = size;
        posX = x;
        posY = y;

        upCell = gridManager.GetCellOffset(this, 0, 1);
        downCell = gridManager.GetCellOffset(this, 0, -1);
        leftCell = gridManager.GetCellOffset(this, -1, 0);
        rightCell = gridManager.GetCellOffset(this, 1, 0);
    }

    public bool TryPlace(GameObject shipPart)
    {
        if (!CanPlace())
            return false;

        Place(shipPart);
        return true;
    }

    public bool CanPlace()
    {
        if (shipPartAttached != null || ((upCell == null || upCell.shipPartAttached == null) && (downCell == null || downCell.shipPartAttached == null) && (leftCell == null || leftCell.shipPartAttached == null) && (rightCell == null || rightCell.shipPartAttached == null)))
            return false;
        return true;
    }

    public bool Place(GameObject shipPart)
    {
        shipPartAttached = shipPart;
        shipPart.transform.parent = transform;
        shipPart.transform.localPosition = Vector2.zero;
        shipPart.transform.localScale = Vector3.one;
        return true;
    }

    public bool AreAllLinkedWithoutIt()
    {
        HashSet<Cell> upCellCount = new();
        HashSet<Cell> downCellCount = new();
        HashSet<Cell> rightCellCount = new();
        HashSet<Cell> leftCellCount = new();

        if (upCell != null && upCell.shipPartAttached != null)
        {
            List<Cell> cellList = new() { this };
            upCellCount = new(upCell.SearchMoreCells(ref cellList));
        }

        if (downCell != null && downCell.shipPartAttached != null)
        {
            List<Cell> cellList = new() { this };
            downCellCount = new(downCell.SearchMoreCells(ref cellList));
        }

        if (rightCell != null && rightCell.shipPartAttached != null)
        {
            List<Cell> cellList = new() { this };
            rightCellCount = new(rightCell.SearchMoreCells(ref cellList));
        }

        if (leftCell != null && leftCell.shipPartAttached != null)
        {
            List<Cell> cellList = new() { this };
            leftCellCount = new(leftCell.SearchMoreCells(ref cellList));
        }

        List<HashSet<Cell>> cellSet = new();

        if(upCellCount.Count > 0)
            cellSet.Add(upCellCount);
        if (downCellCount.Count > 0)
            cellSet.Add(downCellCount);
        if(rightCellCount.Count > 0)
            cellSet.Add(rightCellCount);
        if(leftCellCount.Count > 0)
            cellSet.Add(leftCellCount);

        foreach(var cell1 in cellSet)
        {
            foreach (var cell2 in cellSet)
            {
                if (!cell1.SetEquals(cell2))
                    return false;
            }
        }

        return true;
    }

    public List<Cell> SearchMoreCells(ref List<Cell> cellList)
    {
        if(cellList.Contains(this))
            return cellList;

        cellList.Add(this);

        if (upCell != null && upCell.shipPartAttached != null)
        {
            upCell.SearchMoreCells(ref cellList);
        }

        if (downCell != null && downCell.shipPartAttached != null)
        {
            downCell.SearchMoreCells(ref cellList);
        }

        if (rightCell != null && rightCell.shipPartAttached != null)
        {
            rightCell.SearchMoreCells(ref cellList);
        }

        if (leftCell != null && leftCell.shipPartAttached != null)
        {
            leftCell.SearchMoreCells(ref cellList);
        }

        return cellList;
    }
}
