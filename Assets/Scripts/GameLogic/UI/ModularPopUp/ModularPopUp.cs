using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class ModularPopUp : MonoBehaviour
{
    private int initialPanelOffset = 35;

    [SerializeField] private CanvasGroup CanvasGroup;
    [SerializeField] private RectTransform Body;
    [SerializeField] private VerticalLayoutGroup Layout;

    public void GeneratePopUp(List<PopUpComponentData> ModulesToAdd, bool isVisual = true)
    {
        if (isVisual)
        {
            Body.DOPunchScale(Vector3.one * 0.1f, .5f);
            CanvasGroup.DOFade(0, 0.2f).From();
        }

        int spawnedModules = 0;

        int finalSize = initialPanelOffset;
        foreach (var moduleData in ModulesToAdd)
        {
            finalSize += moduleData.ModuleHeight + 20;

            string adressableKey = Constants.PopUpModule + moduleData.ModuleConcept;
            Addressables.LoadAssetAsync<GameObject>(adressableKey).Completed += handle =>
            {
                Addressables.InstantiateAsync(adressableKey, Body).Result.GetComponent<PopUpComponentObject>().SetData(moduleData, CloseSelf);
                spawnedModules++;

                if (spawnedModules == ModulesToAdd.Count)
                    GenerationComplete();
            };
        }

        Body.sizeDelta += new Vector2(0, finalSize);
    }
    public void CloseSelf() 
    {
        CanvasGroup.interactable = false;
        CanvasGroup.DOFade(0, 0.2f).OnComplete(()=> Addressables.Release(gameObject));
    }

    void GenerationComplete() => StartCoroutine(SetElementsOnDisposition());
    IEnumerator SetElementsOnDisposition()
    {
        Layout.enabled = true;
        yield return new WaitForEndOfFrame();
        Layout.enabled = false;
    }
}
