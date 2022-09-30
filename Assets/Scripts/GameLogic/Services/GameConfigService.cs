using System.Collections.Generic;

public class GameConfigService : IService
{
    public List<ResourceElement> Resources { get; private set; }
    public List<IAPBundle> IAPProducts { get; private set; }

    public string PlayerInitialStarshipModel  { get; private set; }
    public List<SkinColors> PlayerInitialStarshipColors { get; private set; }

    public int AllianceCreditsPerRewardedAd { get; private set; }
    public int ExternalBoosterPerRewardedAd { get; private set; }

    public void Initialize(RemoteConfigGameService dataProvider)
    {
        Resources = dataProvider.Get("InitialResources", new List<ResourceElement>());

        IAPProducts = dataProvider.Get("IAPProducts", new List<IAPBundle>());

        PlayerInitialStarshipModel = dataProvider.Get("PlayerInitialStarshipModel", "");
        PlayerInitialStarshipColors = dataProvider.Get("PlayerInitialStarshipColors", new List<SkinColors>());

        AllianceCreditsPerRewardedAd = dataProvider.Get("AllianceCreditsPerRewardedAd", 0);
        ExternalBoosterPerRewardedAd = dataProvider.Get("ExternalBoosterPerRewardedAd", 0);
    }

    public void Clear() { }
}
