using UnityEngine;

namespace QuanticCollapse
{
    [CreateAssetMenu(fileName = "PlayerStarshipData", menuName = "ScriptableObjects/StarshipData/Player")]
    public class PlayerStarshipData : StarshipData
    {
        private void Awake()
        {
            IsPlayerShip = true;
        }
    }
}