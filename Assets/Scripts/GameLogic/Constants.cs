
public static class Constants
{
    //Ingame Resources
    public const string Reputation = "Reputation";
    public const string AllianceCredits = "AllianceCredits";
    public const string Dilithium = "Dilithium";
    public const string FirstAidKit = "FirstAidKit";
    public const string EasyTrigger = "EasyTrigger";
    public const string DeAthomizer = "DeAthomizer";

    //Initial Ingame Resources
    public const string InitialAllianceCredits = "PlayerInitialAllianceCredits";
    public const string InitialDilithium = "PlayerInitialDilithium";
    public const string InitialFirstAidKit = "PlayerInitialFistAidKitBooster";
    public const string InitialEasyTrigger = "PlayerInitialEasyTriggerBooster";
    public const string InitialDeAthomizer = "PlayerInitialDeAthomizerBooster";
    public const string AllianceCreditsPerAd = "AllianceCreditsPerRewardedAd";
    public const string ExternalBoosterPerAd = "ExternalBoosterPerRewardedAd";
    public const string InitialStarshipModel = "PlayerInitialStarshipModel";
    public const string InitialStarshipColors = "PlayerInitialStarshipColors";
    public const string RewardedAdCompleted = "rewardedAd_completed";
    public const string RewardedAdStart = "rewardedAd_start";
    public const string RewardedAdUserClicked = "rewardedAd_userClicked";

    //Analytic Events
    public const string LevelStart = "level_start";
    public const string LevelLose = "level_lose";
    public const string LevelWin = "level_win";
    public const string LevelIndex = "level_index";

    //Addressable Keys
    public const string ModularPopUp = "Modular_PopUp";
    public const string PopUpModule = "Module_";
    public const string StarshipTPVModel = "StarshipPrefab_";
    public const string StarshipFPVModel = "FPV_Starship_";
    public const string Booster = "ExternalBoosterElement_ViewObject";
    public const string Level = "LevelMissionElement_ViewObject";
    public const string ShopProduct = "ProductSample";
    public const string ShopSection = "ProductSection";
    public const string StarshipColor = "StarshipColorPack";
    public const string StarshipGeo = "StarshipGeo";

    //Shader elements
    public const string AimSightColor = "_AimSightColor";
    public const string AimCenterY = "_Aim_Center_Y";
    public const string GeneralAlpha = "_GeneralAlpha";
    public const string ScreenFresnelColor = "_Color";
    public const string SpaceGeneralColor = "_SpaceGeneralColor";
    public const string SpaceRelativeMovementSpeed = "_SpaceRelativeMovementSpeed";

    //Shop
    public const string ShopElements = "ShopElements";
    public const string Buy = "Buy";
    public const string EmptyResource = "You don't have enought:";
    public const string IAPProducts = "IAPProducts";

    //Navigation
    public const string DefaultInput = "Fire1";
    public const string LobbyScene = "02_Lobby_Scene";
    public const string GamePlayScene = "03_GamePlay_Scene";
    public const string Mission = "Mission ";
    public const string Levels = "Levels";
    public const string LevelViewName = "LevelView_";
    public const string InitialDispositionPath = "LevelDispositionData/Level_";
    public const string RandomInitialDispositionPath = "LevelDispositionData/Level_Random";
    public const string Data = "data";

    //Starship Visuals
    public const string RookName = "Rook";
    public const string RhynoName = "Rhyno";
    public const string PrimaryColor = "_PrimaryColor";
    public const string SecondaryColor = "_SecondaryColor";
    public const string SignatureColor = "_SignatureColor";
    public const string AttackParticle = "AttParticle";
    public const string StarshipColors = "StarshipColors";
    public const string EquipedStarshipModel = "EquipedStarshipModel";
    public const string EquipedStarshipColors = "EquipedStarshipColors";

    //Icons
    public const string Empty = "";
    public const string X = "x";
    public const string MiddleBar = "-";
    public const string BottomBar = "_";

    //Services
    public const string Development = "development";
    public const string Production = "production";
    public const string AdsGameId = "4928649";
    public const string RewardedAndroid = "Rewarded_Android";
    public const string SaveFileName = "/_gameProgression.json";

    //Modular PopUp Component Keys
    public const string Header = "Header";
    public const string Text = "Text";
    public const string Price = "Price";
    public const string Image = "Image";
    public const string Button = "Button";
    public const string CloseButton = "CloseButton";

    //Empty booster PopUp
    public const string WatchAdd = "Watch an ad";
    public const string VideoIcon = "VideoIcon";
    public const string HangarEmpty = "Your hangar is empty! \n Watch to purchase!";

    //Failed Level PopUp
    public const string MissionFailed = "Mission Failed";
    public const string MissionFailedLog = "Your ship was disabled";
    public const string AbandoneMission = "Abandone Missio";
    public const string RepeatMission = "Repeat Missio";
    public const string SkullIcon = "Skull";

    //Completed Level PopUp
    public const string MissionCompleted = "Mission Completed";
    public const string MissionCompletedLog = "Rewards:";
    public const string Continue = "Continue";

    //Escape PopUp
    public const string Escape = "Escape";
    public const string EscapeLog = "You will lose the mission progress";
    public const string ConfirmEscape = "Confirm Exit";

    //Credits PopUp
    public const string Credits = "CREDITS";
    public const string CreditsSelf = "<size=150%>Developed by: <b>Quicorax</b>";
    public const string CreditsLog = "<align=\"left\"><indent=5%><i>Quantic Collapse</i> contais assets made by:";
    public const string Kenney = "<b>Kenney</b>: \n UI Elements";
    public const string Quaternius = "<b>Quaternius</b>: \n Starship Raw Models";
    public const string Iconian = "<b>Iconian Fonts</b>: \n Space Ranger Font";

    //IAP failed PopUp
    public const string IAPFailed = "Something went wrong, true";
    public const string IAPFailedLog = "Purchase cancelled";

    //Delete Files PopUp
    public const string DeleteFiles = "DELETE LOCAL FILES";
    public const string DeleteFilesLog = "Are you shure you want to delete your local files?";
    public const string DeleteFilesDetail = "The following local files will be deleted:";
    public const string DeleteFilesSpecific = "<align=\"left\"><indent=5%><b>Game Progression</b> \n <indent=5%><b>Game Setting</b>";
    public const string ConfirmDeleteFiles= "Close game and delete";
}
