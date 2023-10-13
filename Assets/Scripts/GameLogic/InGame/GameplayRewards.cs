using System.Collections.Generic;
using UnityEngine;

namespace QuanticCollapse
{
    public class GameplayRewards : MonoBehaviour
    {
        [HideInInspector] public LevelModel LevelData;

        [SerializeField] private LevelInjectedEventBus _LevelInjected;
        [SerializeField] private GenericEventBus _WinConditionEventBus;

        [SerializeField] private GameplayCanvasManager _canvas;

        private GameProgressionService _gameProgression;

        private void Awake()
        {
            _WinConditionEventBus.Event += GiveRewards;
            _LevelInjected.Event += SetLevelData;

            _gameProgression = ServiceLocator.GetService<GameProgressionService>();
        }

        private void OnDestroy()
        {
            _WinConditionEventBus.Event -= GiveRewards;
            _LevelInjected.Event -= SetLevelData;
        }

        private void SetLevelData(LevelModel levelReference) => LevelData = levelReference;

        private void GiveRewards()
        {
            List<Reward> rewards = new();

            if (!_gameProgression.CheckLevelWithIndexIsCompleted(LevelData.Level))
            {
                _gameProgression.UpdateElement("Reputation", 1);
                _gameProgression.SetLevelWithIndexCompleted(LevelData.Level);

                rewards.Add(new Reward("Reputation", 1));
            }

            foreach (var reward in LevelData.Reward)
            {
                if (reward.RewardChance >= Random.Range(0, 100))
                {
                    var finalAmount = Random.Range(reward.RewardMinAmount, reward.RewardMaxAmount);
                    _gameProgression.UpdateElement(reward.RewardId, finalAmount);
                    rewards.Add(new Reward(reward.RewardId, finalAmount));
                }
            }

            _canvas.PlayerWinPopUp(rewards);
        }
    }
}