using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CanvasDebugManager : MonoBehaviour
{
    [SerializeField] private TMP_Text[] rewardText;
    [SerializeField] private Slider[] moduleSlider;
    [SerializeField] private GameObject[] turnEnergyVisuals;
    [SerializeField] private CanvasGroup winPanel;
    [SerializeField] private CanvasGroup losePanel;
    public void SetTurns(int turnIndex) 
    {
        for (int i = 0; i < turnEnergyVisuals.Length; i++)
        {
            if(turnIndex > i)
                turnEnergyVisuals[i].transform.DOScale(Vector2.one, 0.3f).SetEase(Ease.OutBack);
            else
                turnEnergyVisuals[i].transform.DOScale(Vector2.zero, 0.3f).SetEase(Ease.InBack);
        }
    }

    public void SetMaxModuleSliderPower(int moduleIndex, int maxPower) { moduleSlider[moduleIndex].maxValue = maxPower; }
    public void AddModuleSlider(int moduleIndex, int value) { moduleSlider[moduleIndex].value += value; }
    public void ResetModuleSlider(int moduleIndex) { moduleSlider[moduleIndex].value = 0; }

    public void PlayerWin()
    {
        winPanel.alpha = 1;
        winPanel.blocksRaycasts = true;
    }
    public void PlayerLose()
    {
        losePanel.alpha = 1;
        losePanel.blocksRaycasts = true;
    }

    public void SetRewardTextToWinPanel(string kind, int amount)
    {
        if(kind == "Dilithium")
            rewardText[0].text = amount.ToString();
        if(kind == "AlianceCredits")
            rewardText[1].text = amount.ToString();
        if(kind == "Reputation")
            rewardText[2].text = amount.ToString();
    }
}
