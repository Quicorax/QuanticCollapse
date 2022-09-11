using System.Threading.Tasks;
using UnityEngine;

public partial class LoadSceneLogic : MonoBehaviour
{
    const string EnviromentId = "development";

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        LoadSevices().ContinueWith(task => Debug.LogException(task.Exception),TaskContinuationOptions.OnlyOnFaulted);
    }

    private async Task LoadSevices()
    {
        ServicesInitializer servicesInitializer = new(EnviromentId);

        GameConfigService gameConfig = new GameConfigService();
        GameProgressionService gameProgression = new GameProgressionService();
        RemoteConfigGameService remoteConfig = new RemoteConfigGameService();
        LoginGameService loginService = new LoginGameService();

        ServiceLocator.RegisterService(gameConfig);
        ServiceLocator.RegisterService(gameProgression);
        ServiceLocator.RegisterService(remoteConfig);
        ServiceLocator.RegisterService(loginService);

        await servicesInitializer.Initialize();
        await loginService.Initialize();
        await remoteConfig.Initialize();
        gameConfig.Initialize(remoteConfig);
        gameProgression.Load(gameConfig);
    }
}
