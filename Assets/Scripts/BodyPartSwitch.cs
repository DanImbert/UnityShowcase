using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;
using UnityEngine.UI;

public class BodyPartSwitch : MonoBehaviour
{
    public GenreCollection[] genres;
    public static QuestionManager.styles chosenStyle = 0;
    public static BodyPartSwitch _Instance { get { return instance; } }
    private static BodyPartSwitch instance;

    [SerializeField] BodyParts[] bodyParts;
    [SerializeField] string[] labels;

    private void Awake()
    {
        instance = this;

        for (int i = 0; i < bodyParts.Length; i++)
        {
            bodyParts[i].Init(labels);
        }
    }
    //private void OnEnable()
    //{
        /*string label = genres[(int)chosenStyle].Genre;
        bodyParts[0].UpdatePart(bodyParts[0].mlabels.IndexOf(label));
        bodyParts[1].UpdatePart(bodyParts[1].mlabels.IndexOf(label));
        bodyParts[2].UpdatePart(bodyParts[2].mlabels.IndexOf(label));
        */
        //RetrievedPartsFromServer(PlayFabManager._Instance.currentHead, PlayFabManager._Instance.currentTorso, PlayFabManager._Instance.currentLegs);
    //}
    public GenreCollection GetGenre()
    {
        return genres[(int)chosenStyle];
    }
    public void SetStyle()
    {
        string label = GetGenre().Genre;
        bodyParts[0].UpdatePart(bodyParts[0].mlabels.IndexOf(label));
        bodyParts[1].UpdatePart(bodyParts[1].mlabels.IndexOf(label));
        bodyParts[2].UpdatePart(bodyParts[2].mlabels.IndexOf(label));
    }

    public void BackToCharacterSelectionWithSelectedChararacter()
    {
        RetrievedPartsFromServer(PlayFabManager._Instance.currentHead, PlayFabManager._Instance.currentTorso, PlayFabManager._Instance.currentLegs);
    }

    public int[] GetBodyPartsId()
    {
        return new int[] { bodyParts[0].id, bodyParts[1].id, bodyParts[2].id };
    }

    public int GetHeadId()
    {
        return bodyParts[0].id;
    }
    

    public void RetrievedPartsFromServer(int heads, int torso, int legs)
    {
        bodyParts[0].UpdatePart( heads);
        bodyParts[1].UpdatePart( torso);
        bodyParts[2].UpdatePart( legs);
    }   

    [System.Serializable]
    public class BodyParts
    {
        [SerializeField] Button buttonRight;
        [SerializeField] Button buttonLeft;

        [SerializeField] SpriteResolver[] spriteResolver; 
        public int id;
        public List<string> mlabels;

        public SpriteResolver[] SpriteResolver { get => spriteResolver; }

        public void Init(string[] labels)
        {
            mlabels = new List<string>();
            mlabels.AddRange(labels);
            buttonRight.onClick.AddListener(delegate { ChangePartsToRight(); }); 
            buttonLeft.onClick.AddListener(delegate { ChangePartsToLeft(); });
        }

        public void ChangePartsToRight()
        {
            id++;
            id %= mlabels.Count; // prevents array from getting out of index 

            foreach (var item in spriteResolver)
            {
                //Debug.Log(item.GetCategory() + " " + labels[id] + " " + id);
                item.SetCategoryAndLabel(item.GetCategory(), mlabels[id]);
            }
        }

        public void ChangePartsToLeft()
        {
            id--;
            if (id < 0)
            {
                id = mlabels.Count - 1;
            }

            foreach (var item in spriteResolver)
            {
                //Debug.Log(item.GetCategory() + " " + labels[id] + " " + id);
                item.SetCategoryAndLabel(item.GetCategory(), mlabels[id]);
            }
        }

        public void UpdatePart( int index)
        {
            foreach (var item in spriteResolver)
            {
                item.SetCategoryAndLabel(item.GetCategory(), mlabels[index]);
            }
            id = index;
        }
    }
}
