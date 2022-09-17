using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreenSceneLogic : MonoBehaviour
{
    private AddressableAssetPreWarm _addressableAssetPreWarm;
    private ServicesLoader _servicesLoader;

    [SerializeField] private Slider _progressionSlider;

    private void Awake()
    {
        Application.targetFrameRate = 60;

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

        ChangeScene();
    }

    void ChangeScene() => SceneManager.LoadScene(1);
    void UpdateProgressionSlider() => _progressionSlider.value += 1;
}
