using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoosterKindBased : BaseBooster
{
    private GameConfigService _config;
    private int _id;
    public BoosterKindBased(int id)
    {
        _config = ServiceLocator.GetService<GameConfigService>();
        _id = id;
    }

    public int BoosterKindId => _id;

    public void OnInteraction(Vector2Int initialCoords, GridModel Model)
    {
        List<Vector2Int> coordsToCheck = new();
        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 7; y++)
                coordsToCheck.Add(new Vector2Int(x, y));
        }

        int n = Random.Range(0, _config.GridBlocks.BaseBlocks.Count());
        int kindId = _config.GridBlocks.BaseBlocks[n].Id;

        foreach (var coords in coordsToCheck)
        {
            if (Model.GridData.TryGetValue(coords, out GridCellModel cell) 
                && cell.BlockModel != null && cell.BlockModel.Id == kindId)
            {
                Model.MatchClosedList.Add(cell);
            }
        }
    }
}
