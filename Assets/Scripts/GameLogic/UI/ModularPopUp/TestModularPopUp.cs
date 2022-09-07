using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TestModularPopUp : MonoBehaviour
{
    const string PopUpObjectAdrsKey = "Modular_PopUp";

    private GameObject GeneratedPopUp;
    public void SpawnPopUp()
    {
        //Add Modules
        List<PopUpComponentData> Modules = new()
        {
            new HeaderPopUpComponentData("FirstAidKit!", true),
            new TextPopUpComponentData("Used to heal your starship on combat"),
            new ImagePopUpComponentData("FirstAidKit", "x3"),
            new ImagePopUpComponentData("EasyTrigger", "x5"),
            new ImagePopUpComponentData("DeAthomizer", "x17"),
            new PricePopUpComponentData("15"),
            new ButtonPopUpComponentData("Buy", TestButtonAction),
            new CloseButtonPopUpComponentData(ClosePopUp)
        };

        //Generate PopUp Object and set up Logic
        Addressables.LoadAssetAsync<GameObject>(PopUpObjectAdrsKey).Completed += handle =>
        {
            GeneratedPopUp = Addressables.InstantiateAsync(PopUpObjectAdrsKey, transform).Result;
            GeneratedPopUp.GetComponent<ModularPopUp>().GeneratePopUp(Modules);
        };
    }

    void TestButtonAction()
    {
        Debug.Log("TestButtonAction succed");
        ClosePopUp();
    }

    void ClosePopUp()
    {
        Debug.Log("Close PopUP");
        Addressables.Release(GeneratedPopUp);
    }
}