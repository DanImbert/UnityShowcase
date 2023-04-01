using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocaleDropdown : MonoBehaviour
{
    public TMP_Dropdown Languageselect;
    //public TMP_Text initialLabel;

    private void Start()
    {
        Languageselect = GetComponent<TMP_Dropdown>();
        Languageselect.value = LocalizationSettings.SelectedLocale.GetInstanceID();
        //ChangeLanguage(Languageselect);
    }
    public void ChangeLanguage(TMP_Dropdown drop)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[Languageselect.value];       

    }
}