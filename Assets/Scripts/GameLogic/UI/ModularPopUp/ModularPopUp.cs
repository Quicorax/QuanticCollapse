using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class ModularPopUp : MonoBehaviour
{
    const string PopUpModuleAdrsKey = "Module_";

    private int initialPanelOffset = 35;
    [SerializeField] private RectTransform Body;
    [SerializeField] private VerticalLayoutGroup Layout;

    public void GeneratePopUp(List<PopUpComponentData> ModulesToAdd)
    {
        int spawnedModules = 0;

        int finalSize = initialPanelOffset;
        foreach (var moduleData in ModulesToAdd)
        {
            finalSize += moduleData.ModuleHeight + 20;

            string adressableKey = PopUpModuleAdrsKey + moduleData.ModuleConcept;
            Addressables.LoadAssetAsync<GameObject>(adressableKey).Completed += handle =>
            {
                Addressables.InstantiateAsync(adressableKey, Body).Result.GetComponent<PopUpComponentObject>().SetData(moduleData);
                spawnedModules++;

                if (spawnedModules == ModulesToAdd.Count)
                    StartCoroutine(SetElementsOnDisposition());
            };
        }

        Body.sizeDelta += new Vector2(0, finalSize);
    }

    IEnumerator SetElementsOnDisposition()
    {
        Layout.enabled = true;
        yield return new WaitForEndOfFrame();
        Layout.enabled = false;
    }
}
