using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoosterKindBased : BaseBooster
{
    private GameConfigService _config;

    public BoosterKindBased()
    {
        _config = ServiceLocator.GetService<GameConfigService>();
    }

    public int BoosterKindId => 6;

    public void OnInteraction(Vector2Int initialCoords, GridController Controller)
    {
        List<Vector2Int> coordsToCheck = new();
        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 7; y++)
                coordsToCheck.Add(new Vector2Int(x, y));
        }

        int n = Random.Range(0, _config.GridBlocks.Where(item => !item.IsBooster).Count());
        int kindId = _config.GridBlocks[n].Id;

        foreach (var coords in coordsToCheck)
        {
            if (Controller.Model.VirtualGrid.TryGetValue(coords, out GridCellController cell) 
                && cell.CheckHasBlock() 
                && cell.GetBlockId() == kindId)
            {
                Controller.InteractionsController.MatchClosedList.Add(cell);
            }
        }
    }
}
