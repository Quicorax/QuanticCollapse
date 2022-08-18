using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public List<ExternalBoosterHolder> ExternalBoosterElementsHolder = new();

    [SerializeField] private UserInputManager _inputManager;

    private VirtualGridView View;
    private MasterSceneManager MasterSceneManager;


    private FistAidExternalBooster fistAidExternalBooster;
    private EasyTriggerExternalBooster easyTriggerExternalBooster;
    private DeAthomizerExternalBooster deAthomizerExternalBooster;

    private void Awake()
    {
        View = FindObjectOfType<VirtualGridView>();
        MasterSceneManager = FindObjectOfType<MasterSceneManager>();
    }

    private void Start()
    {
        SetInitialExternalBoosters();
    }
    void SetInitialExternalBoosters()
    {

        if (TryGetSpecificBoosterElements(ExternalBoosterKind.FistAidKit, out ExternalBoosterElements fistAidSpecificElements))
            fistAidExternalBooster = new(View, MasterSceneManager, fistAidSpecificElements);
        if (TryGetSpecificBoosterElements(ExternalBoosterKind.EasyTrigger, out ExternalBoosterElements easyTriggerSpecificElements))
            easyTriggerExternalBooster = new(View, MasterSceneManager, easyTriggerSpecificElements);
        if (TryGetSpecificBoosterElements(ExternalBoosterKind.DeAthomizer, out ExternalBoosterElements deAthomizerSpecificElements))
            deAthomizerExternalBooster = new(View, MasterSceneManager, deAthomizerSpecificElements);

    }
    bool TryGetSpecificBoosterElements(ExternalBoosterKind expectedKind, out ExternalBoosterElements elements)
    {
        foreach (var item in ExternalBoosterElementsHolder)
        {
            if (item.kind == expectedKind)
            {
                elements = item.elements;
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
            MasterSceneManager.runtimeSaveFiles.progres.deAthomizerBoosterAmount++;
            deAthomizerExternalBooster.SetCountText();
            _inputManager.blockLaserBoosterInput = false;
            return;
        }

        deAthomizerExternalBooster.Execute();

        _inputManager.blockLaserBoosterInput = true;
    }
}
