using DG.Tweening;
using TMPro;
using UnityEngine;

public class ShopCanvas : MonoBehaviour
{
    const string Dilithium = "Dilithium";
    const string Reputation = "Reputation";
    const string AlianceCredits = "AlianceCredits";
    const string FirstAidKit = "FirstAidKit";
    const string EasyTrigger = "EasyTrigger";
    const string DeAthomizer = "DeAthomizer";

    [SerializeField] private GenericEventBus _ElementPurchased;
    [SerializeField] private GenericEventBus _NotEnoughtCredits;

    [SerializeField] private SendMasterReferenceEventBus _MasterReference;

    [SerializeField] private TMP_Text dilithium_Text;
    [SerializeField] private TMP_Text alianceCredits_Text;
    [SerializeField] private TMP_Text reputation_Text;
    [SerializeField] private TMP_Text fistAid_Text;
    [SerializeField] private TMP_Text easyTrigger_Text;
    [SerializeField] private TMP_Text deAthomizer_Text;

    private MasterSceneManager _MasterSceneManager;

    private bool creditsPopUpFading;
    [SerializeField] private CanvasGroup CreditsCapPopUp;

    private void Awake()
    {
        _MasterReference.Event += SetMasterReference;
        _ElementPurchased.Event += UpdateInventoryVisualAmount;
        _NotEnoughtCredits.Event += NotifyNotEnoughtCredits;
    }
    private void OnDisable()
    {
        _MasterReference.Event -= SetMasterReference;
        _ElementPurchased.Event -= UpdateInventoryVisualAmount;
        _NotEnoughtCredits.Event -= NotifyNotEnoughtCredits;
    }
    private void Start()
    {
        UpdateInventoryVisualAmount();
    }
    void SetMasterReference(MasterSceneManager masterReference) { _MasterSceneManager = masterReference; }
    public void UpdateInventoryVisualAmount()
    {
        dilithium_Text.text = _MasterSceneManager.Inventory.CheckElementAmount(Dilithium).ToString();
        alianceCredits_Text.text = _MasterSceneManager.Inventory.CheckElementAmount(AlianceCredits).ToString();
        reputation_Text.text = _MasterSceneManager.Inventory.CheckElementAmount(Reputation).ToString();

        fistAid_Text.text = _MasterSceneManager.Inventory.CheckElementAmount(FirstAidKit).ToString();
        easyTrigger_Text.text = _MasterSceneManager.Inventory.CheckElementAmount(EasyTrigger).ToString();
        deAthomizer_Text.text = _MasterSceneManager.Inventory.CheckElementAmount(DeAthomizer).ToString();
    }

    void NotifyNotEnoughtCredits()
    {
        if (creditsPopUpFading)
            return;

        creditsPopUpFading = true;
        CreditsCapPopUp.alpha = 1;
        CreditsCapPopUp.gameObject.SetActive(true);
        CreditsCapPopUp.transform.DOPunchScale(Vector3.one * 0.1f, .5f);
        CreditsCapPopUp.DOFade(0, 2f).SetEase(Ease.InCirc).OnComplete(() =>
        {
            CreditsCapPopUp.gameObject.SetActive(false);
            creditsPopUpFading = false;
        });
    }
}
