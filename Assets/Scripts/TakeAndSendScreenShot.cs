using UnityEngine;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Collections;
using TMPro;
using UnityEngine.Localization.Settings;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class TakeAndSendScreenShot : MonoBehaviour
{
    //*********************Change all this params!!!!!!*******************
    string senderEmail = "kokemuspisteita@gmail.com";
    string senderPasswaord = "Menestys2022";
    string receiverEmail = "";
    //********************************************************************

    [SerializeField] TMP_InputField email;
    [SerializeField] TMP_Text emailFeedback;

    [SerializeField] Animation animation;

    public GameObject[] TempHideElements;

    //Validating that input field is an email
    private bool isEmailVadid = false;
    private const string MatchEmailPattern =
        @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
        + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
        + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
        + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

    private void OnEnable()
    {
        email.text = receiverEmail = PlayFabManager._Instance.emailInput.text;
        Debug.Log(receiverEmail);
        foreach (GameObject temp in TempHideElements)
        {
            if (temp.TryGetComponent<Image>(out Image tempImage))
            {
                tempImage.enabled = false;
            }
        }
    }
    private void OnDisable()
    {
        foreach (GameObject temp in TempHideElements)
        {
            if (temp.TryGetComponent<Image>(out Image tempImage))
            {
                tempImage.enabled = true;
            }
        }
    }

    public void TakeScreenShot()
    {
        isEmailVadid = validateEmail(email.text);

        if (isEmailVadid)
        {
            PlayFabManager._Instance.HideTkePictureUI(takePicturePanelBool: false);
            receiverEmail = email.text;
            StartCoroutine(WaitforScreenshotToFinnish());
        }
        else
        {
            if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
            {
                //Debug.Log("Selected language is ENGLISH");
                emailFeedback.text = "Please enter a valid email.";
            }
            else
            {
                emailFeedback.text = "Ole hyvä ja syötä toimiva sähköpostiosoite";
            }
            PlayEmailFeedbackAnimation();
        }


    }

    //The email in the input field needs to be validated first!!!


    IEnumerator WaitforScreenshotToFinnish()
    {

        ScreenCapture.CaptureScreenshot("Kpasa.png");
        yield return new WaitForEndOfFrame();
       // yield return new WaitForSeconds(1f);

        if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
        {
            //Debug.Log("Selected language is ENGLISH");
            emailFeedback.text = "Sending email...";
        }
        else
        {
            emailFeedback.text = "Lähetetään sähköpostia...";
        }
        PlayEmailFeedbackAnimation();

        yield return new WaitForSeconds(1f);
        SendEmail();
    }

    public void SendEmail()
    {
        Debug.Log("Receiver email is : " + receiverEmail);
        MailMessage mail = new MailMessage();

        mail.From = new MailAddress(senderEmail);
        mail.To.Add(receiverEmail);
        mail.Subject = "Artmuseum - Hahmotin App";
        mail.Body = "Kiitos Hahmottimen käytöstä! Tässä luomasi hahmo. `\nLisätietoja osoitteesta: https://kokemuspisteita.wixsite.com/kokemuspisteita \n\n" + "********************\n\n" + "Thank you for making a character. As promised heres your character. \n" + "For more information visit: https://kokemuspisteita.wixsite.com/kokemuspisteita";

        // Remember: Application.persistentDataPath + "/Kpasa.png" --> tablet, "Kpasa.png" --> Unity
        mail.Attachments.Add(new Attachment(Application.persistentDataPath + "/Kpasa.png"));

        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new NetworkCredential(senderEmail, senderPasswaord) as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                Debug.Log("Email Success!!");
                return true;
            };

        try
        {
            smtpServer.Send(mail);
        }
        catch (System.Exception e)
        {
            Debug.Log("Email error: " + e);
            PlayFabManager._Instance.HideTkePictureUI(takePicturePanelBool: true);

            if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
            {
                //Debug.Log("Selected language is ENGLISH");
                emailFeedback.text = "Something went wrong. Email not sent.";
            }
            else
            {
                emailFeedback.text = "Jotain meni pieleen. Sähköpostia ei lähetetty.";
            }
            PlayEmailFeedbackAnimation();


        }
        finally
        {
            Debug.Log("Email sent!");
            PlayFabManager._Instance.HideTkePictureUI(takePicturePanelBool: true);

            if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
            {
                //Debug.Log("Selected language is ENGLISH");
                emailFeedback.text = "Email sent successfully.";
            }
            else
            {
                emailFeedback.text = "Sähköposti lähetetty onnistuneesti.";
            }
            PlayEmailFeedbackAnimation();

        }
    }

    public void PlayEmailFeedbackAnimation()
    {
        animation.GetComponent<Animation>().Play();
        //animation.Play();

    }

    public static bool validateEmail(string email)
    {
        if (email != null)
            return Regex.IsMatch(email, MatchEmailPattern);
        else
            return false;
    }

}