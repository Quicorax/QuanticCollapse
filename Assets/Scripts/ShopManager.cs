using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private MasterSceneManager _MasterSceneManager;

    public int fistAidStandardPrice;
    public int easyTriggerStandardPrice;
    public int deAthomizerStandardPrice;

    public TMP_Text fistAidStandardPriceText;
    public TMP_Text easyTriggerStandardPriceText;
    public TMP_Text deAthomizerStandardPriceText;

    private void Awake()
    {
        _MasterSceneManager = FindObjectOfType<MasterSceneManager>();
    }
    private void Start()
    {
        fistAidStandardPriceText.text = fistAidStandardPrice.ToString();
        easyTriggerStandardPriceText.text = easyTriggerStandardPrice.ToString();
        deAthomizerStandardPriceText.text = deAthomizerStandardPrice.ToString();
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

        _MasterSceneManager.runtimeSaveFiles.progres.alianceCreditsAmount = credits;
    }

    void NotifyNotEnoughtCredits()
    {
        //TODO: Not enought credits to buy element Pop UP
    }
}
