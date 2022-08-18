using TMPro;
using UnityEngine.UI;

public class ExternalBoosterBase
{
    public VirtualGridView View;
    public MasterSceneManager MasterSceneManager;
    public Button buttonRef;
    public TMP_Text textRef;

    public void SetBoosterCountText(int count, TMP_Text text) { text.text = count.ToString(); }
    public bool CheckBoosterNotEmpty(int boosterAmount) { return boosterAmount > 0; }
}