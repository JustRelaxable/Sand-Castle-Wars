using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocalizationChanger : MonoBehaviour
{
    private void Awake()
    {
        int selectedLocale = PlayerPrefs.GetInt("SelectedLocale",-1);
        if (selectedLocale < 0)
            return;
        SystemLanguage systemLanguage = (SystemLanguage)selectedLocale;
        LocaleIdentifier localeIdentifier = new LocaleIdentifier(systemLanguage);
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(localeIdentifier);
    }

    public void ChangeLocale(SystemLanguage language)
    {
        LocaleIdentifier localeIdentifier = new LocaleIdentifier(language);
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(localeIdentifier);
        PlayerPrefs.SetInt("SelectedLocale", (int)language);
    }
}
