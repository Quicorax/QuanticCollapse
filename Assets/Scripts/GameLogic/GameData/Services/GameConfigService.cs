using System.Collections.Generic;

public class GameConfigService : IService
{
    public int PlayerInitialAlianceCredits { get; private set; }
    public int PlayerInitialDilithium  { get; private set; }
    public int PlayerInitialDeAthomizerBooster { get; private set; }
    public int PlayerInitialEasyTriggerBooster  { get; private set; }
    public int PlayerInitialFistAidKitBooster  { get; private set; }
    public string PlayerInitialStarshipModel  { get; private set; }
    public List<string> PlayerInitialStarshipColors { get; private set; }

    public void Initialize(RemoteConfigGameService dataProvider)
    {
        PlayerInitialAlianceCredits = dataProvider.Get("PlayerInitialAlianceCredits", 0);
        PlayerInitialDilithium = dataProvider.Get("PlayerInitialDilithium", 0);
        PlayerInitialDeAthomizerBooster = dataProvider.Get("PlayerInitialDeAthomizerBooster", 0);
        PlayerInitialEasyTriggerBooster = dataProvider.Get("PlayerInitialEasyTriggerBooster", 0);
        PlayerInitialFistAidKitBooster = dataProvider.Get("PlayerInitialFistAidKitBooster", 0);
        PlayerInitialStarshipModel = dataProvider.Get("PlayerInitialStarshipModel", "");
        
        PlayerInitialStarshipColors = dataProvider.Get("PlayerInitialStarshipColors", new List<string>());
    }

    public void Clear() { }
}
