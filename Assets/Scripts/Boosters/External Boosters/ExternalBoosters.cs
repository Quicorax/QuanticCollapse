using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExternalBoosters : MonoBehaviour
{
    [HideInInspector] public VirtualGridView View;
    [HideInInspector] public TMP_Text textRef;
    [HideInInspector] public Button selfButton;

    public int usesLeft = 5;
    public bool CheckUses(int uses) { return uses >= 1; }

    void Awake()
    {
        selfButton = GetComponent<Button>();
        textRef = GetComponentInChildren<TMP_Text>();
        View = FindObjectOfType<VirtualGridView>();
    }

    private void Start()
    {
        textRef.text = usesLeft.ToString();
    }

    public virtual void Used()
    {
        usesLeft--; 
        textRef.text = usesLeft.ToString();

        if (!CheckUses(usesLeft))
        {
            selfButton.interactable = false;
        }
    }
}
