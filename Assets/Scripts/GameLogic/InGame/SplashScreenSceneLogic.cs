using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreenSceneLogic : MonoBehaviour
{
    private ServicesLoader _servicesLoader;

    [SerializeField] private Slider _progressionSlider;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        _servicesLoader = new();
    }
    void Start()
    {
        Initialize().ManageTaskExeption();
    }

    async Task Initialize()
    {
        await _servicesLoader.LoadSevices(UpdateProgressionSlider);

        ChangeScene();
    }

    void ChangeScene() => SceneManager.LoadScene(1);
    void UpdateProgressionSlider() => _progressionSlider.value += 1;
}
