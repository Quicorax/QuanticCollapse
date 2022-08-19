using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private MasterSceneManager master;

    public int fistAidStandardPrice;
    public int easyTriggerStandardPrice;
    public int deAthomizerStandardPrice;

    public TMP_Text fistAidStandardPriceText;
    public TMP_Text easyTriggerStandardPriceText;
    public TMP_Text deAthomizerStandardPriceText;

    private void Awake()
    {
        master = FindObjectOfType<MasterSceneManager>();
    }

    private void Start()
    {
        fistAidStandardPriceText.text = fistAidStandardPrice.ToString();
        easyTriggerStandardPriceText.text = easyTriggerStandardPrice.ToString();
        deAthomizerStandardPriceText.text = deAthomizerStandardPrice.ToString();
    }   

    public void TryBuyExternalBooster(ExternalBoosterKind externalBoosterKind, out int credits)
    {
        credits = master.runtimeSaveFiles.progres.alianceCreditsAmount;

            switch (externalBoosterKind)
            {
                case ExternalBoosterKind.FistAidKit:
                    if (credits >= fistAidStandardPrice)
                    {
                        credits -= fistAidStandardPrice;
                        master.runtimeSaveFiles.progres.fistAidKidBoosterAmount++;
                    }
                else
                    NotifyNotEnoughtCredits();
                break;
                    case ExternalBoosterKind.EasyTrigger:
                    if (credits >= easyTriggerStandardPrice)
                    {
                        credits -= easyTriggerStandardPrice;
                        master.runtimeSaveFiles.progres.easyTriggerBoosterAmount++;
                    }
                    else
                        NotifyNotEnoughtCredits();
                break;
                case ExternalBoosterKind.DeAthomizer:
                    if (credits >= deAthomizerStandardPrice)
                    {
                        credits -= deAthomizerStandardPrice;
                        master.runtimeSaveFiles.progres.deAthomizerBoosterAmount++;
                    }
                    else
                        NotifyNotEnoughtCredits();
                break;
            }

        master.runtimeSaveFiles.progres.alianceCreditsAmount = credits;
    }

    void NotifyNotEnoughtCredits()
    {
        //TODO: Not enought credits to buy element Pop UP
    }
}
