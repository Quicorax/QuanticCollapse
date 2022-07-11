using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasDebugManager : MonoBehaviour
{
    public TMP_Text turnsText;

    public Slider[] moduleSlider;

    public void SetTurns(string turnSring) { turnsText.text = turnSring; }
    public void SetMaxModuleSliderPower(int moduleIndex, int maxPower) { moduleSlider[moduleIndex].maxValue = maxPower; }
    public void UpdateModuleSlider(int moduleIndex, int value) { moduleSlider[moduleIndex].value = value; }
}
