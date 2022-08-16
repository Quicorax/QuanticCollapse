using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExternalBoosterManager : MonoBehaviour
{
    private VirtualGridView View;
    private MasterSceneManager MasterSceneManager;

    public StartshipScreenVisualEvents screenVisualEvents;
    public UserInputManager _inputManager;

    [Header("FirstAidKid")]
    public ParticleSystem firstAidParticlesEffect;
    public TMP_Text firstAidTextRef;
    public Button firstAidButton;
    public int lifeRegenAmount = 5;

    [Header("EasyTrigger")]
    public ParticleSystem easyTriggerParticlesEffect;
    public TMP_Text easyTriggerTextRef;
    public Button easyTriggerButton;
    public int lifeSubstractionAmount = 2;

    [Header("DeAtomizer")]
    public TMP_Text deAtomizerTextRef;
    public Button deAtomizerButton;

    private void Awake()
    {
        View = FindObjectOfType<VirtualGridView>();
        MasterSceneManager = FindObjectOfType<MasterSceneManager>();
    }

    private void Start()
    {
        SetBoosterCountText(MasterSceneManager.runtimeSaveFiles.progres.fistAidKidBoosterAmount, firstAidTextRef);
        SetBoosterCountText(MasterSceneManager.runtimeSaveFiles.progres.easyTriggerBoosterAmount, easyTriggerTextRef);
        SetBoosterCountText(MasterSceneManager.runtimeSaveFiles.progres.deAthomizerBoosterAmount, deAtomizerTextRef);
    }

    public void ExecuteFistAidKit()
    {
        if (View.Controller.Model.PlayerLife >= View.Controller.Model.playerMaxLife)
            return;

        View.ModifyPlayerLife(lifeRegenAmount);
        firstAidParticlesEffect.Play();
        screenVisualEvents.Healed();

        MasterSceneManager.runtimeSaveFiles.progres.fistAidKidBoosterAmount--;
        SetBoosterCountText(MasterSceneManager.runtimeSaveFiles.progres.fistAidKidBoosterAmount, firstAidTextRef);
        firstAidButton.interactable = MasterSceneManager.runtimeSaveFiles.progres.fistAidKidBoosterAmount > 0;
    }
    public void ExecuteEasyTrigger()
    {
        View.ModifyEnemyLife(-lifeSubstractionAmount);
        easyTriggerParticlesEffect.Play();

        MasterSceneManager.runtimeSaveFiles.progres.easyTriggerBoosterAmount--;
        SetBoosterCountText(MasterSceneManager.runtimeSaveFiles.progres.easyTriggerBoosterAmount, easyTriggerTextRef);
        easyTriggerButton.interactable = MasterSceneManager.runtimeSaveFiles.progres.easyTriggerBoosterAmount > 0;
    }
    public void ExecuteDeAtomizer()
    {
        if (_inputManager.blockLaserBoosterInput)
        {
            MasterSceneManager.runtimeSaveFiles.progres.deAthomizerBoosterAmount++;
            deAtomizerTextRef.text = MasterSceneManager.runtimeSaveFiles.progres.deAthomizerBoosterAmount.ToString();
            _inputManager.blockLaserBoosterInput = false;
            return;
        }

        MasterSceneManager.runtimeSaveFiles.progres.deAthomizerBoosterAmount--;
        SetBoosterCountText(MasterSceneManager.runtimeSaveFiles.progres.deAthomizerBoosterAmount, deAtomizerTextRef);
        deAtomizerButton.interactable = MasterSceneManager.runtimeSaveFiles.progres.deAthomizerBoosterAmount > 0;

        _inputManager.blockLaserBoosterInput = true;
    }
    void SetBoosterCountText(int count, TMP_Text text) { text.text = count.ToString(); }
}
