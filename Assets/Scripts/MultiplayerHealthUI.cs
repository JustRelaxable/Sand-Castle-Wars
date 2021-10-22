using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MultiplayerHealthUI : MonoBehaviour
{
    private TextMeshProUGUI text;

    [SerializeField]
    MultiplayerHealthRewardedAds multiplayerHealthRewardedAds;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        multiplayerHealthRewardedAds.OnAdsSuccessfullyWatched += MultiplayerHealthRewardedAds_OnAdsSuccessfullyWatched;
    }

    private void MultiplayerHealthRewardedAds_OnAdsSuccessfullyWatched()
    {
        var multiplayerHealth = PlayerPrefs.GetInt("MultiplayerHealth", 5);
        if (multiplayerHealth < 5)
            PlayerPrefs.SetInt("MultiplayerHealth", multiplayerHealth + 1);
        UpdateText();
    }

    private void OnEnable()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        text.text = "Multiplayer Health: " + PlayerPrefs.GetInt("MultiplayerHealth", 5).ToString();
    }
}
