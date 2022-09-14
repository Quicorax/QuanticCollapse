using System;
using System.Threading.Tasks;

public class ServicesLoader
{
    public async Task LoadSevices(Action updateProgress)
    {
        string EnviromentName = "development";

        ServicesInitializer servicesInitializer = new(EnviromentName);

        GameConfigService gameConfig = new();                       //Never changes in Unity
        GameProgressionService gameProgression = new();             //Changes with the user

        RemoteConfigGameService remoteConfig = new();
        LoginGameService loginService = new();
        AnalyticsGameService analyticsService = new();
        StarshipColorsService starshipColorService = new();
        SaveLoadService saveLoadService = new();
        //AdsGameService adsService = new AdsGameService("#######", "Rewarded_Android");

        ServiceLocator.RegisterService(gameConfig);
        ServiceLocator.RegisterService(gameProgression);
        ServiceLocator.RegisterService(remoteConfig);
        ServiceLocator.RegisterService(loginService);
        ServiceLocator.RegisterService(analyticsService);
        ServiceLocator.RegisterService(starshipColorService);
        ServiceLocator.RegisterService(saveLoadService);
        //ServiceLocator.RegisterService(adsService);

        await servicesInitializer.Initialize();
        updateProgress();
        await loginService.Initialize();
        updateProgress();
        await remoteConfig.Initialize();
        updateProgress();
        await analyticsService.Initialize();
        updateProgress();

        gameConfig.Initialize(remoteConfig);
        gameProgression.Initialize(saveLoadService);
        starshipColorService.Initialize();
        saveLoadService.Initialize(gameConfig, gameProgression);
        //adsService.Initialize(Application.isEditor);
    }
}