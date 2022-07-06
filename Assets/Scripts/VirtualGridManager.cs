using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static VirtualGridManager;

public class IndependentCell
{
    public Vector2 cellCoords;
    public GameObject debugCellObject;

    public ElementKind elementKind;
    public IndependentCell(Vector2 coords, GameObject debug, ElementKind kind = ElementKind.Empty)
    {
        cellCoords = coords;
        debugCellObject = debug;
        elementKind = kind;
    }
}
public class GridCell
{
    public VirtualGridManager virtualGridManager;

    public IndependentCell cell;

    public bool idSet;
    public int setId;
    public int setMemberAmount;

    public GridCell(VirtualGridManager grid, Vector2 coords, GameObject debug, ElementKind kind = ElementKind.Empty)
    {
        virtualGridManager = grid;
        cell = new IndependentCell(coords, debug, kind);
    }

    public void ResetGridCell()
    {
        if (virtualGridManager.spawnGraphics && cell.debugCellObject != null)
            GameObject.Destroy(cell.debugCellObject);

        cell = null;

        idSet = false;
        setId = 0;
        setMemberAmount = 0;


    }
    public void SetSetId(bool inheritedId, int id = 0)
    {
        idSet = true;
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

        if (virtualGridManager.spawnGraphics)
            cell.debugCellObject.transform.GetChild(1).GetComponent<TMP_Text>().text = setId.ToString();

        foreach (Vector2 vector in cell.cellCoords.GetCrossCoords())
        {
            if (virtualGridManager.virtualGrid.TryGetValue(vector, out GridCell otherCell) && otherCell.cell.elementKind == cell.elementKind && !otherCell.idSet)
            {
                otherCell.SetSetId(true, setId);
            }
        }
    }
}
public class VirtualGridManager : MonoBehaviour
{
    public TurnManager turnManager;

    public enum ElementKind { Empty, A, B, C, D };

    public int gridHeight;
    public int gridWidth;

    public Transform visualParent;
    public GameObject debugVisualCell;

    [HideInInspector]
    public Vector2 gridOffset;

    int setCount;

    public bool spawnGraphics;

    public Dictionary<Vector2, GridCell> virtualGrid = new();

    public Dictionary<int, List<Vector2>> setList = new();

    public List<Vector2> deletedElementCoords = new();

    void Start()
    {
        //InitGeneration();
    }
    public void InitGeneration()
    {
        SetGrid();
        FillGrid();

        AssignSetGrid();

        SendSetIntel();
    }
    void SetGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector2 coords = new Vector2(x, y);

                GameObject debObject = null;

                if (spawnGraphics)
                    debObject = Instantiate(debugVisualCell, coords, Quaternion.identity, visualParent);

                virtualGrid.Add(coords, new GridCell(this, coords, debObject));
            }
        }
        gridOffset = new Vector2(-(gridWidth / 2), -gridHeight);
        visualParent.position = gridOffset;
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
        foreach (var item in virtualGrid)
        {
            if (!item.Value.idSet)
                item.Value.SetSetId(false);
        }
    }
    void SendSetIntel()
    {
        foreach (var item in setList)
        {
            foreach (var a in item.Value)
            {
                virtualGrid[a].setMemberAmount = item.Value.Count;

                if (spawnGraphics)
                    virtualGrid[a].cell.debugCellObject.transform.GetChild(2).GetComponent<TMP_Text>().text = virtualGrid[a].setMemberAmount.ToString();
            }
        }
    }
    public int AddNewSet()
    {
        setCount++;
        return setCount;
    }
    ElementKind GetElementKind(GridCell debObj)
    {
        ElementKind kind = (ElementKind)Random.Range(1, System.Enum.GetValues(typeof(ElementKind)).Length);

        if (spawnGraphics)
        {
            debObj.cell.debugCellObject.transform.GetChild(0).GetComponent<TMP_Text>().text = kind.ToString();
            debObj.cell.debugCellObject.transform.GetChild(0).GetComponent<TMP_Text>().color = GetDebugColor(kind);
        }

        return kind;
    }
    Color GetDebugColor(ElementKind kind)
    {
        Color selectedColor;
        switch (kind)
        {
            case ElementKind.A:
                selectedColor = Color.blue;
                break;
            case ElementKind.B:
                selectedColor = Color.yellow;
                break;
            case ElementKind.C:
                selectedColor = Color.green;
                break;
            case ElementKind.D:
                selectedColor = Color.red;
                break;
            default:
                selectedColor = Color.white;
                break;
        }
        return selectedColor;
    }
    public void CheckElementOnGrid(Vector2 coords)
    {
        if (virtualGrid.TryGetValue(coords, out GridCell gridCell) && gridCell.cell != null)
        {
            Debug.Log(virtualGrid[coords].cell.elementKind + ": " + virtualGrid[coords].setMemberAmount);

            if (virtualGrid[coords].setMemberAmount >= 2)
            {
                SetInteraction(virtualGrid[coords]);
            }
        }
    }

    void SetInteraction(GridCell debObj)
    {
        deletedElementCoords.Clear();

        int setId = debObj.setId;
        turnManager.AddScoreOfKind(debObj.cell.elementKind, debObj.setMemberAmount);

        foreach (Vector2 coords in setList[setId])
        {
            virtualGrid[coords].ResetGridCell();
            deletedElementCoords.Add(coords);
        }
        setList.Remove(setId);

        //MoveUpperTiles();
    }

    //void MoveUpperTiles()
    //{
    //    foreach (Vector2 delCoords in deletedElementCoords)
    //    {
    //        foreach (Vector2 coords in virtualGrid.Keys)
    //        {
    //            if (coords.x == delCoords.x && coords.y > delCoords.y && virtualGrid[coords].cell != null)
    //            {
    //                TranspassCellData(delCoords);
    //            }
    //        }
    //    }
    //}
    //
    //void TranspassCellData(Vector2 coords)
    //{
    //    if (virtualGrid.TryGetValue(coords, out GridCell upperCell))
    //    {
    //        IndependentCell movedCell = new IndependentCell(coords, upperCell.cell.debugCellObject, upperCell.cell.elementKind);
    //        virtualGrid[coords].cell = movedCell;
    //
    //        virtualGrid[coords].cell.debugCellObject.transform.DOMoveY(coords.y + Vector2.down.y - upperCell.cell.cellCoords.y - 1, 0.5f);
    //    }
    //}

}
