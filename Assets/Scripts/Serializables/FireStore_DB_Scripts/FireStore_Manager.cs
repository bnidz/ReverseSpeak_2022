using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
public class FireStore_Manager : MonoBehaviour
{

    [SerializeField] public string liveWordsPath = "live_words/";
    [SerializeField] public string configsPath = "configs/default";
    [SerializeField] public string playersPath = "players/";
    [SerializeField] public string leaderboardsPath = "leaderboards/";

    //    [SerializeField] public string _Path = "";
    //    [SerializeField] public string _Path = "";
    //    [SerializeField] public string _Path = "";

    public void GetData_Word(string word)
    {
        var firestore = FirebaseFirestore.DefaultInstance;
        firestore.Document(liveWordsPath + Components.c.settings.currentPlayer.playerLocale + word).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if(task.IsFaulted) {
            // Handle the error...
            }
            else if (task.IsCompleted) {
            Word newWord = task.Result.ConvertTo<Word>();
            }
        });
    }

    public void GetData_Player(string UID)
    {
        var firestore = FirebaseFirestore.DefaultInstance;
        firestore.Document(playersPath + UID).GetSnapshotAsync().ContinueWithOnMainThread (task =>
        {
            if(task.IsFaulted) {
            // Handle the error...
            }
            else if (task.IsCompleted) {
            PlayerClass newWord = task.Result.ConvertTo<PlayerClass>();
            }
        });
    }

    public void GetData_Configs()
    {
        var firestore = FirebaseFirestore.DefaultInstance;
        firestore.Document (configsPath).GetSnapshotAsync().ContinueWithOnMainThread (task =>
        {
            if(task.IsFaulted) {
            // Handle the error...
            }
            else if (task.IsCompleted) {
            GameConfigs conf = task.Result.ConvertTo<GameConfigs>();
            }

        });
    }

    public void GetData_Default_Player()
    {
        var firestore = FirebaseFirestore.DefaultInstance;
        firestore.Document ("default_player").GetSnapshotAsync().ContinueWithOnMainThread (task =>
        {
            if(task.IsFaulted) {
            // Handle the error...
            }
            else if (task.IsCompleted) {
            PlayerClass def = task.Result.ConvertTo<PlayerClass>();
            }
        });
    }

    public void Update_LB(PlayerClass p)
    {
        var firestore = FirebaseFirestore.DefaultInstance;
        var lb_e = new LeaderBoard_entry
        {
            p_DisplayName = p.playerName,
            p_score = Components.c.settings.localeScore,
            UID = Components.c.settings.currentPlayer.UID
        };
        firestore.Document(leaderboardsPath + Components.c.settings.currentPlayer.playerLocale)
        .SetAsync(lb_e, SetOptions.MergeAll);
    }

    public void Update_WordData(WordClass w)
    {
        var firestore = FirebaseFirestore.DefaultInstance;

        firestore.Document(liveWordsPath + Components.c.settings.currentPlayer.playerLocale)
        .SetAsync(w, SetOptions.MergeAll);
    }


    ////// UTILITY FUNCTIONS ----------------------------------------------

    public IEnumerator upload_All_cur_locale_WORDS()
    {
        for (int i = 0; i < Components.c.settings.gameWords.Count; i++)
        {
            Update_WordData(Components.c.settings.gameWords[i]);
            yield return new WaitForSeconds(.02f);
        }
    }
    

}

[FirestoreData]
public struct LeaderBoard_entry
{
    [FirestoreProperty] public string p_DisplayName{get; set;}
    [FirestoreProperty] public int p_score{get; set;}
    [FirestoreProperty] public byte[] UID{get; set;}
}

[FirestoreData]
public struct Word
{
    [FirestoreProperty] public string word {get; set;}
    [FirestoreProperty] public int times_tried {get; set;}
    [FirestoreProperty] public int times_skipped {get; set;}
    [FirestoreProperty] public int times_right {get; set;}
    [FirestoreProperty] public float total_score {get; set;}
    [FirestoreProperty] public float avg_score {get; set;}
    [FirestoreProperty] public int tier {get; set;}

}

[FirestoreData]
public struct Player
{
    [FirestoreProperty] public string   playerName{get; set;}
    [FirestoreProperty] public string   playerID{get; set;}
    [FirestoreProperty] public int      skillLevel{get; set;}
    [FirestoreProperty] public int      playTimesCount{get; set;}
    [FirestoreProperty] public string   playerLocale{get; set;}
    [FirestoreProperty] public int      timesSkipped{get; set;}
    [FirestoreProperty] public int      timesQuessed{get; set;}
    [FirestoreProperty] public int      totalTries{get; set;}
    [FirestoreProperty] public int      totalScore{get; set;}
    [FirestoreProperty] public int      enUS_score{get; set;}
    [FirestoreProperty] public int      fiFI_score{get; set;}
    [FirestoreProperty] public int      frFR_score{get; set;}
    [FirestoreProperty] public int      deDE_score{get; set;}
    [FirestoreProperty] public float    avgScore{get; set;}
    [FirestoreProperty] public int      current_Hearts{get; set;}
    [FirestoreProperty] public int      current_Skips{get; set;}
    [FirestoreProperty] public string   lastlogin{get; set;}
    [FirestoreProperty] public byte[]   UID{get; set;}
    [FirestoreProperty] public int      multiplier{get; set;}
    [FirestoreProperty] public int      shield_count{get; set;}
    [FirestoreProperty] public int playerMaxMultiplier{get; set;}
}

[FirestoreData]
public struct Configs
{
    [FirestoreProperty] public int max_Skip_Amount{get; set;}
    [FirestoreProperty] public int max_Hearts{get; set;}
    [FirestoreProperty] public int configVersion{get; set;}
    [FirestoreProperty] public int skip_CoolDown{get; set;}
    [FirestoreProperty] public int heart_CoolDown{get; set;}
    [FirestoreProperty] public int ad_heart_reward{get; set;}
    [FirestoreProperty] public int ad_skip_reward{get; set;}
}


