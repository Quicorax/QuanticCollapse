using System.Collections.Generic;

public class GameConfigService : IService
{
    public List<ResourceElement> Resources { get; private set; }
    public List<IAPBundle> AllianceCreditsPerIAP { get; private set; }
    public int AllianceCreditsPerRewardedAd { get; private set; }
    public int ExternalBoosterPerRewardedAd { get; private set; }

    public string PlayerInitialStarshipModel  { get; private set; }
    public string PlayerInitialStarshipColors { get; private set; }

    public void Initialize(RemoteConfigGameService dataProvider)
    {
        Resources = dataProvider.Get("InitialResources", new List<ResourceElement>());

        AllianceCreditsPerIAP = dataProvider.Get("IAPProducts", new List<IAPBundle>());

        AllianceCreditsPerRewardedAd = dataProvider.Get("AllianceCreditsPerRewardedAd", 0);
        ExternalBoosterPerRewardedAd = dataProvider.Get("ExternalBoosterPerRewardedAd", 0);

        PlayerInitialStarshipModel = dataProvider.Get("PlayerInitialStarshipModel", "");
        PlayerInitialStarshipColors = dataProvider.Get("PlayerInitialStarshipColors", "");
    }

    public void Clear() { }
}
