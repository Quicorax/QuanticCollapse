using System;
using TMPro;
using UnityEngine;

namespace QuanticCollapse
{
    public class LevelView : MonoBehaviour
    {
        [HideInInspector] public LevelModel LevelModel;

        [SerializeField] private TMP_Text _levelName;
        [SerializeField] private TMP_Text _ReputationCap;

        private Action<LevelModel> _onLevelSelectedEvent;

        public void Initialize(LevelModel levelModel, Action<LevelModel> levelSelectedEvent)
        {
            _onLevelSelectedEvent = levelSelectedEvent;
            LevelModel = levelModel;
            UpdateVisuals();
        }

        public void NavigateToSceneWithLevel() => _onLevelSelectedEvent?.Invoke(LevelModel);

        private void UpdateVisuals()
        {
            var text = ServiceLocator.GetService<LocalizationService>().Localize("LOBBY_MAIN_MISSION");
            _levelName.text = text + LevelModel.Level.ToString();
            _ReputationCap.text = LevelModel.ReputationCap.ToString();
        }
    }
}