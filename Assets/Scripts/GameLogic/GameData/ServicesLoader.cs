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
        AdsGameService adsService = new();
        IAPGameService iapService = new();
        SaveLoadService saveLoadService = new();
        IGameProgressionProvider gameProgressionProvider = new GameProgressionProvider();

        StarshipVisualsService starshipVisualService = new();
        AddressablesService addressablesService = new();
        PopUpService popUpService = new();

        ServiceLocator.RegisterService(gameConfig);
        ServiceLocator.RegisterService(gameProgression);
        ServiceLocator.RegisterService(remoteConfig);
        ServiceLocator.RegisterService(loginService);
        ServiceLocator.RegisterService(analyticsService);
        ServiceLocator.RegisterService(adsService);
        ServiceLocator.RegisterService(iapService);
        ServiceLocator.RegisterService(saveLoadService);
        ServiceLocator.RegisterService(starshipVisualService);
        ServiceLocator.RegisterService(addressablesService);
        ServiceLocator.RegisterService(popUpService);

        await servicesInitializer.Initialize();
        updateProgress();

        await loginService.Initialize();
        updateProgress();
        await remoteConfig.Initialize();
        updateProgress();
        await analyticsService.Initialize();
        updateProgress();
        await adsService.Initialize(analyticsService, Application.isEditor);
        updateProgress();
        await iapService.Initialize();
        updateProgress();
        await gameProgressionProvider.Initialize();
        updateProgress(); //TODO: Ad to slider

        gameConfig.Initialize(remoteConfig);
        gameProgression.Initialize(saveLoadService);
        saveLoadService.Initialize(gameConfig, gameProgression, gameProgressionProvider);
        popUpService.Initialize(addressablesService);
        starshipVisualService.Initialize();
    }
}