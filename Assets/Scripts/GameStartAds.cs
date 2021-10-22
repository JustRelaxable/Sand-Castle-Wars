using System.Collections;
using System.Collections.Generic;
using Unity.RemoteConfig;
using UnityEngine;
using UnityEngine.Advertisements;

public class GameStartAds :RewardedAds
{
    [SerializeField]
    private bool OnGameStartAdsActive = true;
    protected override void ConfigManager_FetchCompleted(ConfigResponse obj)
    {
        switch (obj.requestOrigin)
        {
            case ConfigOrigin.Default:
                break;
            case ConfigOrigin.Cached:
                break;
            case ConfigOrigin.Remote:
                OnGameStartAdsActive = ConfigManager.appConfig.GetBool("ShowAdsOnGameStart", true);
                break;
            default:
                break;
        }
    }
    public override void ShowAd()
    {
        if(OnGameStartAdsActive)
            base.ShowAd();
    }
    public override void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        base.OnUnityAdsDidFinish(placementId, showResult);
    }
}
