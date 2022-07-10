using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static VirtualGridManagerV1;

public class IndependentCellV1
{
    public Vector2 cellCoords;
    public GameObject debugCellObject;

    public ElementKindV1 elementKind;
    public IndependentCellV1(Vector2 coords, GameObject debug, ElementKindV1 kind = ElementKindV1.Empty)
    {
        cellCoords = coords;
        debugCellObject = debug;
        elementKind = kind;
    }
}
public class GridCellV1
{
    public VirtualGridManagerV1 virtualGridManager;

    public IndependentCellV1 cell;

    public bool isSet;

    public int setId;
    public int setMemberAmount;

    public GridCellV1(VirtualGridManagerV1 grid, Vector2 coords, GameObject debug, ElementKindV1 kind = ElementKindV1.Empty)
    {
        virtualGridManager = grid;
        cell = new IndependentCellV1(coords, debug, kind);
    }

    public void ResetGridCell()
    {
        if (virtualGridManager.isGraphic && cell.debugCellObject != null)
            GameObject.Destroy(cell.debugCellObject);

        cell = null;

        isSet = false;
        setId = 0;
        setMemberAmount = 0;
    }
    public void SetSetId(bool inheritedId, int id = 0)
    {
        isSet = true;
        setId = inheritedId ? id : virtualGridManager.AddNewSet();

        if (virtualGridManager.setList.TryGetValue(setId, out var list))
        {
            list.Add(cell.cellCoords);
        }
        else
        {
            var newList = new List<Vector2>();
            newList.Add(cell.cellCoords);
            virtualGridManager.setList.Add(setId, newList);
        }

        if (virtualGridManager.isGraphic)
            cell.debugCellObject.transform.GetChild(1).GetComponent<TMP_Text>().text = setId.ToString();

        CheckCrossCoords();
    }

    public void CheckCrossCoords()
    {
        foreach (Vector2 vector in cell.cellCoords.GetCrossCoords())
        {
            if (virtualGridManager.virtualGrid.TryGetValue(vector, out GridCellV1 otherCell) && otherCell.cell.elementKind == cell.elementKind && !otherCell.isSet)
            {
                otherCell.SetSetId(true, setId);
            }
        }
    }
}
public class VirtualGridManagerV1 : MonoBehaviour
{
    public StarshipManager turnManager;

    public enum ElementKindV1 { Empty, A, B, C, D };

    public int gridHeight;
    public int gridWidth;

    public Transform visualParent;
    public GameObject debugVisualCell;

    [HideInInspector] public Vector2 gridOffset;

    int setCount;

    public bool isGraphic;

    public Dictionary<Vector2, GridCellV1> virtualGrid = new();
    public Dictionary<int, List<Vector2>> setList = new();
    public Dictionary<int, List<Vector2>> deletedElementCoordsByX = new();

    void Start()
    {
        //InitGeneration();
    }

    public void InitGeneration()
    {
        ResetGrid();

        SetGrid();
        FillGrid();

        SetAssignation();
    }

    void SetAssignation()
    {
        AssignSetGrid();
        SendSetIntel();
    }
    public void ResetGrid()
    {
        setCount = 0;
        gridOffset = Vector2.zero;
        visualParent.position = Vector2.zero;

        foreach (var item in virtualGrid.Values)
        {
            if (item.cell != null)
                Destroy(item.cell.debugCellObject);
        }

        virtualGrid.Clear();
        setList.Clear();
        deletedElementCoordsByX.Clear();
    }
    void SetGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector2 coords = new Vector2(x, y);

                GameObject debObject = null;

                if (isGraphic)
                    debObject = Instantiate(debugVisualCell, coords, Quaternion.identity, visualParent);

                virtualGrid.Add(coords, new GridCellV1(this, coords, debObject));
            }
        }
        //gridOffset = new Vector2(-(gridWidth / 2), -gridHeight);
        //visualParent.position = gridOffset;
    }
    void FillGrid()
    {
        foreach (var item in virtualGrid)
        {
            item.Value.cell.elementKind = GetElementKind(item.Value);
        }
    }
    void AssignSetGrid()
    {
        foreach (var item in virtualGrid.Values)
        {
            if (!item.isSet)
                item.SetSetId(false);
        }
    }
    void SendSetIntel()
    {
        foreach (var item in setList)
        {
            foreach (var a in item.Value)
            {
                virtualGrid[a].setMemberAmount = item.Value.Count;

                if (isGraphic)
                    virtualGrid[a].cell.debugCellObject.transform.GetChild(2).GetComponent<TMP_Text>().text = virtualGrid[a].setMemberAmount.ToString();
            }
        }
    }
    public int AddNewSet()
    {
        setCount++;
        return setCount;
    }
    ElementKindV1 GetElementKind(GridCellV1 debObj)
    {
        ElementKindV1 kind = (ElementKindV1)Random.Range(1, System.Enum.GetValues(typeof(ElementKindV1)).Length);

        if (isGraphic)
        {
            debObj.cell.debugCellObject.transform.GetChild(0).GetComponent<TMP_Text>().text = kind.ToString();
            debObj.cell.debugCellObject.transform.GetChild(0).GetComponent<TMP_Text>().color = GetDebugColor(kind);
        }

        return kind;
    }
    Color GetDebugColor(ElementKindV1 kind)
    {
        Color selectedColor;
        switch (kind)
        {
            case ElementKindV1.A:
                selectedColor = Color.blue;
                break;
            case ElementKindV1.B:
                selectedColor = Color.yellow;
                break;
            case ElementKindV1.C:
                selectedColor = Color.green;
                break;
            case ElementKindV1.D:
                selectedColor = Color.red;
                break;
            default:
                selectedColor = Color.white;
                break;
        }
        return selectedColor;
    }
    public void CheckElementOnGrid(Vector2 coords, bool debugIput)
    {
        if (virtualGrid.TryGetValue(coords, out GridCellV1 gridCell) && gridCell.cell != null)
        {
            Debug.Log(virtualGrid[coords].cell.elementKind + ": " + virtualGrid[coords].setMemberAmount + ": " + virtualGrid[coords].cell.cellCoords);

            if (virtualGrid[coords].setMemberAmount >= 2 && !debugIput)
            {
                SetInteraction(virtualGrid[coords]);
            }
        }
    }

    void SetInteraction(GridCellV1 debObj)
    {
        deletedElementCoordsByX.Clear();

        int setId = debObj.setId;
        //turnManager.AddScoreOfKind(debObj.cell.elementKind, debObj.setMemberAmount);

        foreach (Vector2 coords in setList[setId])
        {
            virtualGrid[coords].ResetGridCell();
            if (deletedElementCoordsByX.TryGetValue((int)coords.x, out var list))
            {
                list.Add(coords);
            }
            else
            {
                var coordsList = new List<Vector2>();
                coordsList.Add(coords);
                deletedElementCoordsByX.Add((int)coords.x, coordsList);
            }
        }
        setList.Remove(setId);

        MoveUpperTiles();
    }

    void MoveUpperTiles()
    {
        foreach (var pairXcoords in deletedElementCoordsByX)
        {
            foreach (Vector2 delCoords in pairXcoords.Value)
            {
                for (int i = (int)delCoords.y + 1; i <= 6; i++)
                {
                    if (virtualGrid.TryGetValue(new Vector2(delCoords.x, i), out GridCellV1 cellSlot) && cellSlot.isSet)
                    {
                        TranspassCellData(cellSlot, pairXcoords.Value.Count);
                    }
                }
            }
        }
    }

    void TranspassCellData(GridCellV1 cellSlot, int verticalFloors)
    {
        Vector2 newPosition = cellSlot.cell.cellCoords + Vector2.down * verticalFloors;

        IndependentCellV1 movedCell = new IndependentCellV1(newPosition, cellSlot.cell.debugCellObject, cellSlot.cell.elementKind);

        virtualGrid[newPosition].cell = movedCell;
        virtualGrid[newPosition].isSet = false;

        if (isGraphic)
            cellSlot.cell.debugCellObject.transform.DOMoveY(cellSlot.cell.debugCellObject.transform.position.y - 1 * verticalFloors, 0.5f);
    }
}
