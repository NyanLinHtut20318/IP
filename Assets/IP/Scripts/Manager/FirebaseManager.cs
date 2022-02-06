using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;

public class FirebaseManager : MonoBehaviour
{
    DatabaseReference mDataBaseRef;

    //stat display
    public TMP_Text TotalScoreDisplay;
    public TMP_Text HighScoreDisplay;
    public TMP_Text TotalTimeDisplay;
    public TMP_Text HighTimeDisplay;

    //leaderbard display
    public GameObject rowPrefab;
    public Transform tableOfContent;
    //public GameObject[] leaderBoardList;
    public void Awake()
    {
        mDataBaseRef = FirebaseDatabase.DefaultInstance.RootReference;

        /*//refill data in database (for testing)
        WriteNewPlayer("Rank 1", "Dan", 2000);
        WriteNewPlayer("Rank 2", "Kirdesh", 1900);
        WriteNewPlayer("Rank 3", "Jonathan", 1870);
        WriteNewPlayer("Rank 4", "Lin", 1790);
        WriteNewPlayer("Rank 5", "Zavier", 1660);*/
    }

    /*public void WriteNewPlayer(string PlayerRank, string name, int score)
    {
        string key = mDataBaseRef.Push().Key;
        Player player = new Player(PlayerRank, 0, 0, 0, score, name);
        string json = JsonUtility.ToJson(player);

        Debug.Log("json values" + json);

        //adding field
        mDataBaseRef.Child(PlayerRank).SetRawJsonValueAsync(json);
    }*/

    //adds player stats to database -> empty variables
    public void CreateNewPlayer(string userID)
    {
        Debug.Log("creating player stats");
        //Player player = new Player(0, 0, 0, 0);
        //string playerJson = JsonUtility.ToJson(player);

        //mDataBaseRef.Child("playerStats/" + userID).SetRawJsonValueAsync(playerJson);
    }

    //Add player to PLAYERLIST
    public void AddToPlayerList(string userID, string email, string password, string username)
    {
        Debug.Log("Adding player to playerList");
        //ExistUser user = new ExistUser(email, password, username);
        //string userJson = JsonUtility.ToJson(user);

        //mDataBaseRef.Child("playerList/" + userID).SetRawJsonValueAsync(userJson);
    }

    //update playerstats
    public void UpdateDatabase(string userID, int instanceScore, float instanceTime, string username)
    {
        Debug.Log("Man on the MOON");
        Query playerQuery = mDataBaseRef.Child("playerStats/" + userID);

        playerQuery.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if(task.IsCanceled || task.IsFaulted)
            {
                Debug.Log("Unable to update database");
            }

            else if (task.IsCompleted)
            {
                DataSnapshot playerStats = task.Result;
                //Update Stats
                //Player pStats = JsonUtility.FromJson<Player>(playerStats.GetRawJsonValue());
                //pStats.totalScore += instanceScore;
                //pStats.totalTime += instanceTime;

                ////Check for high score + time
                //if (instanceScore > pStats.highScore)
                //{
                //    pStats.highScore = instanceScore;
                //    //display congrats message

                //    //update score leaderboard
                //    mDataBaseRef.Child("Leaderboard").Child(userID).Child("highScore").SetValueAsync(instanceScore);
                //    mDataBaseRef.Child("Leaderboard").Child(userID).Child("username").SetValueAsync(username);
                //}

                //if (instanceTime > pStats.highTime)
                //{
                //    pStats.highTime = instanceTime;
                //    //display congrats message
                //}

                ////set new values to database
                //mDataBaseRef.Child("playerStats/" + userID).SetRawJsonValueAsync(JsonUtility.ToJson(pStats));
            }
        });
    }

    //Load Player Stats
    public void LoadStats(string userID)
    {
        Query playerStatsQuery = mDataBaseRef.Child("playerStats/" + userID);

        playerStatsQuery.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log("Unable to retrieve data from database " + task.Exception);
            }

            else if (task.IsCompleted)
            {
                DataSnapshot playerStats = task.Result;
                //Retrieve Stats
                //Player pStats = JsonUtility.FromJson<Player>(playerStats.GetRawJsonValue());

                ////Update Display
                //TotalScoreDisplay.text = string.Format("Total No. of Spaceships Eaten: {0}", pStats.totalScore.ToString());
                //HighScoreDisplay.text = string.Format("Highest No. of Spaceships Eaten in One Sitting: {0}", pStats.highScore.ToString());
                //TotalTimeDisplay.text = string.Format("Total Time Feasting: {0}", pStats.totalTime.ToString());
                //HighTimeDisplay.text = string.Format("Longest Time Feasting: {0}", pStats.highTime.ToString());
            }
        });
    }

    //Load Leaderboard Stats
    public void LoadLeaderboard()
    {
        Query leaderboardQuery = mDataBaseRef.Child("Leaderboard").OrderByChild("highScore").LimitToLast(5);

        //List<leaderboardEntry> leaderboardEntries = new List<leaderboardEntry>();
        //leaderboardQuery.GetValueAsync().ContinueWithOnMainThread(task =>
        //{
        //    if (task.IsCanceled || task.IsFaulted)
        //    {
        //        Debug.Log("Unable to retrieve data from database " + task.Exception);
        //    }
        //    else if (task.IsCompleted)
        //    {
        //        DataSnapshot leaderboardStats = task.Result;

        //        int rankCounter = 1;
        //        foreach (DataSnapshot leaderboardPlayer in leaderboardStats.Children)
        //        {
        //            leaderboardEntry lb = JsonUtility.FromJson<leaderboardEntry>(leaderboardPlayer.GetRawJsonValue());

        //            leaderboardEntries.Add(lb);
        //            Debug.LogFormat("Leaderboard: Rank{0} Username: {1} HighScore: {2}", rankCounter, lb.username, lb.highScore);
        //        }

        //        leaderboardEntries.Reverse();

        //        //destroy row entries in leaderboard
        //        foreach(Transform rowEntry in tableOfContent)
        //        {
        //            Destroy(rowEntry.gameObject);
        //        }

        //        //append to list
        //        foreach (leaderboardEntry lb in leaderboardEntries)
        //        {
        //            //new row gameObect
        //            GameObject UpdatedEntry = Instantiate(rowPrefab, tableOfContent);
        //            TextMeshProUGUI[] entryDetails = UpdatedEntry.GetComponentsInChildren<TextMeshProUGUI>();
        //            entryDetails[0].text = rankCounter.ToString();
        //            entryDetails[1].text = lb.username.ToString();
        //            entryDetails[2].text = lb.highScore.ToString();
        //            rankCounter++;
        //        }
        //    }
        //});
    }
}
