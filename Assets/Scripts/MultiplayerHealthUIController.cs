using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerHealthUIController : MonoBehaviour
{
    [SerializeField]
    Button multiplayerHealthButton;
    [SerializeField]
    Text multiplayerHealthButtonText;
    [SerializeField]
    MultiplayerHealthRewardedAds multiplayerHealthRewardedAds;

    private void Awake()
    {
        multiplayerHealthButton.interactable = false;
        multiplayerHealthButtonText.text = "Ad is preparing, please wait";
    }

    private void OnEnable()
    {
        multiplayerHealthRewardedAds.OnAdsReady += MultiplayerHealthRewardedAds_OnAdsReady;
    }

    private void MultiplayerHealthRewardedAds_OnAdsReady()
    {
        multiplayerHealthButton.interactable = true;
        multiplayerHealthButtonText.text = "Increase MultiplayerHealth";
    }

    private void OnDisable()
    {
        multiplayerHealthRewardedAds.OnAdsReady -= MultiplayerHealthRewardedAds_OnAdsReady;
    }
}
