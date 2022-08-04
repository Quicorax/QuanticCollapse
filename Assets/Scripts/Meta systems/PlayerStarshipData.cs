using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStarshipData", menuName = "ScriptableObjects/StarshipData/Player")]
public class PlayerStarshipData : StarshipData
{
    private void Awake()
    {
        isPlayerShip = true;
    }
}
