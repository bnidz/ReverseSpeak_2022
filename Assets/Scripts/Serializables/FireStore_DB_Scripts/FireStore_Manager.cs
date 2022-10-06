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
        firestore.Document(liveWordsPath + Components.c.settings.thisPlayer.playerLocale + word).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if(task.IsFaulted) {
            // Handle the error...
            }
            else if (task.IsCompleted) {
            Word newWord = task.Result.ConvertTo<Word>();
            }
        });
    }

    public Player fs_Player;
    public void GetData_Player(string pID)
    {
       // fs_Player = new Player();
        var firestore = FirebaseFirestore.DefaultInstance;
        firestore.Document(playersPath + pID).GetSnapshotAsync().ContinueWith(task =>
        {
            if(task.IsFaulted) {
            // Handle the error...
            
            }
            else if (task.IsCompleted) {
            if(!task.Result.Exists)
            {
                Debug.Log("returnnn DOES NOT EXIST :D :O :ooo ");
                Components.c.gameManager.player_DB_save = false;
                isDone = true;
            }
            // if(JsonUtility.ToJson(task.Result).Length < 4)
            // {
            //     Debug.Log("returnnn nulll :ooo ");
            // }
            Components.c.settings.thisPlayer = task.Result.ConvertTo<Player>();
            Debug.Log("uouerh" + fs_Player.playerName);

           // = fs_Player;
            Components.c.gameManager.player_DB_save = true;
            isDone = true;
            }
        });

        //return p;
    }
    public bool isDone = false;
    public void GetConfigs()
    {
        Configs conf = new Configs();
        var firestore = FirebaseFirestore.DefaultInstance;
        firestore.Document(configsPath).GetSnapshotAsync().ContinueWith(task =>
        {
            if(task.IsFaulted) {
            // Handle the error...
            Debug.Log("errooorororoorororor geting def congigs");
            }
            else if (task.IsCompleted) {
                conf = task.Result.ConvertTo<Configs>();
                Components.c.settings.thisConfigs = conf;
                isDone = true;
            }
        });
    }

    public void GetData_Default_Player()
    {
        //Player def = new Player();
        var firestore = FirebaseFirestore.DefaultInstance;

        firestore.Document("default_player/default").GetSnapshotAsync().ContinueWith(task =>
        {
            if(task.IsFaulted) {
            // Handle the error...
            Debug.Log("errrooroooror fetching def player ");
            }
            else if (task.IsCompleted) {
            Components.c.settings.thisPlayer = task.Result.ConvertTo<Player>();
            isDone = true;
            }
        });
        //return def;
    }

    public void Update_LB(Player p)
    {
        var firestore = FirebaseFirestore.DefaultInstance;
        var lb_e = new LeaderBoard_entry
        {
            p_DisplayName = p.playerName,
            p_score = Components.c.settings.localeScore,
            UID = p.UID
        };
        firestore.Document(leaderboardsPath + "all_time/" + Components.c.settings.thisPlayer.playerLocale + "/" + p.playerID)
        .SetAsync(lb_e, SetOptions.MergeAll);
    }

    public void Save_Player_to_DB(Player p)
    {
        var firestore = FirebaseFirestore.DefaultInstance;
        firestore.Document(playersPath + p.playerID)
        .SetAsync(p, SetOptions.MergeAll);
        Debug.Log("player upload to firestore ");
    }

    public void Update_WordData(WordClass w)
    {   
        Word n = WordClassToWord(w);
        var firestore = FirebaseFirestore.DefaultInstance;
        firestore.Document("words/live_words/" + Components.c.settings.thisPlayer.playerLocale + "/" + (w.word.ToString()))
        .SetAsync(n, SetOptions.MergeAll);
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

    public void PopulateLeaderBoards()
    {
        StartCoroutine(LB_pop());
    }
    private string n_name;
    public IEnumerator LB_pop()
    {
        var firestore = FirebaseFirestore.DefaultInstance;

        for (int i = 0; i < 200; i++)
        {
            for(int x=0; x <10; x++)
            {
                n_name += glyphs[UnityEngine.Random.Range(0, glyphs.Length)];
            }
            var e = new LeaderBoard_entry
            {
                p_DisplayName = n_name,
                p_score = UnityEngine.Random.Range(100, 100000),
                UID = GenerateUUID.UUID(),
            };
            firestore.Document(leaderboardsPath + "all_time/" + Components.c.settings.thisPlayer.playerLocale + "/" + e.UID)
            .SetAsync(e, SetOptions.MergeAll);
            yield return new WaitForSeconds(.01f);
        }
    }
    const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";
    ////// UTILITY FUNCTIONS ----------------------------------------------
    public void Upload_all_worsd()
    {
        //StartCoroutine(upload_All_cur_locale_WORDS());
    }
    // public IEnumerator upload_All_cur_locale_WORDS()
    // {
    //     for (int i = 0; i < Components.c.settings.gameWords.Count; i++)
    //     {
    //         Update_WordData(WordClassToWord(Components.c.settings.gameWords[i]));
    //         yield return new WaitForSeconds(.005f);
    //     }
    // }

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

[FirestoreData][System.Serializable]
public struct LeaderBoard_entry
{
    [FirestoreProperty] public string p_DisplayName{get; set;}
    [FirestoreProperty] public int p_score{get; set;}
    [FirestoreProperty] public byte[] UID{get; set;}
}

[FirestoreData][System.Serializable]
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

[FirestoreData][System.Serializable]
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

[FirestoreData][System.Serializable]
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


