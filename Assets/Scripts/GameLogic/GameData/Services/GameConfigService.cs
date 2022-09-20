using System.Collections.Generic;

public class GameConfigService : IService
{
    public int PlayerInitialAllianceCredits { get; private set; }
    public int PlayerInitialDilithium  { get; private set; }
    public int PlayerInitialDeAthomizerBooster { get; private set; }
    public int PlayerInitialEasyTriggerBooster  { get; private set; }
    public int PlayerInitialFistAidKitBooster  { get; private set; }

    public int AllianceCreditsPerRewardedAd { get; private set; }
    public List<IAPBundle> AllianceCreditsPerIAP { get; private set; }
    public int ExternalBoosterPerRewardedAd { get; private set; }

    public string PlayerInitialStarshipModel  { get; private set; }
    public string PlayerInitialStarshipColors { get; private set; }

    public void Initialize(RemoteConfigGameService dataProvider)
    {
        PlayerInitialAllianceCredits = dataProvider.Get(Constants.InitialAllianceCredits, 0);
        PlayerInitialDilithium = dataProvider.Get(Constants.InitialDilithium, 0);
        PlayerInitialDeAthomizerBooster = dataProvider.Get(Constants.InitialDeAthomizer, 0);
        PlayerInitialEasyTriggerBooster = dataProvider.Get(Constants.InitialEasyTrigger, 0);
        PlayerInitialFistAidKitBooster = dataProvider.Get(Constants.InitialFirstAidKit, 0);
        AllianceCreditsPerRewardedAd = dataProvider.Get(Constants.AllianceCreditsPerAd, 0);
        ExternalBoosterPerRewardedAd = dataProvider.Get(Constants.ExternalBoosterPerAd, 0);
        PlayerInitialStarshipModel = dataProvider.Get(Constants.InitialStarshipModel, Constants.Empty);
        PlayerInitialStarshipColors = dataProvider.Get(Constants.InitialStarshipColors, Constants.Empty);

        AllianceCreditsPerIAP = dataProvider.Get(Constants.IAPProducts, new List<IAPBundle>());
    }

    public void Clear() { }
}
