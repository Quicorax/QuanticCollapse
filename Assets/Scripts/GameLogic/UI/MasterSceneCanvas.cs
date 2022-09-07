using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MasterSceneCanvas : MonoBehaviour
{
    const string PopUpObjectAdrsKey = "Modular_PopUp";
    const string Empty = "";
    const string AlianceCredits = "AlianceCredits";

    [SerializeField] private Transform transparentParent;

    private CanvasGroup CanvasGroup;
    private Transform IconTransform;
    private GameObject provPopUp;

    private bool pause;

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
        IconTransform = transform.GetChild(0);
    }
    private void OnDisable()
    {
        ReleaseAssetWarmedPopUp();
    }
    private void Start()
    {
        IconInitMovement();

        PreWarmPopUpAsset();
    }
    public void FadeCanvas(bool fade)
    {
        CanvasGroup.DOFade(fade ? 0 : 1, 0.5f);

        if (fade)
            IconPauseRotation();
        else
            IconInitMovement();
    }
    void IconInitMovement()
    {
        pause = false;
        IconRotate();
    }
    void IconPauseRotation()
    {
        pause = true;
    }
    void IconRotate()
    {
        if (pause)
            return;

        IconTransform.DORotate(Vector2.up * 360, 1f, RotateMode.LocalAxisAdd)
            .OnComplete(() => IconRotate());
    }

    void PreWarmPopUpAsset()
    {
        //Add Modules
        List<PopUpComponentData> Modules = new()
        {
            new HeaderPopUpComponentData(Empty, false),
            new TextPopUpComponentData(Empty),
            new ImagePopUpComponentData(AlianceCredits, Empty),
            new ImagePopUpComponentData(AlianceCredits, Empty),
            new ImagePopUpComponentData(AlianceCredits, Empty),
            new PricePopUpComponentData(Empty),
            new ButtonPopUpComponentData(Empty, null),
            new CloseButtonPopUpComponentData(null)
        };

        //Generate PopUp Object and set up Logic
        Addressables.LoadAssetAsync<GameObject>(PopUpObjectAdrsKey).Completed += handle =>
        {
            provPopUp = Addressables.InstantiateAsync(PopUpObjectAdrsKey, transparentParent).Result;
            provPopUp.GetComponent<ModularPopUp>().GeneratePopUp(Modules);
        };
            
    }
    void ReleaseAssetWarmedPopUp()
    {
        Addressables.Release(provPopUp);
    }
}
