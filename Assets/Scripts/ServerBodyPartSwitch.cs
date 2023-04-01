using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

public class ServerBodyPartSwitch : MonoBehaviour
{
    public static ServerBodyPartSwitch _Instance { get { return instance; } }
    private static ServerBodyPartSwitch instance;

    [SerializeField] BodyParts[] bodyParts; 
    [SerializeField] string[] labels; 
   
    private void Awake()
    {
        instance = this;
        
    }

    public int[] GetBodyPartsId()
    {
        return new int[] { bodyParts[0].id, bodyParts[1].id, bodyParts[2].id };
    }

    public void RetrievedPartsFromLeaderBoard(int heads, int torso, int legs)
    {
        bodyParts[0].UpdatePart(labels, heads);
        bodyParts[1].UpdatePart(labels, torso);
        bodyParts[2].UpdatePart(labels, legs);

    }

    [System.Serializable]
    public class BodyParts
    {
        
        [SerializeField] SpriteResolver[] spriteResolver; 
        public int id;

        public SpriteResolver[] SpriteResolver { get => spriteResolver; }

       
        public void UpdatePart(string[] labels, int index)
        {
            foreach (var item in spriteResolver)
            {
                item.SetCategoryAndLabel(item.GetCategory(), labels[index]);
            }
            id = index;
        }
    }
}
