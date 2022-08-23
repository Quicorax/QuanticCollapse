using TMPro;
using UnityEngine.UI;

public class ExternalBoosterBase
{
    public VirtualGridController Controller;
    public MasterSceneManager MasterSceneManager;
    public Button ButtonRef;
    public TMP_Text TextRef;

    public void SetBoosterCountText(int count, TMP_Text text) { text.text = count.ToString(); }
    public bool CheckBoosterNotEmpty(int boosterAmount) { return boosterAmount > 0; }
}