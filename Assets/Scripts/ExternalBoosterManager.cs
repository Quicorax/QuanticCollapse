using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExternalBoosterManager : MonoBehaviour
{
    private VirtualGridView View;
    private MasterSceneManager MasterSceneManager;

    [SerializeField] private StartshipScreenVisualEvents screenVisualEvents;
    [SerializeField] private UserInputManager _inputManager;

    [Header("FirstAidKid")]
    [SerializeField] private ParticleSystem firstAidParticlesEffect;
    [SerializeField] private TMP_Text firstAidTextRef;
    [SerializeField] private Button firstAidButton;
    [SerializeField] private int lifeRegenAmount = 5;

    [Header("EasyTrigger")]
    [SerializeField] private ParticleSystem easyTriggerParticlesEffect;
    [SerializeField] private TMP_Text easyTriggerTextRef;
    [SerializeField] private Button easyTriggerButton;
    [SerializeField] private int lifeSubstractionAmount = 2;

    [Header("DeAtomizer")]
    [SerializeField] private TMP_Text deAtomizerTextRef;
    [SerializeField] private Button deAtomizerButton;

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
        firstAidButton.interactable = CheckBoosterNotEmpty(MasterSceneManager.runtimeSaveFiles.progres.fistAidKidBoosterAmount);
    }
    public void ExecuteEasyTrigger()
    {
        View.ModifyEnemyLife(-lifeSubstractionAmount);
        easyTriggerParticlesEffect.Play();

        MasterSceneManager.runtimeSaveFiles.progres.easyTriggerBoosterAmount--;
        SetBoosterCountText(MasterSceneManager.runtimeSaveFiles.progres.easyTriggerBoosterAmount, easyTriggerTextRef);
        easyTriggerButton.interactable = CheckBoosterNotEmpty(MasterSceneManager.runtimeSaveFiles.progres.easyTriggerBoosterAmount);
    }
    public void ExecuteDeAtomizer()
    {
        if (_inputManager.blockLaserBoosterInput)
        {
            MasterSceneManager.runtimeSaveFiles.progres.deAthomizerBoosterAmount++;
            SetBoosterCountText(MasterSceneManager.runtimeSaveFiles.progres.deAthomizerBoosterAmount, deAtomizerTextRef);
            _inputManager.blockLaserBoosterInput = false;
            return;
        }

        MasterSceneManager.runtimeSaveFiles.progres.deAthomizerBoosterAmount--;
        SetBoosterCountText(MasterSceneManager.runtimeSaveFiles.progres.deAthomizerBoosterAmount, deAtomizerTextRef);
        deAtomizerButton.interactable = CheckBoosterNotEmpty(MasterSceneManager.runtimeSaveFiles.progres.deAthomizerBoosterAmount);

        _inputManager.blockLaserBoosterInput = true;
    }

    void SetBoosterCountText(int count, TMP_Text text) { text.text = count.ToString(); }
    bool CheckBoosterNotEmpty(int boosterAmount) { return boosterAmount > 0; }
}
