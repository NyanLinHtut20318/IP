using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    //Singleton
    private static DialogueManager dialogueManagerInstance;

    public static DialogueManager GetInstance { get { return dialogueManagerInstance; } }

    private void Awake()
    {
        if (dialogueManagerInstance != null && dialogueManagerInstance != this)
        {
            Debug.LogWarning("More than 1 DialogueManager in Scene");
            Destroy(gameObject);
        }
        else
        {
            dialogueManagerInstance = this;
        }
    }

    //This script will mamage all Inky and Dialogue Integration
    //Travellers on Default will be spawned with Default Response Options
    //When there is a discrepency, the Default Response Option will be Replaced with an Inky Script related to the discrepency assigned (set in SpawnManager)

    private void Start()
    {

    }
}
