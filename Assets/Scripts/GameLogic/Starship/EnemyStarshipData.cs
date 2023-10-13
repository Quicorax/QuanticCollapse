using UnityEngine;

namespace QuanticCollapse
{
    [CreateAssetMenu(fileName = "EnemyStarshipData", menuName = "ScriptableObjects/StarshipData/Enemy")]
    public class EnemyStarshipData : StarshipData
    {
        private readonly int[] _energyGrid = new int[4];

        private void Awake()
        {
            IsPlayerShip = false;
        }

        public void Init()
        {
            CheckModuleActivation(_energyGrid);
        }
    }
}