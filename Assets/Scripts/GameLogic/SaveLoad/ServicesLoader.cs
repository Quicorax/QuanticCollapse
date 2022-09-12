using System;
using System.Threading.Tasks;

public class ServicesLoader
{
    const string EnviromentId = "development";

    public async Task LoadSevices(Action updateProgress)
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
        updateProgress();
        await loginService.Initialize();
        updateProgress();
        await remoteConfig.Initialize();
        updateProgress();

        gameConfig.Initialize(remoteConfig);
        gameProgression.Load(gameConfig);
    }
}
