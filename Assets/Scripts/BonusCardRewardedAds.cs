using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System;
using UnityEngine.Networking;
using System.Linq;

public class BonusCardRewardedAds : MonoBehaviour, IUnityAdsListener
{
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] public bool adReady;

    private void Awake()
    {
        Advertisement.AddListener(this);
        adReady = false;
    }

    public void ShowBonusCardRewardedAd()
    {
        Advertisement.Show(_androidAdUnitId);
    }

    public void OnUnityAdsReady(string placementId)
    {
        if (placementId != _androidAdUnitId)
            return;
        adReady = true;
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
                
                break;
            default:
                break;
        }

        adReady = false;
    }
}