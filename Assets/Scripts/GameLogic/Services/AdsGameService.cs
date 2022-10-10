using System.Threading.Tasks;
using UnityEngine.Advertisements;
public class AdsGameService : IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener, IService
{
    private string _adsGameId;
    private string _adUnitId;
    private bool _isAdLoaded = false;

    private AnalyticsGameService _analytics;
    public bool IsAdReady => _isInitialized && _isAdLoaded;
    private bool _isInitialized;

    private TaskStatus _watchAdTask = TaskStatus.Created;
    public AdsGameService()    
    {
        _adsGameId = "4928649";
        _adUnitId = "Rewarded_Android";
    }

    public void Initialize(AnalyticsGameService analytics, bool testMode = false)
    {
        _analytics = analytics;

        Advertisement.Initialize(_adsGameId, testMode, this);

        _isInitialized = true;
    }

    public void OnInitializationComplete() => LoadAd();

    public void OnInitializationFailed(UnityAdsInitializationError error, string message) => _isInitialized = false;

    public void LoadAd() 
    {
        _isAdLoaded = false;
        Advertisement.Load(_adUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string adUnitId) =>_isAdLoaded = true; 
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message) => Advertisement.Load(_adUnitId, this);

    public async Task<bool> ShowAd()
    {
        if (_watchAdTask == TaskStatus.Running)
            return false;

        if (!_isInitialized)
            return false;

        if (!IsAdReady)
            return false;

        _watchAdTask = TaskStatus.Running;

        Advertisement.Show(_adUnitId, this);

#if UNITY_EDITOR
        await Task.Delay(2000);
        OnUnityAdsShowComplete(_adUnitId, UnityAdsShowCompletionState.COMPLETED);
#endif
        while (_watchAdTask == TaskStatus.Running)
            await Task.Delay(2000);

        if(_watchAdTask == TaskStatus.RanToCompletion)
        {
            _analytics.SendEvent("rewardedAd_completed");
            return true;
        }

        return false;
    }


    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        Advertisement.Load(_adUnitId, this);
        _watchAdTask = showCompletionState == UnityAdsShowCompletionState.COMPLETED ? TaskStatus.RanToCompletion : TaskStatus.Faulted;
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Advertisement.Load(_adUnitId, this);
        _watchAdTask = TaskStatus.Faulted;
    }
    #region Analitics
    public void OnUnityAdsShowStart(string adUnitId) => _analytics.SendEvent("rewardedAd_start");

    public void OnUnityAdsShowClick(string adUnitId) => _analytics.SendEvent("rewardedAd_userClicked");
    #endregion
    public void Clear() { }
}