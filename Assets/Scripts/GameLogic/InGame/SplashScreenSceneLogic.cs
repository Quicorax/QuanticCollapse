using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace QuanticCollapse
{
    public class SplashScreenSceneLogic : MonoBehaviour
    {
        private ServicesLoader _servicesLoader;

        [SerializeField] private Slider _progressionSlider;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            _servicesLoader = new();
        }

        private void Start()
        {
            Initialize().ManageTaskException();
        }

        private async Task Initialize()
        {
            await _servicesLoader.LoadSevices(UpdateProgressionSlider);

            ChangeScene();
        }

        private void ChangeScene() => SceneManager.LoadScene(1);
        private void UpdateProgressionSlider() => _progressionSlider.value += 1;
    }
}