using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Text;
public class FireStore_Manager : MonoBehaviour
{

    [SerializeField] public string liveWordsPath = "words/live_words/";
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

    public Player GetData_Player(string pID)
    {
        Player p = new Player();
        var firestore = FirebaseFirestore.DefaultInstance;
        firestore.Document(playersPath + pID).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if(task.IsFaulted) {
            // Handle the error...
            Debug.Log("vittu error -------------------------------------------------------------------------");
            }
            else if (task.IsCompleted) {
            p = task.Result.ConvertTo<Player>();
            }
        });

        return p;
    }

    public void GetData_Configs()
    {
        var firestore = FirebaseFirestore.DefaultInstance;
        firestore.Document(configsPath).GetSnapshotAsync().ContinueWithOnMainThread(task =>
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
        firestore.Document("default_player").GetSnapshotAsync().ContinueWithOnMainThread(task =>
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
        firestore.Document(leaderboardsPath + "all_time/" + Components.c.settings.currentPlayer.playerLocale + "/" + p.playerID)
        .SetAsync(lb_e, SetOptions.MergeAll);
    }

    public void Save_Player_to_DB(Player p)
    {
        var firestore = FirebaseFirestore.DefaultInstance;
        firestore.Document(playersPath + p.playerID)
        .SetAsync(p, SetOptions.MergeAll);

        Debug.Log("player upload to firestore ");
    }

    public void Update_WordData(Word w)
    {

        var firestore = FirebaseFirestore.DefaultInstance;
        firestore.Document("words/live_words/" + Components.c.settings.currentPlayer.playerLocale + "/" + (w.word.ToString()))
        .SetAsync(w, SetOptions.MergeAll);

    }


    public void Upload_Configs(Configs w)
    {
        var firestore = FirebaseFirestore.DefaultInstance;
        firestore.Document("configs/default/")
        .SetAsync(w, SetOptions.MergeAll);
    }

    public void Upload_DefaultPlayer(Player w)
    {
        var firestore = FirebaseFirestore.DefaultInstance;
        firestore.Document("default_player/default/")
        .SetAsync(w, SetOptions.MergeAll);
    }


    ////// UTILITY FUNCTIONS ----------------------------------------------
    public void Upload_all_worsd()
    {
        StartCoroutine(upload_All_cur_locale_WORDS());
    }
    public IEnumerator upload_All_cur_locale_WORDS()
    {
        for (int i = 0; i < Components.c.settings.gameWords.Count; i++)
        {
            Update_WordData(WordClassToWord(Components.c.settings.gameWords[i]));
            yield return new WaitForSeconds(.005f);
        }
    }




    public Word WordClassToWord(WordClass w)
    {
        var word = new Word {

           word = w.word,
           times_tried = w.times_tried,
           times_skipped = w.times_skipped,
           times_right = w.times_right,
           total_score = w.total_score,
           tier = w.tier,
           set = w.set,

        };
        return word;
    }

    public Player PlayerClassToPlayer(PlayerClass p)
    {
        var _p = new Player{

        playerName = p.playerName,
        playerID = p.playerID,
        skillLevel = p.skillLevel,
        playTimesCount = p.playTimesCount,
        playerLocale = p.playerLocale,
        timesSkipped = p.timesSkipped,
        timesQuessed = p.timesQuessed,
        totalTries = p.totalTries,
        totalScore = p.totalScore,
        enUS_score = p.enUS_score,
        fiFI_score = p.fiFI_score,
        frFR_score = p.frFR_score,
        deDE_score = p.deDE_score,
        avgScore = p.avgScore,
        current_Hearts = p.current_Hearts,
        current_Skips = p.current_Skips,
        lastlogin = p.lastlogin,
        UID = p.UID,
        multiplier = p.multiplier,
        shield_count = p.shield_count,
        playerMaxMultiplier = p.playerMaxMultiplier,
        };
        return _p;
    }

    public Configs GameConfigsToConfigs(GameConfigs gc)
    {

        var n = new Configs{

            max_Skip_Amount  =  gc.max_Skip_Amount,
            max_Hearts  =  gc.max_Hearts,
            configVersion  =  gc.configVersion,
            skip_CoolDown  =  gc.skip_CoolDown,
            heart_CoolDown  =  gc.heart_CoolDown,
            ad_heart_reward  =  gc.ad_heart_reward,
            ad_skip_reward  =  gc.ad_skip_reward,
        };

        return n;
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
    [FirestoreProperty] public int set{get; set;}

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
    [FirestoreProperty] public int      playerMaxMultiplier{get; set;}
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


