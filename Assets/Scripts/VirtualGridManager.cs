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

    public ElementKind elementKind;

    public bool idSet;
    public int setId;
    public int setMemberAmount;

    public GridCell(VirtualGridManager grid, Vector2 coords, GameObject debug, ElementKind kind = ElementKind.Empty)
    {
        virtualGridManager = grid;
        cell = new IndependentCell(coords, debug, kind);
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
            if (virtualGridManager.virtualGrid.TryGetValue(vector, out GridCell cell) && cell.elementKind == elementKind && !cell.idSet)
            {
                cell.SetSetId(true, setId);
            }
        }
    }
}
public class VirtualGridManager : MonoBehaviour
{
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

    void Start()
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
            item.Value.elementKind = GetElementKind(item.Value);
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
        if (virtualGrid.ContainsKey(coords))
        {
            Debug.Log(virtualGrid[coords].elementKind + ": " + virtualGrid[coords].setMemberAmount);
        }
    }
}
