using System;
using TMPro;
using UnityEngine;

public class LevelView : MonoBehaviour
{
    const string Mission = "Mission ";

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
        _levelName.text = Mission + LevelModel.Level.ToString();
        _ReputationCap.text = LevelModel.ReputationCap.ToString();
    }

    public void NavigatoToSceneWithLevel()
    {
        if (LevelModel == null)
            return;

        _onLevelSelectedEvent?.Invoke(LevelModel);
    }
}


