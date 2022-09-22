using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ModularPopUp : MonoBehaviour
{
    private int initialPanelOffset = 35;

    [SerializeField] private CanvasGroup CanvasGroup;
    [SerializeField] private RectTransform Parent;
    [SerializeField] private VerticalLayoutGroup Layout;

    private AddressablesService _addressables;
    private float _moduleSize;
    private void Awake()
    {
        _addressables = ServiceLocator.GetService<AddressablesService>();
    }
    public void GeneratePopUp(IPopUpComponentData[] ModulesToAdd)
    {
        CanvasGroup.alpha = 0;
        int currentModules = 0;

        _moduleSize = initialPanelOffset;
        foreach (var moduleData in ModulesToAdd)
        {
            _moduleSize += moduleData.ModuleHeight + 20;

            string adressableKey = Constants.PopUpModule + moduleData.ModuleConcept;

            _addressables.SpawnAddressable<IPopUpComponentObject>(adressableKey, Parent, x => 
            {
                x.SetData(moduleData, CloseSelf);
                currentModules++;

                if (currentModules == ModulesToAdd.Length)
                    GenerationComplete();
            });
        }
    }
    public void CloseSelf() 
    {
        CanvasGroup.interactable = false;
        CanvasGroup.DOFade(0, 0.2f).OnComplete(()=> _addressables.ReleaseAddressable(gameObject));
    }

    void GenerationComplete() 
    {
        Parent.sizeDelta += new Vector2(0, _moduleSize);

        Parent.DOPunchScale(Vector3.one * 0.1f, .5f);
        CanvasGroup.DOFade(1, 0.3f);
        StartCoroutine(SetElementsOnDisposition());
    }
    IEnumerator SetElementsOnDisposition()
    {
        Layout.enabled = true;
        yield return new WaitForEndOfFrame();
        Layout.enabled = false;
    }
}
