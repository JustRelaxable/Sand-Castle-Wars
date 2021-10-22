using System.Collections;
using System.Collections.Generic;
using Unity.RemoteConfig;
using UnityEngine;

public class RatePopupController : MonoBehaviour
{
    [SerializeField]
    GameObject adsManager;

    [SerializeField]
    int maxTimeForPopup;

    [SerializeField]
    GameObject popup;

    private void Awake()
    {
        var iAdEvents = adsManager.GetComponents<IAdEvents>();
        ConfigManager.FetchCompleted += ConfigManager_FetchCompleted;
        foreach (var item in iAdEvents)
        {
            item.OnAdsSuccessfullyWatched += Ýtem_OnAdsSuccessfullyWatched;
        }
    }

    private void ConfigManager_FetchCompleted(ConfigResponse obj)
    {
        switch (obj.requestOrigin)
        {
            case ConfigOrigin.Default:
                break;
            case ConfigOrigin.Cached:
                break;
            case ConfigOrigin.Remote:
                maxTimeForPopup = ConfigManager.appConfig.GetInt("MaxTimeForPopup",20);
                break;
            default:
                break;
        }
    }

    private void Ýtem_OnAdsSuccessfullyWatched()
    {
        var popupTime = PlayerPrefs.GetInt("PopupTime", 20);
        PlayerPrefs.SetInt("PopupTime", popupTime - 1);
        if(popupTime <= 0)
        {
            //Show Popup
            popup.SetActive(true);
            PlayerPrefs.SetInt("PopupTime", maxTimeForPopup);
        }
    }
}
