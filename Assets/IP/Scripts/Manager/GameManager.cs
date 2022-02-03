using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton
    private static GameManager gameManagerInstance;

    public static GameManager GetInstance { get { return gameManagerInstance; } }

    private void Awake()
    {
        if (gameManagerInstance != null && gameManagerInstance != this)
        {
            Debug.LogWarning("More than 1 GameManager in Scene");
            Destroy(gameObject);
        }
        else
        {
            gameManagerInstance = this;
        }
    }

    [Header("Day Instance Variables")]
    public bool dayActive = false;

    [Header("Days for Difficulty Change")]
    [SerializeField]private int currentDay = 0;
    public int DayDiff1 = 1; //when currentDay == daydiff1, add more rules/increase difficulty
    public int DayDiff2 = 2;

    private void Update()
    {
        //UpdateRules();
    }

    // RULEBOOK LIST //

    //Store active rules -> Set to true
    //Inactive rules -> Set to false
    public Dictionary<string, bool> ruleBook = new Dictionary<string, bool>
    { 
        //All Applicable
        {"validPassport", false},
        {"validArrivalCard", false},
        {"validVisa", false},
        {"validTicket", false},
        {"validEntryApproval", false}

        //Region Specific

        //Tourists
        //Workers
        //Students
        //VIPs/Ambassadors
    };

    public void StartDay()//Start of Day -> Update rulebook
    {
        dayActive = true;

        currentDay++;
        if (currentDay == DayDiff1)
        {//Actual rules to adjust to be decided next time
            //initialize active rules -> finds rule of the same name and sets rule to true
            ruleBook["validPassport"] = true;
            ruleBook["validArrivalCard"] = true;
        }

        if (currentDay == DayDiff2)
        {
            ruleBook["validVisa"] = true;
            SpawnManager.GetInstance.maxDisc++;
        }
    }
}
