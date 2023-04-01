using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocaleChange : MonoBehaviour
{
    public void ChangeLanguage(int LanguageID)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[LanguageID];

    }
}
