using UnityEngine;

public class MenuManager : MonoBehaviour
{ 
    public void ResetQuestionaire()
    {
        //Destroying all the clones from the server
        var clones = GameObject.FindGameObjectsWithTag("Clone");
        if (clones.Length > 0)
        {
            foreach (var clone in clones)
            {
                Destroy(clone);
            }
        }

        PlayFabManager._Instance.EnableDisableObjects(loginCanvasBool: true, questionsAudioManagerBool: true);
    }   
}