using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;

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

    //variables
    [Header("Dialogue Prefabs")]
    public TextAsset[] travellerResponses;
    public GameObject dialogueArea;
    public GameObject playerDialogueBox;
    public GameObject travelerDialogueBox;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    public Story currentStory;
    public bool dialogueIsPlaying = false;

    private bool canContinueToNextLine = false;
    private void Update()
    {
        if (currentStory.canContinue)
        {
            displayDialogue(0, 1);
        }
    }

    private void Start()
    {
        currentStory = new Story(travellerResponses[0].text);
    }

    public void displayDialogue(int responseIndex, int categoryIndex)
    {
        //Retrieve Dialogue From Inky Script
        
        //Assign Dialogue
        //Spawn Dialogue Boxes and Set Dialogue
        //GameObject playerBoxInstance = Instantiate(playerDialogueBox, dialogueArea.transform.position, dialogueArea.transform.rotation, dialogueArea.transform);
        //playerBoxInstance.GetComponentInChildren<TMP_Text>().text = "BALLS";
        if (currentStory.canContinue)
        {
            GameObject playerBoxInstance = Instantiate(playerDialogueBox, dialogueArea.transform.position, dialogueArea.transform.rotation, dialogueArea.transform);
            playerBoxInstance.GetComponentInChildren<TMP_Text>().text = currentStory.Continue();
            DisplayChoices();
        }

        //GameObject travelerBoxInstance = Instantiate(travelerDialogueBox, dialogueArea.transform.position, dialogueArea.transform.rotation, dialogueArea.transform);
        //travelerBoxInstance.GetComponentInChildren<TMP_Text>().text = "BALLS AGAIN";
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        // defensive check to make sure our UI can support the number of choices coming in
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given: "
                + currentChoices.Count);
        }

        int index = 0;
        // enable and initialize the choices up to the amount of choices for this line of dialogue
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            //choicesText[index].text = choice.text;
            index++;
        }
        // go through the remaining choices the UI supports and make sure they're hidden
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int index)
    {
        currentStory.ChooseChoiceIndex(index);
        displayDialogue(0, 0);
    }
}
