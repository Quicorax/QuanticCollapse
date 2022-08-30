using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Test_SpawnPopUp : MonoBehaviour
{
    const string PopUpAdrsKey = "Generic_PopUp";


    public PopUpDisplay PopUp;

    public void SpawnPopUp(int popUpIndex)
    {
        ConfigPopUp(popUpIndex);
    }

    void ConfigPopUp(int index)
    {
        PopUpData popUpData = new();

        if(index == 0) //Rep Cap
        {
            popUpData.SetHeader("You don't have enought:", false);
            popUpData.SetIcon("Reputation");
        }
        else if(index == 1) //Credit Cap
        {
            popUpData.SetHeader("You don't have enought:", false);
            popUpData.SetIcon("AlianceCredits");
        }
        else if (index == 2) //Dilith Cap
        {
            popUpData.SetHeader("You don't have enought:", false);
            popUpData.SetIcon("Dilithium");
            popUpData.SetButton("Go to Shop", TestButtonAction);
        }
        else if(index == 3) //Abandone Mission
        {
            popUpData.SetHeader("Escape", true);
            popUpData.SetButton("Confirm Exit", TestButtonAction);
            popUpData.SetBodyText("You are going to lose the mission progress");
        }
        else if(index == 998)
        {
            popUpData.SetHeader("Epic", true);
            popUpData.SetButton("Epic Button", TestButtonAction);
            popUpData.SetBodyText("This is an Epic PopUp, it has plenty of things!");
            popUpData.SetIcon("Reputation");
        }
        else
        {
        }
        GeneratePopUp(popUpData);
    }


    private List<GameObject> PopUpObjects = new();
    void GeneratePopUp(PopUpData data)
    {
        GameObject element = null;
        Addressables.LoadAssetAsync<GameObject>(PopUpAdrsKey).Completed += handle =>
        {
            element = Addressables.InstantiateAsync(PopUpAdrsKey, transform).Result;
            element.GetComponent<PopUpDisplay>().Initialize(data);
            PopUpObjects.Add(element);
        };

        Invoke(nameof(PopUpDeSpawn), 2f);
    }

    void PopUpDeSpawn()
    {
        Addressables.Release(PopUpObjects[0]);
        PopUpObjects.RemoveAt(0);
    }

    void TestButtonAction()
    {
        Debug.Log("PopUp Button Pressed");
    }
}
