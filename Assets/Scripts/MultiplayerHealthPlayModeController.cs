using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerHealthPlayModeController : MonoBehaviour
{
    [SerializeField]
    Button multiplayerButton;

    [SerializeField]
    MultiplayerHealthRewardedAds multiplayerHealthRewardedAds;

    private void Awake()
    {
        multiplayerHealthRewardedAds.OnAdsSuccessfullyWatched += MultiplayerHealthRewardedAds_OnAdsSuccessfullyWatched;
    }

    private void MultiplayerHealthRewardedAds_OnAdsSuccessfullyWatched()
    {
        //HandleButtonIntearacitivity();
    }

    private void OnEnable()
    {
        //HandleButtonIntearacitivity();
    }

    private void HandleButtonIntearacitivity()
    {
        var multiplayerHealth = PlayerPrefs.GetInt("MultiplayerHealth", 3);
        if (multiplayerHealth <= 0)
            multiplayerButton.interactable = false;
        else
            multiplayerButton.interactable = true;
    }

    public void OnMultiplayerEnter()
    {
        var multiplayerHealth = PlayerPrefs.GetInt("MultiplayerHealth", 3);
        PlayerPrefs.SetInt("MultiplayerHealth", multiplayerHealth - 1);
        if (multiplayerHealth <= 0)
        {
            FindObjectOfType<MultiplayerHealthRewardedAds>().ShowAd();
        }
    }
}
