using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocalizationGameObjectactivator : MonoBehaviour
{
    public LocalizedGameObjectSettings[] localizedGameObjectSettings;
    private void Awake()
    {
        LocalizationSettings.SelectedLocaleChanged += LocalizationSettings_SelectedLocaleChanged;
        LocalizationSettings_SelectedLocaleChanged(LocalizationSettings.SelectedLocale);
    }

    private void LocalizationSettings_SelectedLocaleChanged(Locale locale)
    {
        for (int i = 0; i < localizedGameObjectSettings.Length; i++)
        {
            if(localizedGameObjectSettings[i].LocaleIdentifier == locale.Identifier)
            {
                localizedGameObjectSettings[i].gameObject.SetActive(true);
            }
            else
            {
                localizedGameObjectSettings[i].gameObject.SetActive(false);
            }
        }
    }
}

[System.Serializable]
public class LocalizedGameObjectSettings
{
    public GameObject gameObject;
    public SystemLanguage systemLanguage;
    public LocaleIdentifier LocaleIdentifier { get { return new LocaleIdentifier(systemLanguage); } }
}
