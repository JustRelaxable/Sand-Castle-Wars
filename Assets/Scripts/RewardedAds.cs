using System;
using System.Collections;
using System.Collections.Generic;
using Unity.RemoteConfig;
using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedAds : MonoBehaviour,IUnityAdsListener,IAdEvents
{
    [SerializeField] string _androidAdUnitId = "";
    public event Action OnAdsReady;
    public event Action OnAdsSuccessfullyWatched;

    private void Awake()
    {
        ConfigManager.FetchCompleted += ConfigManager_FetchCompleted;
        Advertisement.AddListener(this);
    }

    protected virtual void ConfigManager_FetchCompleted(ConfigResponse obj)
    {

    }

    public virtual void ShowAd()
    {
        Advertisement.Show(_androidAdUnitId);
    }

    public void OnUnityAdsReady(string placementId)
    {
        if (placementId != _androidAdUnitId)
            return;
        OnAdsReady?.Invoke();
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.Log(message);
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        if (placementId != _androidAdUnitId)
            return;
    }


    public virtual void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId != _androidAdUnitId)
            return;

        switch (showResult)
        {
            case ShowResult.Failed:
                break;
            case ShowResult.Skipped:
                break;
            case ShowResult.Finished:
                OnAdsSuccessfullyWatched?.Invoke();
                break;
            default:
                break;
        }
    }
}
