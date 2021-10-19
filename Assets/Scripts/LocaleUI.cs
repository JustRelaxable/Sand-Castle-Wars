using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocaleUI : MonoBehaviour
{
    [SerializeField]
    LocalizationChanger localizationChanger;
    [SerializeField]
    SystemLanguage localeLanguage;

    public void OnSelected()
    {
        localizationChanger.ChangeLocale(localeLanguage);
    }
}
