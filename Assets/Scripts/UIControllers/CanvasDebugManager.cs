using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasDebugManager : MonoBehaviour
{
    public TMP_Text turnsText;

    public Slider[] moduleSlider;
    public Slider[] lifeSliders;

    public CanvasGroup winPanel;
    public CanvasGroup losePanel;
    public void SetTurns(string turnSring) { turnsText.text = turnSring; }
    public void SetMaxModuleSliderPower(int moduleIndex, int maxPower) { moduleSlider[moduleIndex].maxValue = maxPower; }
    public void AddModuleSlider(int moduleIndex, int value) { moduleSlider[moduleIndex].value += value; }
    public void ResetModuleSlider(int moduleIndex) { moduleSlider[moduleIndex].value = 0; }
    public void SetMaxLifeModuleSlider(int maxLife)
    {
        for (int i = 0; i < lifeSliders.Length; i++)
        {
            lifeSliders[i].maxValue = maxLife;
        }
    }

    public void ChangePlayerLife(int amount)
    {
        lifeSliders[0].value += amount;
    }
    public void ChangeEnemyLife(int amount)
    {
        lifeSliders[1].value += amount;
    }

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
