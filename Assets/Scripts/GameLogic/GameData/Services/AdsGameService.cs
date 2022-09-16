using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsGameService : IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener, IService
{
    private string _adsGameId;
    private string _adUnitId;

    private AnalyticsGameService _analytics;
    public bool IsAdReady => IsInitialized && Advertisement.IsReady(_adUnitId);
    public bool IsInitialized => _initializationTask == TaskStatus.RanToCompletion;

    private TaskStatus _initializationTask = TaskStatus.Created;
    private TaskStatus _watchAdTask = TaskStatus.Created;
    public AdsGameService(string adsGameId, string adUnitId, AnalyticsGameService analytics)    
    {
        _adsGameId = adsGameId;
        _adUnitId = adUnitId;

        _analytics = analytics;
    }

    public async Task<bool> Initialize(bool testMode = false)
    {
        _initializationTask = TaskStatus.Running;
        Advertisement.Initialize(_adsGameId, testMode, true, this);

        while (_initializationTask == TaskStatus.Running)
            await Task.Delay(500);

        return IsInitialized;
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        LoadAd();
        _initializationTask = TaskStatus.RanToCompletion;
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error} - {message}");
        _initializationTask = TaskStatus.Faulted;
    }

    public void LoadAd()
    {
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);
    }
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error} - {message}");
        Advertisement.Load(_adUnitId, this);
    }

    public async Task<bool> ShowAd()
    {

        if (_watchAdTask == TaskStatus.Running)
            return false;

        if (!IsInitialized)
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
        Debug.Log("Unity Ads Rewarded Ad:" + showCompletionState);
        Advertisement.Load(_adUnitId, this);
        _watchAdTask = showCompletionState == UnityAdsShowCompletionState.COMPLETED ? TaskStatus.RanToCompletion : TaskStatus.Faulted;
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error} - {message}");
        Advertisement.Load(_adUnitId, this);
        _watchAdTask = TaskStatus.Faulted;
    }
    #region Analitics
    public void OnUnityAdsShowStart(string adUnitId)
    {
        _analytics.SendEvent("rewardedAd_start");
        Debug.Log("Started watching an ad");
    }

    public void OnUnityAdsShowClick(string adUnitId)
    {
        _analytics.SendEvent("rewardedAd_userClicked");
        Debug.Log("User clicked in the ad");
    }
    #endregion
    public void Clear() { }
}