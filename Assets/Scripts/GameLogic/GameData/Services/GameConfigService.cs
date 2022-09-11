public class GameConfigService : IService
{
    public int PlayerInitialAlianceCredits = 0;
    public int PlayerInitialDeAthomizerBooster = 0;
    public int PlayerInitialDilithium = 0;
    public int PlayerInitialEasyTriggerBooster = 0;
    public int PlayerInitialFistAidKitBooster = 0;
    public string PlayerInitialStarshipColors = "";
    public string PlayerInitialStarshipModel = "";

    public void Initialize(RemoteConfigGameService dataProvider)
    {
        PlayerInitialAlianceCredits = dataProvider.Get("PlayerInitialAlianceCredits", 0);
        PlayerInitialDeAthomizerBooster = dataProvider.Get("PlayerInitialDeAthomizerBooster", 0);
        PlayerInitialDilithium = dataProvider.Get("PlayerInitialDilithium", 0);
        PlayerInitialEasyTriggerBooster = dataProvider.Get("PlayerInitialEasyTriggerBooster", 0);
        PlayerInitialFistAidKitBooster = dataProvider.Get("PlayerInitialFistAidKitBooster", 0);
        PlayerInitialStarshipColors = dataProvider.Get("PlayerInitialStarshipColors", "");
        PlayerInitialStarshipModel = dataProvider.Get("PlayerInitialStarshipModel", ""); 
    }

    public void Clear() { }
}
