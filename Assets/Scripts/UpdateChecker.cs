using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.RemoteConfig;

public class UpdateChecker : MonoBehaviour
{
    [SerializeField]
    GameObject updatePopup;
    private void Awake()
    {
        ConfigManager.FetchCompleted += ConfigManager_FetchCompleted;
    }

    private void ConfigManager_FetchCompleted(ConfigResponse obj)
    {
        switch (obj.requestOrigin)
        {
            case ConfigOrigin.Default:
                break;
            case ConfigOrigin.Cached:
            case ConfigOrigin.Remote:
                var remoteVersion = ConfigManager.appConfig.GetString("Version");
                if (remoteVersion != Application.version)
                    updatePopup.SetActive(true);
                break;
            default:
                break;
        }
    }
}
