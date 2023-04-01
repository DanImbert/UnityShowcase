using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Voices : MonoBehaviour
{
    [SerializeField]private AudioClip[] characterVoices;
    private AudioSource audioSource;
    public int voiceIndex;


    
    
    //private GameObject[] voiceManager;
    //private int index;


    private void Start()
    {
        /*voiceManager = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)

            voiceManager[i] = transform.GetChild(i).gameObject;


        //toggle off 
        foreach (GameObject song in voiceManager)

            song.SetActive(false);
        */
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayCharacterVoice()
    {
        voiceIndex = BodyPartSwitch._Instance.GetHeadId();
        audioSource.clip = characterVoices[voiceIndex];
        audioSource.Play();
    }
    /*
    public void SwitchVoiceR()
    {
        voiceManager[index].SetActive(false);
        index++; //index -= 1; index = index - 1;
        if (index == voiceManager.Length)
            index = 0;


        // Toggle on the new back
       // voiceManager[index].SetActive(true);
    }

    public void SwitchVoiceL()
    {
        //Toggle off current model
        voiceManager[index].SetActive(false);
        index--; //index -= 1; index = index - 1;
        if (index < 0)
            index = voiceManager.Length - 1;

        // Toggle on the new model
       // voiceManager[index].SetActive(true);
    }

    public void PLayVoice()
    {

        //toggle on the selected 
        if (voiceManager[index])
            voiceManager[index].SetActive(true);
    }
    */
}
