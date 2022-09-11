using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class LoadingCanvas : MonoBehaviour
{
    const string PopUpObjectAdrsKey = "Modular_PopUp";
    const string Empty = "";
    const string AlianceCredits = "AlianceCredits";

    [SerializeField] private Transform _popUpParent;

    private CanvasGroup _canvasGroup;
    private Transform _iconTransform;

    private bool pause;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _iconTransform = transform.GetChild(0);
    }
    private void Start()
    {
        IconInitMovement();

        PreWarmPopUpAsset();
    }
    public void FadeCanvas(bool fade)
    {
        _canvasGroup.DOFade(fade ? 0 : 1, 0.5f);

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

        _iconTransform.DORotate(Vector2.up * 360, 1f, RotateMode.LocalAxisAdd)
            .OnComplete(() => IconRotate());
    }

    void PreWarmPopUpAsset()
    {
        List<PopUpComponentData> Modules = new()
        {
            new HeaderPopUpComponentData(Empty, false),
            new TextPopUpComponentData(Empty),
            new ImagePopUpComponentData(AlianceCredits, Empty),
            new ImagePopUpComponentData(AlianceCredits, Empty),
            new ImagePopUpComponentData(AlianceCredits, Empty),
            new PricePopUpComponentData(Empty),
            new ButtonPopUpComponentData(Empty, null, true),
            new CloseButtonPopUpComponentData()
        };
        Addressables.LoadAssetAsync<GameObject>(PopUpObjectAdrsKey).Completed += handle =>
        {
           Addressables.InstantiateAsync(PopUpObjectAdrsKey, _popUpParent)
           .Result.GetComponent<ModularPopUp>()
           .GeneratePopUp(Modules, (x)=> Addressables.Release(x));
        };
    }
}
