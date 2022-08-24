using DG.Tweening;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private SendMasterReferenceEventBus _MasterReference;
    private MasterSceneManager _MasterSceneManager;

    [Header("Element Prices")]
    [SerializeField] private int fistAidStandardPrice;
    [SerializeField] private int easyTriggerStandardPrice;
    [SerializeField] private int deAthomizerStandardPrice;
    [SerializeField] private int dilithiumStandardPrice;
    //public int alianceCreditsStandardPrice;

    [Header("Element Price Text Display")]
    [SerializeField] private TMP_Text fistAidStandardPriceText;
    [SerializeField] private TMP_Text easyTriggerStandardPriceText;
    [SerializeField] private TMP_Text deAthomizerStandardPriceText;
    [SerializeField] private TMP_Text dilithiumStandardPriceText;
    //public TMP_Text alianceCreditsStandardPriceText;

    [Header("Element Current Amount Display")]
    [SerializeField] private TMP_Text fistAidStandardCurrentAmountText;
    [SerializeField] private TMP_Text easyTriggerStandardCurrentAmountText;
    [SerializeField] private TMP_Text deAthomizerStandardCurrentAmountText;
    [SerializeField] private TMP_Text dilithiumStandardCurrentAmountText;
    [SerializeField] private TMP_Text alianceCreditsStandardCurrentAmountText;

    private bool creditsPopUpFading;
    [SerializeField] private CanvasGroup CreditsCapPopUp;

    private void Awake()
    {
        _MasterReference.Event += SetMasterReference;
    }
    private void OnDestroy()
    {
        _MasterReference.Event -= SetMasterReference;
    }
    private void Start()
    {
        SetElementPrices();
        UpdateExtermalBoostersAmount();
    }
    void SetMasterReference(MasterSceneManager masterReference) { _MasterSceneManager = masterReference; }
    private void SetElementPrices()
    {
        fistAidStandardPriceText.text = fistAidStandardPrice.ToString();
        easyTriggerStandardPriceText.text = easyTriggerStandardPrice.ToString();
        deAthomizerStandardPriceText.text = deAthomizerStandardPrice.ToString();
        dilithiumStandardPriceText.text = dilithiumStandardPrice.ToString();
        //alianceCreditsStandardPriceText.text = alianceCreditsStandardPrice.ToString();
    }

    void UpdateExtermalBoostersAmount()
    {
        fistAidStandardCurrentAmountText.text = _MasterSceneManager.SaveFiles.progres.fistAidKitBoosterAmount.ToString();
        easyTriggerStandardCurrentAmountText.text = _MasterSceneManager.SaveFiles.progres.easyTriggerBoosterAmount.ToString();
        deAthomizerStandardCurrentAmountText.text = _MasterSceneManager.SaveFiles.progres.deAthomizerBoosterAmount.ToString();
    }
    public void TryBuyExternalBooster(ExternalBoosterKind externalBoosterKind, out int credits)
    {
        credits = _MasterSceneManager.SaveFiles.progres.alianceCreditsAmount;

            switch (externalBoosterKind)
            {
                case ExternalBoosterKind.FistAidKit:
                    if (credits >= fistAidStandardPrice)
                    {
                        credits -= fistAidStandardPrice;
                        _MasterSceneManager.SaveFiles.progres.fistAidKitBoosterAmount++;
                    }
                else
                    NotifyNotEnoughtCredits();
                break;
                    case ExternalBoosterKind.EasyTrigger:
                    if (credits >= easyTriggerStandardPrice)
                    {
                        credits -= easyTriggerStandardPrice;
                        _MasterSceneManager.SaveFiles.progres.easyTriggerBoosterAmount++;
                    }
                    else
                        NotifyNotEnoughtCredits();
                break;
                case ExternalBoosterKind.DeAthomizer:
                    if (credits >= deAthomizerStandardPrice)
                    {
                        credits -= deAthomizerStandardPrice;
                        _MasterSceneManager.SaveFiles.progres.deAthomizerBoosterAmount++;
                    }
                    else
                        NotifyNotEnoughtCredits();
                break;
            }

        UpdateExtermalBoostersAmount();
        _MasterSceneManager.SaveFiles.progres.alianceCreditsAmount = credits;
    }
    public void TryBuyDilithium(out int credits)
    {
        credits = _MasterSceneManager.SaveFiles.progres.alianceCreditsAmount;

        if (credits >= dilithiumStandardPrice)
        {
            credits -= dilithiumStandardPrice;
            _MasterSceneManager.SaveFiles.progres.dilithiumAmount++;
            dilithiumStandardCurrentAmountText.text = _MasterSceneManager.SaveFiles.progres.dilithiumAmount.ToString();
        }
        else
            NotifyNotEnoughtCredits();

        _MasterSceneManager.SaveFiles.progres.alianceCreditsAmount = credits;
    }
    public void TryBuyCredits()
    {
        _MasterSceneManager.SaveFiles.progres.alianceCreditsAmount += 10;
        alianceCreditsStandardCurrentAmountText.text = _MasterSceneManager.SaveFiles.progres.alianceCreditsAmount.ToString();
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
