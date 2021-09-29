using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class RoundBasedAds : MonoBehaviour,IUnityAdsListener
{
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] public bool adReady;
    [SerializeField] private ClientGameManager clientGameManager;
    private int adCount = 0;

    private void Awake()
    {
        Advertisement.AddListener(this);
        clientGameManager.OnGameStarted += ClientGameManager_OnGameStarted;
        adReady = false;
    }

    private void ClientGameManager_OnGameStarted()
    {
        var castleTurnControllers = FindObjectsOfType<CastleTurnController>();
        foreach (var castleTurnController in castleTurnControllers)
        {
            castleTurnController.OnTurnMine += CastleTurnController_OnTurnMine;
        }
    }

    private void CastleTurnController_OnTurnMine(bool obj)
    {
        adCount++;
        if(adCount % 10 == 0)
        {
            ShowRoundBasedAd();
        }
    }

    public void ShowRoundBasedAd()
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
