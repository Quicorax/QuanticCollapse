using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStarshipData", menuName = "ScriptableObjects/StarshipeData/Player")]
public class PlayerStarshipData : StarshipData
{
    public int maxInteractions;

    private void Awake()
    {
        isPlayerShip = true;
    }
}
