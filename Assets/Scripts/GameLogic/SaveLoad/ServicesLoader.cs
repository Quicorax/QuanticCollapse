using System;
using System.Threading.Tasks;
using UnityEngine;

public class ServicesLoader
{
    public async Task LoadSevices(Action updateProgress)
    {
        string EnviromentName = "development";

        ServicesInitializer servicesInitializer = new(EnviromentName);

        GameConfigService gameConfig = new GameConfigService();                     //Never changes in Unity
        GameProgressionService gameProgression = new GameProgressionService();      //Changes with the user

        RemoteConfigGameService remoteConfig = new RemoteConfigGameService();
        LoginGameService loginService = new LoginGameService();
        AnalyticsGameService analyticsService = new AnalyticsGameService();
        //AdsGameService adsService = new AdsGameService("#######", "Rewarded_Android");

        ServiceLocator.RegisterService(gameConfig);
        ServiceLocator.RegisterService(gameProgression);
        ServiceLocator.RegisterService(remoteConfig);
        ServiceLocator.RegisterService(loginService);
        ServiceLocator.RegisterService(analyticsService);
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
        gameProgression.Initialize(gameConfig);
        //adsService.Initialize(Application.isEditor);
    }
}
