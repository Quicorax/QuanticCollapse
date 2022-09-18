using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ModularPopUp : MonoBehaviour
{
    private int initialPanelOffset = 35;

    [SerializeField] private CanvasGroup CanvasGroup;
    [SerializeField] private RectTransform Parent;
    [SerializeField] private VerticalLayoutGroup Layout;

    private AddressablesService _addressables;

    private void Awake()
    {
        _addressables = ServiceLocator.GetService<AddressablesService>();
    }
    public async void GeneratePopUp(List<PopUpComponentData> ModulesToAdd) //TODO: Turn the PopUpComponentData to array not new List
    {
        CanvasGroup.alpha = 0;

        int finalSize = initialPanelOffset;
        foreach (var moduleData in ModulesToAdd)
        {
            finalSize += moduleData.ModuleHeight + 20;

            string adressableKey = Constants.PopUpModule + moduleData.ModuleConcept;

            var adrsInstance = await _addressables
                .SpawnAddressable<PopUpComponentObject>(adressableKey, Parent);

            adrsInstance.SetData(moduleData, CloseSelf);
        }

        GenerationComplete();
        Parent.sizeDelta += new Vector2(0, finalSize);

        Parent.DOPunchScale(Vector3.one * 0.1f, .5f);
        CanvasGroup.DOFade(1, 0.3f);
    }
    public void CloseSelf() 
    {
        CanvasGroup.interactable = false;
        CanvasGroup.DOFade(0, 0.2f).OnComplete(()=> _addressables.ReleaseAddressable(gameObject));
    }

    void GenerationComplete() => StartCoroutine(SetElementsOnDisposition());
    IEnumerator SetElementsOnDisposition()
    {
        Layout.enabled = true;
        yield return new WaitForEndOfFrame();
        Layout.enabled = false;
    }
}
