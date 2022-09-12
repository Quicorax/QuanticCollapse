using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class SplashScreenSceneLogic : MonoBehaviour
{
    private AddressableAssetPreWarm _addressableAssetPreWarm;
    private ServicesLoader _servicesLoader;

    [SerializeField] private Slider _progressionSlider;
    private float finalValue;

    private void Awake()
    {
        _servicesLoader = new();
        _addressableAssetPreWarm = new();
    }
    void Start()
    {
        Initialize().ContinueWith(task => Debug.LogException(task.Exception), TaskContinuationOptions.OnlyOnFaulted);
    }

    async Task Initialize()
    {
        await _servicesLoader.LoadSevices(UpdateProgressionSlider);
        await _addressableAssetPreWarm.PreWarmElements(UpdateProgressionSlider);

        //Debug.Log(finalValue);
        ChangeScene();
    }

    void ChangeScene() => SceneManager.LoadScene(1);
    void UpdateProgressionSlider() => _progressionSlider.value += 0.1f;
}
