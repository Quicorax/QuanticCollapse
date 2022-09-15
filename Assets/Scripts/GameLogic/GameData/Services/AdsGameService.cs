using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsGameService : IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener, IService
{
    private string _adsGameId;
    private string _adUnitId;

    private TaskStatus _initializationTask = TaskStatus.Created;
    private TaskStatus _watchAdvTask = TaskStatus.Created;
    public bool IsAdReady => Advertisement.IsReady(_adUnitId);

    private bool _isInitialized;
    public bool IsInitialized => _isInitialized;

    public AdsGameService(string adsGameId, string adUnitId)
    {
        _adsGameId = adsGameId;
        _adUnitId = adUnitId;
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
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
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

    public async Task<bool> ShowAd()
    {
        _watchAdvTask = TaskStatus.Created;

        Advertisement.Show(_adUnitId, this);
#if UNITY_EDITOR
        await Task.Delay(2000);
        OnUnityAdsShowComplete(_adUnitId, UnityAdsShowCompletionState.COMPLETED);
#endif
#if !UNITY_EDITOR
        while (_watchAdvTask == TaskStatus.Running)
            await Task.Delay(2000);

        OnUnityAdsShowComplete(_adUnitId, UnityAdsShowCompletionState.COMPLETED);
#endif

        return _watchAdvTask == TaskStatus.RanToCompletion;
    }


    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log("Unity Ads Rewarded Ad:" + showCompletionState.ToString());
        Advertisement.Load(_adUnitId, this);
        _watchAdvTask = TaskStatus.RanToCompletion;
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        Advertisement.Load(_adUnitId, this);
        _watchAdvTask = TaskStatus.Faulted;
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        Advertisement.Load(_adUnitId, this);
        _watchAdvTask = TaskStatus.Faulted;
    }
    #region Analitics
    public void OnUnityAdsShowStart(string adUnitId)
    {
        Debug.Log("Started watching an ad");
    }

    public void OnUnityAdsShowClick(string adUnitId)
    {
        Debug.Log("User clicked in the ad");
    }
    #endregion
    public void Clear() { }
}