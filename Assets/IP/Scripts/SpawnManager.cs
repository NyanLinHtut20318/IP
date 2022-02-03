using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Lexic;

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
    public GameObject[] TravellerPrefabs;
    private static GameObject TravellerPrefabsInstance = null;
    public int TravellerSpeciesInstanceIndex; //randomized species index
    [SerializeField]private Transform spawnPoint;
    [SerializeField]private bool travellerActive = false;

    [Header("Stat Parameters")]
    public int minDisc = 1; //set min and max documents with discrepencies allowed -> used to ramp up/adjust difficulty
    public int maxDisc = 1;
    public int minDiscInDoc = 1; //min num of discrepencies in a document -> used to ramp up/adjust difficulty

    //Set Discrepency Arrays -> To store active and available discrepencies based on active rules
    public List<string> availableDisc = new List<string>();
    public List<string> activeDiscParams = new List<string>();
    public List<string> activeDisc = new List<string>();

    [Header("Randomized Stats")]
    public TMP_Text nameBox;
    public Lexic.NameGenerator nameGenerator;

    private void Update()
    {
        if (!GameManager.GetInstance.dayActive) //when day not active -> don't execute code
        {
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
        TravellerSpeciesInstanceIndex = Random.Range(0, 2); //rmbr to change back to (0, 4)
        nameGenerator.nameListIndex = TravellerSpeciesInstanceIndex;// for setting temp name

        //Fill Available discrepency array, after clearing available and active disc arrays
        availableDisc.Clear();
        activeDiscParams.Clear();
        foreach (KeyValuePair<string, bool> rule in GameManager.GetInstance.ruleBook)
        {
            if (rule.Value == true)
            {
                availableDisc.Add(rule.Key);
            }
        }

        TravellerPrefabsInstance = Instantiate(TravellerPrefabs[TravellerSpeciesInstanceIndex], spawnPoint.position, TravellerPrefabs[TravellerSpeciesInstanceIndex].transform.rotation);
        //name gen
        nameBox.text = nameGenerator.GetNextRandomName();
        RetrieveStatParameters();
    }

    public void nextTraveller()
    {
        //Destroy previous Traveler
        if(TravellerPrefabsInstance != null)
        {
            Destroy(TravellerPrefabsInstance);
        }

        travellerActive = false;
    }

    private void RetrieveStatParameters()
    {
        //Decide if traveller is valid or not
        bool validTraveller = Random.value > 0.5f;
        if (validTraveller)
        {
            //If Valid
            //Get active rule parameters
            //Set appropriate values

            Debug.Log("is valid traveler");
            return;
        }

        //INValid Traveller//
        Debug.Log("INvalid traveller");
        //Randomize active discrepencies, using minDisc + maxDisc
        int instanceDiscCount = Random.Range(minDisc, maxDisc + 1);
        //add validation check aganst rulebook to ensure that the index wont be out of bounds!!!!!!!!!!! to add later (might not be needed if using maxDisc++)

        //Using the availableDisc Array, randomly select discrepencies to add to the activeDiscParams array
        for (int i = 0; i < instanceDiscCount;)
        {
            //if randomly selected does not already exist in activeDiscParams array -> add it to activeDiscParams array
            string temp = availableDisc[Random.Range(0, availableDisc.Count)];
            if (!activeDiscParams.Contains(temp))
            {
                activeDiscParams.Add(temp);
                i++;
            }
        }

        //Set Stats based on activeDiscParams Array
        SetStats();
    }

    private void SetStats()
    {
        Debug.Log("Setting Stats");


        //check related documents in activeDiscParams (eg.valid passport), randomize which discrepencies are active

        //Valid Passport
        if (activeDiscParams.Contains("validPassport"))
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

                    //Add to activeDisc array (might not need to)
                    j++;
                }

                //inCity
                if (Random.value > 0.5f)
                {
                    Debug.Log("inCity");
                    //Display Inconsistent/Invalid City
                    //Insert Appropriate Inky Script
                    j++;
                }

                //inDOB
                if (Random.value > 0.5f)
                {
                    Debug.Log("inDOB");
                    //Display Inconsistent/Invalid Date of Birth
                    //Insert Appropriate Inky Script
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
        if (activeDiscParams.Contains("validArrivalCard"))
        {
            Debug.Log("discrepency in arrival card");
        }

        //Valid Visa

        //Valid Ticket

        //Valid Proof of Prior Entry Approval
    }
}
