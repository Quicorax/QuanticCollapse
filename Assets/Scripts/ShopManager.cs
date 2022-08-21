using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private MasterSceneManager _MasterSceneManager;

    [Header("Element Prices")]
    public int fistAidStandardPrice;
    public int easyTriggerStandardPrice;
    public int deAthomizerStandardPrice;
    public int dilithiumStandardPrice;
    //public int alianceCreditsStandardPrice;

    [Header("Element Price Text Display")]
    public TMP_Text fistAidStandardPriceText;
    public TMP_Text easyTriggerStandardPriceText;
    public TMP_Text deAthomizerStandardPriceText;
    public TMP_Text dilithiumStandardPriceText;
    //public TMP_Text alianceCreditsStandardPriceText;

    [Header("Element Current Amount Display")]
    public TMP_Text fistAidStandardCurrentAmountText;
    public TMP_Text easyTriggerStandardCurrentAmountText;
    public TMP_Text deAthomizerStandardCurrentAmountText;
    public TMP_Text dilithiumStandardCurrentAmountText;
    public TMP_Text alianceCreditsStandardCurrentAmountText;

    private void Awake()
    {
        _MasterSceneManager = FindObjectOfType<MasterSceneManager>();
    }
    private void Start()
    {
        fistAidStandardPriceText.text = fistAidStandardPrice.ToString();
        easyTriggerStandardPriceText.text = easyTriggerStandardPrice.ToString();
        deAthomizerStandardPriceText.text = deAthomizerStandardPrice.ToString();
        dilithiumStandardPriceText.text = dilithiumStandardPrice.ToString();
        //alianceCreditsStandardPriceText.text = alianceCreditsStandardPrice.ToString();

        UpdateExtermalBoostersAmount();
    }

    void UpdateExtermalBoostersAmount()
    {
        fistAidStandardCurrentAmountText.text = _MasterSceneManager.runtimeSaveFiles.progres.fistAidKidBoosterAmount.ToString();
        easyTriggerStandardCurrentAmountText.text = _MasterSceneManager.runtimeSaveFiles.progres.easyTriggerBoosterAmount.ToString();
        deAthomizerStandardCurrentAmountText.text = _MasterSceneManager.runtimeSaveFiles.progres.deAthomizerBoosterAmount.ToString();
    }
    public void TryBuyExternalBooster(ExternalBoosterKind externalBoosterKind, out int credits)
    {
        credits = _MasterSceneManager.runtimeSaveFiles.progres.alianceCreditsAmount;

            switch (externalBoosterKind)
            {
                case ExternalBoosterKind.FistAidKit:
                    if (credits >= fistAidStandardPrice)
                    {
                        credits -= fistAidStandardPrice;
                        _MasterSceneManager.runtimeSaveFiles.progres.fistAidKidBoosterAmount++;
                    }
                else
                    NotifyNotEnoughtCredits();
                break;
                    case ExternalBoosterKind.EasyTrigger:
                    if (credits >= easyTriggerStandardPrice)
                    {
                        credits -= easyTriggerStandardPrice;
                        _MasterSceneManager.runtimeSaveFiles.progres.easyTriggerBoosterAmount++;
                    }
                    else
                        NotifyNotEnoughtCredits();
                break;
                case ExternalBoosterKind.DeAthomizer:
                    if (credits >= deAthomizerStandardPrice)
                    {
                        credits -= deAthomizerStandardPrice;
                        _MasterSceneManager.runtimeSaveFiles.progres.deAthomizerBoosterAmount++;
                    }
                    else
                        NotifyNotEnoughtCredits();
                break;
            }

        UpdateExtermalBoostersAmount();
        _MasterSceneManager.runtimeSaveFiles.progres.alianceCreditsAmount = credits;
    }
    public void TryBuyDilithium(out int credits)
    {
        credits = _MasterSceneManager.runtimeSaveFiles.progres.alianceCreditsAmount;

        if (credits >= dilithiumStandardPrice)
        {
            credits -= dilithiumStandardPrice;
            _MasterSceneManager.runtimeSaveFiles.progres.dilithiumAmount++;
            dilithiumStandardCurrentAmountText.text = _MasterSceneManager.runtimeSaveFiles.progres.dilithiumAmount.ToString();
        }
        else
            NotifyNotEnoughtCredits();

        _MasterSceneManager.runtimeSaveFiles.progres.alianceCreditsAmount = credits;
    }
    public void TryBuyCredits()
    {
        _MasterSceneManager.runtimeSaveFiles.progres.alianceCreditsAmount++;
        alianceCreditsStandardCurrentAmountText.text = _MasterSceneManager.runtimeSaveFiles.progres.alianceCreditsAmount.ToString();
    }
    void NotifyNotEnoughtCredits()
    {
        //TODO: Not enought credits to buy element Pop UP
    }
}
