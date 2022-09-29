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
        PlayerInitialAllianceCredits = dataProvider.Get("PlayerInitialAllianceCredits", 0);
        PlayerInitialDilithium = dataProvider.Get("PlayerInitialDilithium", 0);
        PlayerInitialDeAthomizerBooster = dataProvider.Get("PlayerInitialDeAthomizerBooster", 0);
        PlayerInitialEasyTriggerBooster = dataProvider.Get("PlayerInitialEasyTriggerBooster", 0);
        PlayerInitialFistAidKitBooster = dataProvider.Get("PlayerInitialFistAidKitBooster", 0);
        AllianceCreditsPerRewardedAd = dataProvider.Get("AllianceCreditsPerRewardedAd", 0);
        ExternalBoosterPerRewardedAd = dataProvider.Get("ExternalBoosterPerRewardedAd", 0);
        PlayerInitialStarshipModel = dataProvider.Get("PlayerInitialStarshipModel", "");
        PlayerInitialStarshipColors = dataProvider.Get("PlayerInitialStarshipColors", "");

        AllianceCreditsPerIAP = dataProvider.Get("IAPProducts", new List<IAPBundle>());
    }

    public void Clear() { }
}
