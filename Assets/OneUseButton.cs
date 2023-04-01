using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class OneUseButton : MonoBehaviour
{
    public string GenericEmail = "@email.com";
    public string GenericPassword = "gmfubQ0pfDgLpyEZsLpX";
    public void OnPress()
    {
        RegisterWithEmail(PlayFabManager._Instance.emailInput.text);
    }
    public void OnSkip()
    {
        RegisterWithEmail((int)(Random.value * 100000) + GenericEmail);
    }

    public void RegisterWithEmail(string email) { 
       RegisterPlayFabUserRequest registerRequest = new PlayFab.ClientModels.RegisterPlayFabUserRequest
        {
            Email = email,
            Password = GenericEmail,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, (RegisterPlayFabUserResult result)=>
        {
            Debug.Log("Succesfully registered " + email);
            PlayFabManager._Instance.EnableDisableObjects(questionsCanvasBool: true, questionsAudioManagerBool: true);
        }, (PlayFabError error) => {

            LoginWithEmailAddressRequest loginrequest = new LoginWithEmailAddressRequest
            {
                Email = email,
                Password = GenericEmail,
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetUserData = true,
                }
            };
            PlayFabClientAPI.LoginWithEmailAddress(loginrequest, (LoginResult result) =>
            {
                Debug.Log("Succesfully login " + email);
                PlayFabManager._Instance.GetUserData(result.PlayFabId);

            }, (PlayFabError error) =>
            {
                PlayFabManager._Instance.messageText.text = LocalizeError("Please enter a valid email", "Anna voimassa oleva sähköpostiosoite");
            });
        });
    }
    public string LocalizeError(string english, string finnish)
    {
        {
            if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
            {
                //Debug.Log("Selected language is ENGLISH");            
                return english;
            }
            else
            {
                return finnish;
            }
        }
    }
}
