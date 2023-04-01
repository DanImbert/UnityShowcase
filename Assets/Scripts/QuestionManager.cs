using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;


public class QuestionManager : MonoBehaviour
{
    public Questionaire questionaire;
    public GameObject q;
    public GameObject a;
    public int[] qaArr;
    public int currentQuestion = 0;
    public GameObject finalAnswerPanel;
    public GameObject selectedStyle;
    public GameObject questionPanel;
    public GameObject answerPanel;



    // Start is called before the first frame update
    void Awake()
    {
        qaArr = new int[questionaire.Questions.Length];
    }

    public void OnEnable()
    {
        currentQuestion = 0;
        AssignQuestion(questionaire.Questions[currentQuestion]);
        finalAnswerPanel.SetActive(false);
        questionPanel.SetActive(true);
        answerPanel.SetActive(true);
    }

    public void AssignQuestion(Questionaire.Question quest)
    {
        GetComponentInChildren<ToggleGroup>().SetAllTogglesOff();
        //q.GetComponent<Text>().text = quest.question;
        q.GetComponent<LocalizeStringEvent>().StringReference.SetReference("en fi", quest.question);



        for (int i = 0; i < a.transform.childCount; i++)
        {
            if (i >= quest.Answers.Length)
            {
                a.transform.GetChild(i).gameObject.SetActive(false);

            }

            else
            {
                a.transform.GetChild(i).gameObject.SetActive(true);
                //a.transform.GetChild(i).GetComponentInChildren<Text>().text = quest.Answers[i].answer;
                a.transform.GetChild(i).GetComponentInChildren<LocalizeStringEvent>().StringReference.SetReference("en fi", quest.Answers[i].answer);

            }
        }
    }

    public void Submit()
    {
        if (currentQuestion >= questionaire.Questions.Length)
        {
            finalAnswerPanel.SetActive(false);
            questionPanel.SetActive(true);
            answerPanel.SetActive(true);

            PlayFabManager._Instance.EnableDisableObjects(mainCanvasBool: true, characterListBool: true, backGroundManagerBool: true);
            BodyPartSwitch._Instance.SetStyle();
            return;
        }
        else
        {
            //Debug.Log("Start: " + currentQuestion);

            if (!GetComponentInChildren<ToggleGroup>().AnyTogglesOn())
                return;
            qaArr[currentQuestion] = ReadQuestionAndAnswer();
            currentQuestion++;
            //Debug.Log("End: " + currentQuestion);

            if (currentQuestion < questionaire.Questions.Length)
            {
                AssignQuestion(questionaire.Questions[currentQuestion]);
            }
            else
            {
                questionPanel.SetActive(false);
                answerPanel.SetActive(false);
                DisplayResult();            
            }
        }
    }

    int ReadQuestionAndAnswer()
    {
        if (a.GetComponent<ToggleGroup>() != null)
        {
            for (int i = 0; i < a.transform.childCount; i++)
            {
                if (a.transform.GetChild(i).GetComponent<Toggle>().isOn)
                {
                    return i;
                }
            }
        }

        return -1;
    }

    void DisplayResult()
    {
        //string s = "Congratulations your style is: ";

        int[] results = new int[6];
        for (int i = 0; i < questionaire.Questions.Length; i++)
            for (int k = 0; k < questionaire.Questions[i].Answers[qaArr[i]].Changes.Length; k++)
                results[(int)questionaire.Questions[i].Answers[qaArr[i]].Changes[k]]++;

        int answer = 0;
        int value = 0;
        for (int k = 0; k < results.Length; k++)
        {
            if (results[k] > value)
            {
                value = results[k];
                answer = k;
            }
        }

        finalAnswerPanel.SetActive(true);
        selectedStyle.GetComponent<Text>().text = "" + (styles)answer + "";
        PlayFabManager._Instance.questionaireAnswer = "" + (styles)answer + "";
        BodyPartSwitch.chosenStyle = (styles)answer;
    }   

    public enum styles
    {
        ANIME = 0,
        PIXEL = 1,
        VECTOR = 2,
        INDIE = 3,
        LOWBLOCK = 4,
        REALISM = 5,
       
    }


}