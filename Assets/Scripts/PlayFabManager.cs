
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using System;
using UnityEngine.Localization.Settings;
using Random = UnityEngine.Random;

public class PlayFabManager : MonoBehaviour
{
    //creatorcharacter94@gmail.com
    //Jyvaskyla2021

    [Header("Canvases")]
    public GameObject loginCanvas;
    public GameObject questionAudioManager;
    public GameObject questions;
    public GameObject characterList;
    public GameObject mainCanvas;
    public GameObject backgroundManager;
    public GameObject takePictureCanvas;
    public GameObject serverCanvas; 
    public GameObject settingsCanvas;
    public GameObject TakePicturePanel;
    public GameObject settingContainer;


    [HideInInspector] public int savedHeadIndex, 
        savedTorsoIndex, 
        savedLegsIndex;

    [Header("UI")]
    public TMP_Text messageText;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    [Header("Server Stuff")]
    public GameObject characterCopyFromServer;
    public GameObject postioningClones;
    public GameObject characterToBeCloned;

    //Positioning clones
    float[] clonePositionX = new float[5] { -2.06f, 0.68f, -4.44f, -7.65f, 5.04f };
    float[] clonePositionY = new float[5] {1.6f, -2.08f, -1.49f, 0.17f, -1.28f };

    //Questionaire answer
    [Header("Taking picture")]
    public TMP_Text favouriteStyle;
    [HideInInspector]public string questionaireAnswer = "";


    public int currentHead, currentTorso, currentLegs;


    public static PlayFabManager _Instance { get { return instance; } }
    private static PlayFabManager instance;

    private void Awake()
    {
        EnableDisableObjects(loginCanvasBool: true, questionsAudioManagerBool: true) ;
    }

    void Start()
    {
        instance = this;
        Login();
    }

    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    void OnSuccess(LoginResult result)
    {
        //Debug.Log("Successful login/account created!");
    }

    void OnError(PlayFabError error)
    {
        if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
        {
            //Debug.Log("Selected language is ENGLISH");            
            messageText.text = "User not found or incorrect password";
        }
        else
        {
            messageText.text = "Käyttäjää ei löydy tai salasana on väärä";
        }
        //Debug.Log(error.GenerateErrorReport());
    }

    public void RegisterButton()
    {
        if (passwordInput.text.Length < 6)
        {
            //Debug.Log("Selected language is: " + LocalizationSettings.SelectedLocale);

            if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
            {
                //Debug.Log("Selected language is ENGLISH");
                messageText.text = "Password too short. Minimum 6 letters.";
            }
            else
            {
                messageText.text = "Salasana on liian lyhyt. Vähintään 6 kirjainta.";
            }

            return;
        }
        //Debug.Log(passwordInput.text.ToString());
        var request = new RegisterPlayFabUserRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text,
            RequireBothUsernameAndEmail = false
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {

        if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
        {
            //Debug.Log("Selected language is ENGLISH");
            messageText.text = "Registered and logged in";
        }
        else
        {
            messageText.text = "Rekisteröity ja kirjautunut sisään";
        }

        EnableDisableObjects(questionsCanvasBool: true, questionsAudioManagerBool: true);
    }

    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetUserData = true,
            }
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
        {
            //Debug.Log("Selected language is ENGLISH");            
            messageText.text = "Logged in";
        }
        else
        {
            messageText.text = "Kirjautunut sisään";
        }


        GetUserData(result.PlayFabId);
    }

    public void GetUserData(string myPlayFabeId)
    {

        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myPlayFabeId,
            Keys = null
        }, result =>
        {
            //Debug.Log("Got user data:");
            if (result.Data == null || !result.Data.ContainsKey("Head")) 
            {
                //Debug.Log("No data, back to questionaire!!");
                EnableDisableObjects(questionsAudioManagerBool: true, questionsCanvasBool: true);                
            }
            else
            {
                if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
                {
                    //Debug.Log("Selected language is ENGLISH");   
                    messageText.text = "Loading...";
                }
                else
                {
                    messageText.text = "Ladataan...";
                }

                EnableDisableObjects(mainCanvasBool: true, characterListBool: true, backGroundManagerBool: true);
                savedHeadIndex = Int32.Parse(result.Data["Head"].Value);
                savedTorsoIndex = Int32.Parse(result.Data["Torso"].Value);
                savedLegsIndex = Int32.Parse(result.Data["Legs"].Value);
                questionaireAnswer = result.Data["Style"].Value;
                

                BodyPartSwitch._Instance.RetrievedPartsFromServer(savedHeadIndex, savedTorsoIndex, savedLegsIndex);

            }
        }, (error) =>
        {
            //Debug.Log("Got error retrieving user data:");
            //Debug.Log(error.GenerateErrorReport());
        });        
    }

    public void ResetPasswordButton()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailInput.text,

            //*****************************************************
            // Remember to change this for the final server title!!!!!
            TitleId = "2F147"
            //*****************************************************

        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
    }

    private void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
        {
            //Debug.Log("Selected language is ENGLISH"); 
            messageText.text = "Sent a new password to your email";
        }
        else
        {
            messageText.text = "Lähetti uuden salasanan sähköpostiisi";
        }

    }


    public void SaveAppearearance()
    {
        int[] bodypartsID = BodyPartSwitch._Instance.GetBodyPartsId();

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Head", bodypartsID[0].ToString() },
                {"Torso", bodypartsID[1].ToString() },
                {"Legs", bodypartsID[2].ToString() },
                {"Style", questionaireAnswer }
            }
        };
        
        SendLeaderBoard(bodypartsID[0], bodypartsID[1], bodypartsID[2]);
        
        currentHead = bodypartsID[0];
        currentTorso = bodypartsID[1];
        currentLegs = bodypartsID[2];

        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
                
    }

    void OnDataSend(UpdateUserDataResult result )
    {
        EnableDisableObjects(takePictuerCanvasBool: true, backGroundManagerBool: true, characterListBool: true);
        favouriteStyle.text = questionaireAnswer;        
    }


    public void GetAppearance()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataReceived, OnError);
    }

    public void OnDataReceived(GetUserDataResult result)
    {
        //Debug.Log("Received user data!");
        if (result.Data != null && result.Data.ContainsKey("Head"))
        {
            savedHeadIndex = Int32.Parse(result.Data["Head"].Value);
            savedTorsoIndex = Int32.Parse(result.Data["Torso"].Value);
            savedLegsIndex = Int32.Parse(result.Data["Legs"].Value);
            questionaireAnswer = result.Data["Style"].Value;

            BodyPartSwitch._Instance.RetrievedPartsFromServer(savedHeadIndex, savedTorsoIndex, savedLegsIndex);
        }
        else
        {
            //Debug.Log("Character Data not received....");
        }
    }

    //LeaderBoardStuff
    public void SendLeaderBoard(int head, int torso, int legs)
    {
        int score = head * 10000 + torso * 100 + legs * 1;

        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    //*************************************** The leaderboard
                    StatisticName = "TestingSttistics",
                    Value = score
                    //****************************************
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, OnError);
    }

    private void OnLeaderBoardUpdate(UpdatePlayerStatisticsResult result)
    {
        //Debug.Log("Succesfull leaderboard sent");
    }
    
    public void GetLeaderBoardAroundPlayer()
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = "TestingSttistics",
            MaxResultsCount = 5,
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnLeaderboardAroundPlayerGet, OnError);
    }

    private void OnLeaderboardAroundPlayerGet(GetLeaderboardAroundPlayerResult result)
    {
        EnableDisableObjects(backGroundManagerBool:true, serverCanvasBool: true, characterToBeClonedBool: true);
        int playerScore;
        int layer = 100;

        int indexX = 0;
        

        foreach (var item in result.Leaderboard)
        {
            playerScore = item.StatValue;

            if (playerScore == 0)
            {
                savedHeadIndex = 0;
                savedTorsoIndex = 0;
                savedLegsIndex = 0;
            }
            else
            {
                savedHeadIndex = (int)Mathf.Floor(playerScore / 10000);
                playerScore -= savedHeadIndex * 10000;
                savedTorsoIndex = (int)Mathf.Floor(playerScore / 100);
                playerScore -= savedTorsoIndex * 100;
                savedLegsIndex = playerScore;
            }

            

            //********************************
            //clonePositionX = Random.Range(-7.63f, 6.8f);
            //clonePositionY = Random.Range(-2.32f, 2.9f);
            //Debug.Log(clonePositionX);
            //********************************


            var characterCopy = Instantiate(characterCopyFromServer);
            characterCopy.gameObject.tag = "Clone";
            //characterCopy.transform.position -= Vector3.forward * clonePositionY[indexX] * .1f;
            //characterCopy.GetComponent<SpriteRenderer>().sortingLayerName = newLayer.ToString();
            var layerorders = characterCopy.GetComponentsInChildren<SpriteRenderer>();
            foreach (var layers in layerorders)
            {
                layers.GetComponent<SpriteRenderer>().sortingOrder = layers.GetComponent<SpriteRenderer>().sortingOrder + layer;
            }

            layer += 50;
            
            int parts = 0;
            foreach (Transform part in characterCopy.transform)
            {                

                //Debug.Log("Part = " + parts + " x: " + clonePositionX + " y: " + clonePositionY);

                part.transform.position = new Vector3(part.transform.position.x + clonePositionX[indexX], part.transform.position.y + clonePositionY[indexX], 0);
                //Debug.Log("parts " + parts);
                parts++;
            }

            indexX++;
            ServerBodyPartSwitch._Instance.RetrievedPartsFromLeaderBoard(savedHeadIndex, savedTorsoIndex, savedLegsIndex);
            characterCopy.SetActive(true);
        }
        EnableDisableObjects(backGroundManagerBool:true, serverCanvasBool: true);
    }

    // Skip Button
    public void SkipButton()
    {
        if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
        {
            //Debug.Log("Selected language is ENGLISH"); 
            questionaireAnswer = "No idea, you skipped...";
        }
        else
        {
            questionaireAnswer = "Ei aavistustakaan, ohitit...";
        }

        EnableDisableObjects(mainCanvasBool: true, characterListBool: true, backGroundManagerBool:true);

    }
    public void EnableDisableObjects(bool loginCanvasBool = false, bool questionsAudioManagerBool = false, bool questionsCanvasBool = false, bool mainCanvasBool = false, bool characterListBool = false, bool backGroundManagerBool = false, bool serverCanvasBool = false, bool takePictuerCanvasBool = false, bool characterToBeClonedBool = false, bool settingContainerBool = true)
    {
        loginCanvas.SetActive(loginCanvasBool);
        questionAudioManager.SetActive(questionsAudioManagerBool);
        questions.SetActive(questionsCanvasBool);
        mainCanvas.SetActive(mainCanvasBool);
        characterList.SetActive(characterListBool);
        backgroundManager.SetActive(backGroundManagerBool);
        serverCanvas.SetActive(serverCanvasBool);
        takePictureCanvas.SetActive(takePictuerCanvasBool);
        characterToBeCloned.SetActive(characterToBeClonedBool);
        settingContainer.SetActive(settingContainerBool);
    }

    public void EnableSettingsDisable()
    {
        if (settingsCanvas.activeSelf == true)
        {
            settingsCanvas.SetActive(false);
        }
        else
        {
            settingsCanvas.SetActive(true);
        }
    }

    public void BackToCharacterSelection()
    {
        var clones = GameObject.FindGameObjectsWithTag("Clone");
        if (clones.Length > 0)
        {
            foreach (var clone in clones)
            {
                Destroy(clone);
            }
        }
        EnableDisableObjects(mainCanvasBool: true, characterListBool: true, backGroundManagerBool: true);
        BodyPartSwitch._Instance.BackToCharacterSelectionWithSelectedChararacter();
    }

    public void HideTkePictureUI(bool takePicturePanelBool = true)
    {
        TakePicturePanel.SetActive(takePicturePanelBool);
    }

}
