using System.Collections;
using System.Collections.Generic;
using Unity.RemoteConfig;
using UnityEngine;
using UnityEngine.Advertisements;

public class GameEndAds : RewardedAds
{
    [SerializeField]
    private bool OnGameEndAdsActive = true;
    protected override void ConfigManager_FetchCompleted(ConfigResponse obj)
    {
        switch (obj.requestOrigin)
        {
            case ConfigOrigin.Default:
                break;
            case ConfigOrigin.Cached:
                break;
            case ConfigOrigin.Remote:
                OnGameEndAdsActive = ConfigManager.appConfig.GetBool("ShowAdsOnGameEnd", true);
                break;
            default:
                break;
        }
    }
    public override void ShowAd()
    {
        if (OnGameEndAdsActive)
            base.ShowAd();
    }
    public override void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        base.OnUnityAdsDidFinish(placementId, showResult);
    }
}
