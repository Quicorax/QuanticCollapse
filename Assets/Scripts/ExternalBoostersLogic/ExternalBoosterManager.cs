using System.Collections.Generic;
using UnityEngine;

public enum ExternalBoosterKind { FistAidKit, EasyTrigger, DeAthomizer};

[System.Serializable]
public class ExternalBoosterHolder
{
    public ExternalBoosterKind kind;
    public ExternalBoosterElements elements;

    public bool CheckExpectedExternalBooster(ExternalBoosterKind expectedBoosterKind)
    {
        return expectedBoosterKind == kind;
    }
}
public class ExternalBoosterManager : MonoBehaviour
{
    [SerializeField] private List<ExternalBoosterHolder> ExternalBoosterElementsHolder = new();

    [SerializeField] private UserInputManager _inputManager;

    private VirtualGridView View;
    private MasterSceneManager _MasterSceneManager;

    private FistAidExternalBooster fistAidExternalBooster;
    private EasyTriggerExternalBooster easyTriggerExternalBooster;
    private DeAthomizerExternalBooster deAthomizerExternalBooster;

    private void Awake()
    {
        _MasterSceneManager = FindObjectOfType<MasterSceneManager>();
        View = FindObjectOfType<VirtualGridView>();
    }

    private void Start()
    {
        SetInitialExternalBoosters();
    }

    private void SetInitialExternalBoosters()
    {
        if (TryGetSpecificBoosterElements(ExternalBoosterKind.FistAidKit, out ExternalBoosterElements fistAidSpecificElements))
        {
            fistAidExternalBooster = new(View, _MasterSceneManager, fistAidSpecificElements);
        }

        if (TryGetSpecificBoosterElements(ExternalBoosterKind.EasyTrigger, out ExternalBoosterElements easyTriggerSpecificElements))
        {
            easyTriggerExternalBooster = new(View, _MasterSceneManager, easyTriggerSpecificElements);
        }

        if (TryGetSpecificBoosterElements(ExternalBoosterKind.DeAthomizer, out ExternalBoosterElements deAthomizerSpecificElements))
        {
            deAthomizerExternalBooster = new(View, _MasterSceneManager, deAthomizerSpecificElements);
        }
    }
    private bool TryGetSpecificBoosterElements(ExternalBoosterKind expectedKind, out ExternalBoosterElements elements)
    {
        foreach (var element in ExternalBoosterElementsHolder)
        {
            if (element.kind == expectedKind)
            {
                elements = element.elements;
                return true;
            }
        }

        elements = new();
        return false;
    }

    public void ExecuteFistAidKit()
    {
        if (View.Controller.Model.PlayerLife >= View.Controller.Model.playerMaxLife)
            return;

        fistAidExternalBooster.Execute();
    }

    public void ExecuteEasyTrigger()
    {
        easyTriggerExternalBooster.Execute();
    }

    public void ExecuteDeAtomizer()
    {
        if (_inputManager.blockLaserBoosterInput)
        {
            _MasterSceneManager.runtimeSaveFiles.progres.deAthomizerBoosterAmount++;
            deAthomizerExternalBooster.SetCountText();
            _inputManager.blockLaserBoosterInput = false;
            return;
        }

        deAthomizerExternalBooster.Execute();

        _inputManager.blockLaserBoosterInput = true;
    }
}
