using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System;
using UnityEngine.Networking;
using System.Linq;

public class MultiplayerHealthRewardedAds : MonoBehaviour, IUnityAdsListener
{
    [SerializeField] string _androidAdUnitId = "Interstitial_Android";
    [SerializeField] public bool adReady;
    public event Action OnAdsReady;
    public event Action OnAdsSuccessfullyWatched;

    private void Awake()
    {
        Advertisement.AddListener(this);
        adReady = false;
    }

    public void ShowAd()
    {
        Advertisement.Show(_androidAdUnitId);
    }

    public void OnUnityAdsReady(string placementId)
    {
        if (placementId != _androidAdUnitId)
            return;
        adReady = true;
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


    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
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

        adReady = false;
    }
}