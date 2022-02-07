using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Lexic;
using Ink.Runtime;

public class SpawnManager : MonoBehaviour
{
    //Singleton
    private static SpawnManager spawnManagerInstance;

    public static SpawnManager GetInstance { get { return spawnManagerInstance; } }

    private void Awake()
    {
        if (spawnManagerInstance != null && spawnManagerInstance != this)
        {
            Debug.LogWarning("More than 1 SpawnManager in Scene");
            Destroy(gameObject);
        }
        else
        {
            spawnManagerInstance = this;
        }
    }

    [Header("Traveller Spawner")]
    public bool validTraveller;
    public GameObject[] TravellerPrefabs;
    public GameObject TravellerPrefabsInstance;
    public int TravellerSpeciesInstanceIndex; //randomized species index
    [SerializeField]private Transform spawnPoint;
    [SerializeField]private bool travellerActive = false;

    [Header("Stat Parameters")]
    public int minDisc = 1; //set min and max documents with discrepencies allowed -> used to ramp up/adjust difficulty
    public int maxDisc = 1;
    public int minDiscInDoc = 1; //min num of discrepencies in a document -> used to ramp up/adjust difficulty

    //Set Discrepency Arrays -> To store active and available discrepencies based on active rules
    public List<string> availableDocs = new List<string>();
    public List<string> activeDocsWithDisc = new List<string>();
    public List<string> activeDisc = new List<string>();

    [Header("Documents")]
    public GameObject Passport;
    public GameObject ArrivalCard;
    public GameObject Visa;
    public GameObject Ticket;
    public GameObject EntryApproval;

    [Header("Randomized Stats")]
    public Lexic.NameGenerator nameGenerator;
    public string[] countryNames = { "Earth", "Not Earth", "Sort of Earth", "Ew Earth" };

    private void Update()
    {
        if (!GameManager.GetInstance.dayActive) //when day not active -> don't execute code
        {
            //Debug.Log("hello");
            return;
        }

        if (travellerActive) //when traveler is active -> don't execute code
        {
            return;
        }

        SpawnTraveller();
    }

    public void SpawnTraveller()
    {
        travellerActive = true;
        //Set Random Species Index
        TravellerSpeciesInstanceIndex = Random.Range(0, 4);
        nameGenerator.nameListIndex = TravellerSpeciesInstanceIndex;// for setting temp name

        //Fill Available documents array, after clearing available and activeDocsWithDisc arrays
        availableDocs.Clear();
        activeDocsWithDisc.Clear();
        foreach (KeyValuePair<string, bool> rule in GameManager.GetInstance.ruleBook)
        {
            if (rule.Value == true)
            {
                availableDocs.Add(rule.Key);
            }
        }

        TravellerPrefabsInstance = Instantiate(TravellerPrefabs[TravellerSpeciesInstanceIndex], spawnPoint.position, TravellerPrefabs[TravellerSpeciesInstanceIndex].transform.rotation);
        
        RetrieveStatParameters();
    }

    public void nextTraveller()
    {
        //Destroy previous Traveler
        if (TravellerPrefabsInstance != null)
        {
            Destroy(TravellerPrefabsInstance.gameObject);
        }

        travellerActive = false;
    }

    private void RetrieveStatParameters()
    {
        //Decide if traveller is valid or not
        validTraveller = Random.value > 0.5f;
        if (validTraveller)
        {
            //If Valid, set default stats for available docs and return;
            SetStats();

            Debug.Log("is valid traveler");
            return;
        }

        //INValid Traveller//
        Debug.Log("INvalid traveller");
        //Randomize active discrepencies, using minDisc + maxDisc
        int instanceDiscCount = Random.Range(minDisc, maxDisc + 1);
        //add validation check aganst rulebook to ensure that the index wont be out of bounds!!!!!!!!!!! to add later (might not be needed if using maxDisc++)

        //Using the availableDocs Array, randomly select discrepencies to add to the activeDocsWithDisc array
        for (int i = 0; i < instanceDiscCount;)
        {
            //if randomly selected does not already exist in activeDocsWithDisc array -> add it to activeDocsWithDisc array
            string temp = availableDocs[Random.Range(0, availableDocs.Count)];
            if (!activeDocsWithDisc.Contains(temp))
            {
                activeDocsWithDisc.Add(temp);
                i++;
            }
        }

        //Set Default Stats
        SetStats();

        //Set Discrepencies based on activeDocsWithDisc Array
        SetDiscrepencies();
    }

    private void SetStats()
    {
        //GENERATE Generic Default stats//
        //Name
        string travelerName = nameGenerator.GetNextRandomName();

        //Country - Based on Traveller Species Instance Index
        string travelerCountry = countryNames[TravellerSpeciesInstanceIndex];

        //DOB - DD//MM/YYYY
        string travelerDOB = string.Format("{0}/{1}/{2}", Random.Range(0, 20), Random.Range(1, 13), Random.Range(1946, 2046));

        //Sex
        string travelerSex = "F";
        if (Random.value > 0.5f)
        {
            travelerSex = "M";
        }

        //Passport//
        if (availableDocs.Contains("validPassport"))
        {
            //Name
            Passport.transform.GetChild(0).GetComponent<TMP_Text>().text = travelerName;
            DialogueManager.GetInstance.currentStory.variablesState["NAME"] = travelerName;
            //Country
            Passport.transform.GetChild(1).GetComponent<TMP_Text>().text = travelerCountry;
            DialogueManager.GetInstance.currentStory.variablesState["Country"] = travelerCountry;

            //DOB
            Passport.transform.GetChild(2).GetComponent<TMP_Text>().text = travelerDOB;
            DialogueManager.GetInstance.currentStory.variablesState["DOB"] = travelerDOB;

            //Sex
            Passport.transform.GetChild(3).GetComponent<TMP_Text>().text = travelerSex;

            //PassNum - PLS FIX does not generate 7 digit number
            Passport.transform.GetChild(4).GetComponent<TMP_Text>().text = string.Format("{0}{1}", (char)('A' + Random.Range(0, 26)), Random.Range(0, 99999));

            //DEAL WITH LATER - CORRECT ISSUE AND EXPIRE DATE
            //PassIssueDate
            Passport.transform.GetChild(5).GetComponent<TMP_Text>().text = string.Format("{0}/{1}/{2}", Random.Range(0, 20), Random.Range(1, 13), Random.Range(1946, 2046));

            //PassExpiryDate
            Passport.transform.GetChild(6).GetComponent<TMP_Text>().text = string.Format("{0}/{1}/{2}", Random.Range(0, 20), Random.Range(1, 13), Random.Range(1946, 2046));
        }

        /*//Arrival//
        if (availableDocs.Contains("validArrivalCard"))
        {
            //Name
            ArrivalCard.transform.GetChild(0).GetComponent<TMP_Text>().text = travelerName;

            //Country
            ArrivalCard.transform.GetChild(1).GetComponent<TMP_Text>().text = travelerCountry;

            //DOB
            ArrivalCard.transform.GetChild(2).GetComponent<TMP_Text>().text = travelerDOB;

            //Sex
            ArrivalCard.transform.GetChild(3).GetComponent<TMP_Text>().text = travelerSex;

            //DateOfArrival - Be the same as in-game date
            //DateOfDeparture - After in-game date

            //ModeOfTransport - Checkpoint at Airport
            //ArrivalCard.transform.GetChild(?).GetComponent<TMP_Text>().text = "Airplane";

            //TypeOfAccommodation - Based on traveller type (Student, Worker, VIP, Tourist)
            //ArrivalCard.transform.GetChild(?).GetComponent<TMP_Text>().text =;

            //LastCity / PortOfEmbarkment - For now use travelerCountry, potentially add cities in country to use
            //ArrivalCard.transform.GetChild(?).GetComponent<TMP_Text>().text = travelerCountry;
        }

        //Visa//
        if (availableDocs.Contains("validVisa"))
        {
            //Name
            Visa.transform.GetChild(0).GetComponent<TMP_Text>().text = travelerName;

            //Country
            Visa.transform.GetChild(1).GetComponent<TMP_Text>().text = travelerCountry;

            //DOB
            Visa.transform.GetChild(2).GetComponent<TMP_Text>().text = travelerDOB;

            //Sex
            Visa.transform.GetChild(3).GetComponent<TMP_Text>().text = travelerSex;

            //VisaNum - Same Format as Passport Number

            //VisaIssueDate

            //VisaExpiryDate

            //PeriodOfStay
            //VisaType
            //Remarks - Can randomize froma  remark list / should be blank (?)
        }

        //Entry Approval//
        if (availableDocs.Contains("validEntryApproval"))
        {
            //Diplomatic Relations thing - Use for Story Events
            //Pandemic Safety Regulation Thing
        }*/
    }

    private void SetDiscrepencies()
    {
        //check related documents in activeDocsWithDisc (eg.valid passport), randomize which discrepencies are active

        //Valid Passport
        if (activeDocsWithDisc.Contains("validPassport"))
        {
            Debug.Log("discrepency in passport");
            for (int j = 0; j < minDiscInDoc;) //to ensure at least minDiscInDoc number of error(s) is present
            {
                //RANDOMIZER -> "in___" inconsistent/invalid//

                //inName
                if (Random.value > 0.5f)
                {
                    Debug.Log("inName");
                    //Display Inconsistent Name
                    //Insert Appropriate Inky Script
                    DialogueManager.GetInstance.currentStory.variablesState["validName"] = "false";
                    DialogueManager.GetInstance.currentStory.variablesState["NAME"] = nameGenerator.GetNextRandomName();
                    //Add to activeDisc array (might not need to)
                    j++;
                }

                //inCountry
                if (Random.value > 0.5f)
                {
                    Debug.Log("inCountry");
                    //Display Inconsistent/Invalid City
                    //Insert Appropriate Inky Script
                    DialogueManager.GetInstance.currentStory.variablesState["validcountry"] = "false";
                    DialogueManager.GetInstance.currentStory.variablesState["Country"] = "Japan";
                    j++;
                }

                //inDOB
                if (Random.value > 0.5f)
                {
                    Debug.Log("inDOB");
                    //Display Inconsistent/Invalid Date of Birth
                    //Insert Appropriate Inky Script
                    DialogueManager.GetInstance.currentStory.variablesState["validDOB"] = "false";
                    DialogueManager.GetInstance.currentStory.variablesState["DOB"] = string.Format("{0}/{1}/{2}", Random.Range(0, 20), Random.Range(1, 13), Random.Range(1946, 2046));
                    j++;
                }

                //inPhoto
                if (Random.value > 0.5f)
                {
                    Debug.Log("inPhoto");
                    //Display Inconsistent Photo
                    //Insert Appropriate Inky Script
                    j++;
                }

                //inSex
                if (Random.value > 0.5f)
                {
                    Debug.Log("inSex");
                    //Display Inconsistent Sex
                    //Insert Appropriate Inky Script
                    j++;
                }

                //inPassNum
                if (Random.value > 0.5f)
                {
                    Debug.Log("inPassNum");
                    //Display Inconsistent/Invalid Passport Number
                    //Insert Appropriate Inky Script
                    j++;
                }

                //inExpire
                if (Random.value > 0.5f)
                {
                    Debug.Log("inExpire");
                    //Display Inconsistent/Invalid Expiry Date
                    //Insert Appropriate Inky Script
                    j++;
                }

            }
        }

        //Valid Arrival Card
        if (activeDocsWithDisc.Contains("validArrivalCard"))
        {
            //Debug.Log("discrepency in arrival card");
        }

        //Valid Visa

        //Valid Ticket

        //Valid Proof of Prior Entry Approval
    }
}
