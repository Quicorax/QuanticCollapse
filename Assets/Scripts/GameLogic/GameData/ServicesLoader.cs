using System;
using System.Threading.Tasks;
using UnityEngine;

public class ServicesLoader
{
    [SerializeField]
    private bool IsDevBuild = true;
    public async Task LoadSevices(Action updateProgress)
    {
        string enviromentName = IsDevBuild ? Constants.Development : Constants.Production;

        ServicesInitializer servicesInitializer = new(enviromentName);

        GameConfigService gameConfig = new();
        GameProgressionService gameProgression = new();
        RemoteConfigGameService remoteConfig = new();
        LoginGameService loginService = new();
        AnalyticsGameService analyticsService = new();

        StarshipVisualsService starshipVisualService = new();
        SaveLoadService saveLoadService = new();
        AdsGameService adsService = new(Constants.AdsGameId, Constants.RewardedAndroid, analyticsService);

        ServiceLocator.RegisterService(gameConfig);
        ServiceLocator.RegisterService(gameProgression);
        ServiceLocator.RegisterService(remoteConfig);
        ServiceLocator.RegisterService(loginService);
        ServiceLocator.RegisterService(analyticsService);
        ServiceLocator.RegisterService(starshipVisualService);
        ServiceLocator.RegisterService(saveLoadService);
        ServiceLocator.RegisterService(adsService);

        await servicesInitializer.Initialize();
        updateProgress();
        await loginService.Initialize();
        updateProgress();
        await remoteConfig.Initialize();
        updateProgress();
        await analyticsService.Initialize();
        updateProgress();
        await adsService.Initialize(Application.isEditor);
        updateProgress();

        gameConfig.Initialize(remoteConfig);
        gameProgression.Initialize(saveLoadService);
        starshipVisualService.Initialize();
        saveLoadService.Initialize(gameConfig, gameProgression);
    }
}