using System;
using TMPro;
using UnityEngine;

public class LevelView : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelName;
    [SerializeField] private TMP_Text _ReputationCap;

    [HideInInspector] public LevelModel LevelModel;

    private Action<LevelModel> _onLevelSelectedEvent;
    public void Initialize(LevelModel levelModel, Action<LevelModel> levelSelectedEvent)
    {
        _onLevelSelectedEvent = levelSelectedEvent;
        LevelModel = levelModel;
        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        string loc_text = ServiceLocator.GetService<LocalizationService>().Localize("LOBBY_MAIN_MISSION");
        _levelName.text = loc_text + LevelModel.Level.ToString();
        _ReputationCap.text = LevelModel.ReputationCap.ToString();
    }

    public void NavigatoToSceneWithLevel() => _onLevelSelectedEvent?.Invoke(LevelModel);
}


