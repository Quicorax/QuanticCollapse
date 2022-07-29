using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStarshipData", menuName = "ScriptableObjects/StarshipeData/Player")]
public class PlayerStarshipData : StarshipData
{
    private void Awake()
    {
        isPlayerShip = true;
    }
}
