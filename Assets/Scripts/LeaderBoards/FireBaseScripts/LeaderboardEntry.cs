using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using System;
using Firebase.Auth;
public class LeaderboardEntry {


    string pName;
    int pScore;
    string key;
    DatabaseReference mDatabase;

    void Start()
    {
        mDatabase = FirebaseDatabase.DefaultInstance.RootReference;
        key = mDatabase.Child("scores").Push().Key;



    }
    //public string uid;
    //public int score = 0;

    public LeaderboardEntry() {
    }

    public LeaderboardEntry(string pName, int pScore) {
        this.pName = pName;
        this.pScore = pScore;
    }

    public Dictionary<string, object> ToDictionary() {
        Dictionary<string, object> result = new Dictionary<string, object>();
        result["player_ID"] = pName;
        result["player_Score"] = pScore;

        return result;
    }
    private void WriteNewScore(string userId, int score) {
    // Create new entry at /user-scores/$userid/$scoreid and at
    // /leaderboard/$scoreid simultaneously
            pName = Components.c.settings.thisPlayer.playerName;
            pScore = Components.c.settings.thisPlayer.totalScore;
            LeaderboardEntry entry = new LeaderboardEntry(pName, pScore);
            Dictionary<string, object> entryValues = entry.ToDictionary();

            Dictionary<string, object> childUpdates = new Dictionary<string, object>();
            childUpdates["/scores/" + key] = entryValues;
            childUpdates["/user-scores/" + pScore + "/" + key] = entryValues;

            mDatabase.UpdateChildrenAsync(childUpdates);
    }

}

