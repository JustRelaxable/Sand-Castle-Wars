using System.Collections;
using System.Collections.Generic;
using Unity.RemoteConfig;
using UnityEngine;

public class RemoteConfigSettingsLoader : MonoBehaviour
{
    public struct userAttributes
    {
    }

    public struct appAttributes
    {
    }

    private void Awake()
    {
        ConfigManager.FetchConfigs<userAttributes, appAttributes>(new userAttributes(), new appAttributes());
    }
}
