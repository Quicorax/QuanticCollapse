using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CanvasDebugManager : MonoBehaviour
{
    public Slider[] moduleSlider;
    public GameObject[] turnEnergyVisuals;

    public CanvasGroup winPanel;
    public CanvasGroup losePanel;
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
}
