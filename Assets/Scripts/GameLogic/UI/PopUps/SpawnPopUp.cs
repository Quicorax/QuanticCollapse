using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SpawnPopUp
{
    const string PopUpAdrsKey = "Generic_PopUp";

    private Transform _parent;
    private List<GameObject> PopUpObjects = new();
    public SpawnPopUp(Transform parent) 
    {
        _parent = parent;
    }

    public void SimpleGeneratePopUp(string popUpConcept, Action onButtonAction = null) => GeneratePopUp(ConfigPopUp(popUpConcept, onButtonAction));

    PopUpData ConfigPopUp(string popUpConcept, Action onButtonAction = null)
    {
        PopUpData popUpData = new();

        if(popUpConcept == "Reputation") 
        {
            popUpData.SetHeader("You don't have enought:", false);
            popUpData.SetIcon("Reputation");
        }

        if(popUpConcept == "AlianceCredits") 
        {
            popUpData.SetHeader("You don't have enought:", false);
            popUpData.SetIcon("AlianceCredits");
        }

        if (popUpConcept == "Dilithium")
        {
            popUpData.SetHeader("You don't have enought:", false);
            popUpData.SetIcon("Dilithium");
            popUpData.SetButton("Buy some!", onButtonAction);
        }

        if(popUpConcept == "EscapeMission")
        {
            popUpData.SetHeader("Escape", true);
            popUpData.SetButton("Confirm Exit", onButtonAction);
            popUpData.SetBodyText("You will lose the mission progress");
        }

        return popUpData;
    }

    public void GeneratePopUp(PopUpData data, bool fade = true)
    {
        GameObject element = null;

        data.onCloseButtonClickedAction = PopUpDeSpawn;

        Addressables.LoadAssetAsync<GameObject>(PopUpAdrsKey).Completed += handle =>
        {
            element = Addressables.InstantiateAsync(PopUpAdrsKey, _parent).Result;
            element.GetComponent<PopUpDisplay>().Initialize(data);
            PopUpObjects.Add(element);

            element.GetComponent<CanvasGroup>().alpha = 1;
            element.transform.DOPunchScale(Vector3.one * 0.1f, .5f);

            if (fade)
            {
                element.GetComponent<CanvasGroup>().DOFade(0, 2f).SetEase(Ease.InCirc).OnComplete(() =>
                {
                    PopUpDeSpawn();
                });
            }
        };
    }

    public void PopUpDeSpawn()
    {
        Addressables.Release(PopUpObjects[0]);
        PopUpObjects.RemoveAt(0);
    }
}
