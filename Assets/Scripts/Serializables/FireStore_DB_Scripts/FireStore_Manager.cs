using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Text;
using System;
using System.Globalization;
using System.IO;
public class FireStore_Manager : MonoBehaviour
{

    [SerializeField] public string liveWordsPath = "words/live_words/";
    [SerializeField] public string configsPath = "configs/default";
    [SerializeField] public string playersPath = "players/";
    [SerializeField] public string leaderboardsPath = "leaderboards/";

    //    [SerializeField] public string _Path = "";
    //    [SerializeField] public string _Path = "";
    //    [SerializeField] public string _Path = "";

 private Calendar myCal;
 private CultureInfo myCI;
 private CalendarWeekRule myCWR;
 private DayOfWeek myFirstDOW;


 public string lb_alltime_path;
 public string lb_year_path;
 public string lb_month_path;
 public string lb_week_path;

 public string week_lb_title;
 public string month_lb_title;
 public string year_lb_title;
 public string alltime_lb_title;
public List<string> _fireStoreloc_iOSloc; 
    public void Init()
    {

        myCI = new CultureInfo("en-US");
        myCal = myCI.Calendar;
        // Gets the DTFI properties required by GetWeekOfYear.
        myCWR = myCI.DateTimeFormat.CalendarWeekRule;
        myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;


        lb_week_path = myCal.GetWeekOfYear(DateTime.UtcNow, myCWR, myFirstDOW).ToString() + DateTime.UtcNow.ToString("_yyyy") + "/" + Components.c.settings.thisPlayer.playerLocale + "/";
        lb_month_path = DateTime.UtcNow.ToString("MMMM_yyyy") + "/" + Components.c.settings.thisPlayer.playerLocale + "/";
        lb_year_path = DateTime.UtcNow.ToString("yyyy") + "/" + Components.c.settings.thisPlayer.playerLocale + "/";
        lb_alltime_path = "all_time/" + Components.c.settings.thisPlayer.playerLocale + "/";


        week_lb_title = myCal.GetWeekOfYear(DateTime.UtcNow, myCWR, myFirstDOW).ToString() + " WEEK Leaderboads" + " - " + Components.c.settings.locale;
        month_lb_title = DateTime.UtcNow.ToString("MMMM") + " Leaderboads" + " - " + Components.c.settings.locale;
        year_lb_title = DateTime.UtcNow.ToString("yyyy")  + " Leaderboads"+ " - " + Components.c.settings.locale;
        alltime_lb_title = "All-time  Leaderboards"+ " - " + Components.c.settings.locale;

        _fireStoreloc_iOSloc = new List<string>(){

             {"en-US"},
             {"fi-FI"},
             {"fr-FR"},
             {"de-DE"},
             {"ar-AE"},
             {"ca-ES"},
             {"cs-CZ"},
             {"da-DK"},
             {"es-ES"},
             {"iw-IL"},
             {"hi-IN"},
             {"hr-HR"},
             {"hu-HU"}, 
             {"id-ID"},
             {"it-IT"},
             {"ja-JP"},
             {"ko-KR"},
             {"ms-MY"},
             {"nl-NL"},
             {"no-NO"},
             {"pl-PL"},
             {"ro-RO"},
             {"ru-RU"},
             {"sk-SK"},
             {"sv-SE"},
             {"th-TH"},
             {"tr-TR"},
             {"uk-UA"},
             {"vi-VN"},

        };
        locDic = new Dictionary<string, string>(){

             {"en-US", "en"},
             {"fi-FI", "fi"},
             {"fr-FR", "fr"},
             {"de-DE", "de"},
             {"ar-AE", "ar"},
             {"ca-ES", "ca"},
             {"cs-CZ", "cs"},
             {"da-DK", "da"},
             {"es-ES", "es"},
             {"iw-IL", "he"},
             {"hi-IN", "hi"},
             {"hr-HR", "hr"},
             {"hu-HU", "hu"}, 
             {"id-ID", "id"},
             {"it-IT", "it"},
             {"ja-JP", "ja"},
             {"ko-KR", "ko"},
             {"ms-MY", "ms"},
             {"nl-NL", "nl"},
             {"no-NO", "no"},
             {"pl-PL", "pl"},
             {"ro-RO", "ro"},
             {"ru-RU", "ru"},
             {"sk-SK", "sk"},
             {"sv-SE", "sv"},
             {"th-TH", "th"},
             {"tr-TR", "tr"},
             {"uk-UA", "uk"},
             {"vi-VN", "vi"},
        };  
    }

    public Dictionary<string, string> locDic;
    public Dictionary<string, string> fireStoreloc_iOSloc;

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
    public bool isDoneConfigs = false;
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
                isDoneConfigs = true;
            }
        });
    }
    public bool isDoneDefPlayer = false;
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
            Components.c.settings.thisPlayer = new Player();
            Components.c.settings.thisPlayer = task.Result.ConvertTo<Player>();
            isDoneDefPlayer = true;
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
            UID = p.UID,
            scoreUploaded = FieldValue.ServerTimestamp,
            
        };
        //all time
        firestore.Document(leaderboardsPath + "all_time/" + Components.c.settings.locale + "/" + Components.c.settings.thisPlayer.playerID)
        .SetAsync(lb_e, SetOptions.MergeAll);
        //year
        firestore.Document(leaderboardsPath + DateTime.UtcNow.ToString("yyyy") + "/" + Components.c.settings.locale+ "/" + Components.c.settings.thisPlayer.playerID)
        .SetAsync(lb_e, SetOptions.MergeAll);
        //month
        firestore.Document(leaderboardsPath + DateTime.UtcNow.ToString("MMMM yyyy") + "/" + Components.c.settings.locale+ "/" + Components.c.settings.thisPlayer.playerID)
        .SetAsync(lb_e, SetOptions.MergeAll);
        //week
        firestore.Document(leaderboardsPath  + "Week " + myCal.GetWeekOfYear(DateTime.UtcNow, myCWR, myFirstDOW).ToString() + DateTime.UtcNow.ToString("_yyyy") + "/" + Components.c.settings.locale+ "/" + Components.c.settings.thisPlayer.playerID)
        .SetAsync(lb_e, SetOptions.MergeAll);

      StartCoroutine(DonaLB());
    }

    public IEnumerator DonaLB()
    {
        yield return new WaitForSeconds(1f);
        var firestore = FirebaseFirestore.DefaultInstance;
        firestore.Document(leaderboardsPath + "all_time/" + Components.c.settings.thisPlayer.playerLocale + "/" + Components.c.settings.thisPlayer.playerID).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if(task.IsFaulted) {
            // Handle the error...
            }
            else if (task.IsCompleted) {
            LeaderBoard_entry le = task.Result.ConvertTo<LeaderBoard_entry>();
            //timestamp = le.scoreUploaded;
            Debug.Log("TimeStamp from LB enty: " + parseMyTimestamp(le.scoreUploaded).ToString());
            Debug.Log("TimeStamp local UTCnow " + DateTime.UtcNow.ToString());
            Debug.Log("Week of the year " + myCal.GetWeekOfYear(DateTime.UtcNow, myCWR, myFirstDOW).ToString());
            Debug.Log("Month of the year " + DateTime.UtcNow.ToString("MMMM"));
            }
        });
    }

    public int score_locale_weekly;
    public int score_locale_monthly;
    public int score_locale_yearly;
    public int score_locale_all_time;

    public string thisWeek;
    public string thisMonth;
    public string thisYear;


    public IEnumerator DonaLB_values()
    {

        thisWeek = myCal.GetWeekOfYear(DateTime.UtcNow, myCWR, myFirstDOW).ToString();
        thisMonth = DateTime.UtcNow.ToString("MMMM");
        thisYear = DateTime.UtcNow.ToString("yyyy");


        yield return new WaitForSeconds(1f);
        var firestore = FirebaseFirestore.DefaultInstance;
        firestore.Document(leaderboardsPath + "all_time/" + Components.c.settings.thisPlayer.playerLocale + "/" + Components.c.settings.thisPlayer.playerID)
        .GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if(task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted) {

                if(!task.Result.Exists)
                {
                    var lb_e = new LeaderBoard_entry
                    {
                        p_DisplayName = Components.c.settings.thisPlayer.playerName,
                        p_score = 0,
                        UID = Components.c.settings.thisPlayer.UID,
                        scoreUploaded = FieldValue.ServerTimestamp,
                    };

                    firestore.Document(leaderboardsPath + "all_time/" + Components.c.settings.locale + "/" + Components.c.settings.thisPlayer.playerID)
                    .SetAsync(lb_e, SetOptions.MergeAll);


                    score_locale_all_time = 0;
                    return;
                }

                //all_time score to local variable
                LeaderBoard_entry le = task.Result.ConvertTo<LeaderBoard_entry>();
                score_locale_all_time = le.p_score;

            }
            
        });
        firestore.Document(leaderboardsPath + DateTime.UtcNow.ToString("yyyy") + "/" + Components.c.settings.locale+ "/" + Components.c.settings.thisPlayer.playerID)
        .GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if(task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted) {

                if(!task.Result.Exists)
                {
                    var lb_e = new LeaderBoard_entry
                    {
                        p_DisplayName = Components.c.settings.thisPlayer.playerName,
                        p_score = 0,
                        UID = Components.c.settings.thisPlayer.UID,
                        scoreUploaded = FieldValue.ServerTimestamp,
                    };

                    firestore.Document(leaderboardsPath + DateTime.UtcNow.ToString("yyyy") + "/" + Components.c.settings.locale+ "/" + Components.c.settings.thisPlayer.playerID)
                    .SetAsync(lb_e, SetOptions.MergeAll);


                    score_locale_yearly = 0;
                    return;
                }

                LeaderBoard_entry le = task.Result.ConvertTo<LeaderBoard_entry>();
                score_locale_yearly = le.p_score;
            }

        });
        firestore.Document(leaderboardsPath + DateTime.UtcNow.ToString("MMMM yyyy") + "/" + Components.c.settings.locale+ "/" + Components.c.settings.thisPlayer.playerID)
        .GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if(task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted) {

                if(!task.Result.Exists)
                {
                    var lb_e = new LeaderBoard_entry
                    {
                        p_DisplayName = Components.c.settings.thisPlayer.playerName,
                        p_score = 0,
                        UID = Components.c.settings.thisPlayer.UID,
                        scoreUploaded = FieldValue.ServerTimestamp,
                    };

                    firestore.Document(leaderboardsPath + DateTime.UtcNow.ToString("MMMM yyyy") + "/" + Components.c.settings.locale+ "/" + Components.c.settings.thisPlayer.playerID)
                    .SetAsync(lb_e, SetOptions.MergeAll);


                    score_locale_monthly = 0;
                    return;
                }


                LeaderBoard_entry le = task.Result.ConvertTo<LeaderBoard_entry>();
                score_locale_monthly = le.p_score;
            }

        });
        firestore.Document(leaderboardsPath + "Week " + myCal.GetWeekOfYear(DateTime.UtcNow, myCWR, myFirstDOW).ToString() + DateTime.UtcNow.ToString("_yyyy") + "/" + Components.c.settings.locale+ "/" + Components.c.settings.thisPlayer.playerID)
        .GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if(task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted) {

                if(!task.Result.Exists)
                {
                    var lb_e = new LeaderBoard_entry
                    {
                        p_DisplayName = Components.c.settings.thisPlayer.playerName,
                        p_score = 0,
                        UID = Components.c.settings.thisPlayer.UID,
                        scoreUploaded = FieldValue.ServerTimestamp,
                    };

                    firestore.Document(leaderboardsPath  + "Week " + myCal.GetWeekOfYear(DateTime.UtcNow, myCWR, myFirstDOW).ToString() + DateTime.UtcNow.ToString("_yyyy") + "/" + Components.c.settings.locale+ "/" + Components.c.settings.thisPlayer.playerID)
                    .SetAsync(lb_e, SetOptions.MergeAll);


                    score_locale_weekly = 0;
                    return;
                }

                LeaderBoard_entry le = task.Result.ConvertTo<LeaderBoard_entry>();
                score_locale_weekly = le.p_score;
            }

        });
    }
    private Timestamp timestamp;

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
    private int rank;
    public void Get_LB_local_top10(string lb_type)
    {
        var firestore = FirebaseFirestore.DefaultInstance;
        //Query q = 
        rank = 1;

        if(lb_type == "week")
        {
            
            firestore.Collection(leaderboardsPath + lb_week_path).OrderBy("p_score").LimitToLast(100).GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                if(task.IsFaulted) {
                // Handle the error...
                }
                else if (task.IsCompleted)
                {
                    foreach (DocumentSnapshot l in task.Result.Documents)
                    {
                        var le = l.ConvertTo<LeaderBoard_entry>();
                        Components.c.displayHighScores.AddToLB(rank, le.p_DisplayName, le.p_score.ToString());
                        rank++;
                    }
                }
            });
        }
        
        if(lb_type == "month")
        {
            firestore.Collection(leaderboardsPath + lb_month_path).OrderBy("p_score").LimitToLast(100).GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                if(task.IsFaulted) {
                // Handle the error...
                }
                else if (task.IsCompleted)
                {
                    foreach (DocumentSnapshot l in task.Result.Documents)
                    {
                        var le = l.ConvertTo<LeaderBoard_entry>();
                        Components.c.displayHighScores.AddToLB(rank, le.p_DisplayName, le.p_score.ToString());
                        rank++;
                    }
                }
            });
        }
        
        if(lb_type == "year")
        {

            firestore.Collection(leaderboardsPath + lb_year_path).OrderBy("p_score").LimitToLast(100).GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                if(task.IsFaulted) {
                // Handle the error...
                }
                else if (task.IsCompleted)
                {
                    foreach (DocumentSnapshot l in task.Result.Documents)
                    {
                        var le = l.ConvertTo<LeaderBoard_entry>();
                        Components.c.displayHighScores.AddToLB(rank, le.p_DisplayName, le.p_score.ToString());
                        rank++;
                    }
                }
            });
        }
        
        if(lb_type == "alltime")
        {
            firestore.Collection(leaderboardsPath + lb_alltime_path).OrderBy("p_score").LimitToLast(100).GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                if(task.IsFaulted) {
                // Handle the error...
                }
                else if (task.IsCompleted)
                {
                    foreach (DocumentSnapshot l in task.Result.Documents)
                    {
                        var le = l.ConvertTo<LeaderBoard_entry>();
                        Components.c.displayHighScores.AddToLB(rank, le.p_DisplayName, le.p_score.ToString());
                        rank++;
                    }
                }
            });
        }
    }

    public void Get_Rank()
    {
        var firestore = FirebaseFirestore.DefaultInstance;
        //Query q = 
        rank = 1;
        firestore.Collection(leaderboardsPath + "all_time/" + Components.c.settings.thisPlayer.playerLocale).OrderBy("p_score").WhereGreaterThan("p_score", Components.c.settings.localeScore).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if(task.IsFaulted) {
            // Handle the error...
            }
            else if (task.IsCompleted)
            {
                QuerySnapshot qs = task.Result;
                IEnumerable<DocumentSnapshot> t = qs.Documents;
                string d  = qs.Count.ToString();
                Debug.Log("RANK IS :" +d);
            }
        });
    }
public DateTime parseMyTimestamp(object ts) {
    // Return the time converted into UTC 
    return ((Timestamp)ts).ToDateTime().ToUniversalTime();
}

    public bool donaRankdone = false;
    public void Get_Daily_ScoreList_for_Rank()
    {
        //checkk if betterscores exists
        //if so-- compare times
        //if ok -- use that --- 
        //if not --- dona new 

        var firestore = FirebaseFirestore.DefaultInstance;
        //Wrapping_LB rankList = new Wrapping_LB();
        //Components.c.settings.locale_ranklist = new Wrapping_LB();
        firestore.Collection(leaderboardsPath + "all_time/" + Components.c.settings.thisPlayer.playerLocale).OrderBy("p_score").WhereGreaterThan("p_score", Components.c.settings.localeScore).GetSnapshotAsync().ContinueWith(task =>
        {
            if(task.IsFaulted) {
            // Handle the error...
            }
            else if (task.IsCompleted)
            {
                QuerySnapshot qs = task.Result;
                IEnumerable<DocumentSnapshot> t = qs.Documents;
                string d  = qs.Count.ToString();
                Debug.Log("RANK IS :" +d);

                foreach (DocumentSnapshot l in task.Result.Documents)
                {
                    var le = l.ConvertTo<LeaderBoard_entry>();
                    Components.c.settings.localeRankList.Add(le.p_score);
                }
                Debug.Log("rank count " + Components.c.settings.localeRankList.Count);
                donaRankdone = true;
            }
        });
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
            firestore.Document(leaderboardsPath + lb_month_path + e.p_DisplayName)
            .SetAsync(e, SetOptions.MergeAll);
      
            firestore.Document(leaderboardsPath + lb_year_path + e.p_DisplayName)
            .SetAsync(e, SetOptions.MergeAll);
      
            firestore.Document(leaderboardsPath + lb_week_path + e.p_DisplayName)
            .SetAsync(e, SetOptions.MergeAll);
          
            firestore.Document(leaderboardsPath + lb_alltime_path + e.p_DisplayName)
            .SetAsync(e, SetOptions.MergeAll);
            n_name = "";
            yield return new WaitForSeconds(.01f);
        }
    }
    


    const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";
    ////// UTILITY FUNCTIONS ----------------------------------------------
    public void Upload_all_worsd()
    {
       // StartCoroutine(upload_all_baseword_locales());
       //Upload_variables_to_FireStore();
       DonaUI_translations();
    }

    public IEnumerator upload_All_cur_locale_WORDS(List<Word> words)
    {
        for (int i = 0; i < words.Count; i++)
        {
           // Upload_WordData(words[i]);
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
    //private List<Word> wordlist = new List<Word>();

    public IEnumerator upload_all_baseword_locales()
    {

        foreach (string loc in _fireStoreloc_iOSloc)
        {
            Components.c.filereader.isDoing = true;
            Debug.Log("key val = " +  loc.ToString());
            Components.c.filereader.MakeNewWordItems(loc.ToString());
            while(Components.c.filereader.isDoing == true) yield return null;

        }
    }

   // private Wrapping_Word ww;
    private List<Word> wordList;// = new List<Word>();
    List<string>[] localesListArray = new List<string>[30];
    private bool loc_in_progress = true;
    private string wörds;
    public void GetData_translated_Words(string firestore_locale, string ios_locale)
    {
        wordList = new List<Word>();
     //  // loc_in_progress = true;
        var firestore = FirebaseFirestore.DefaultInstance;
        firestore.Collection("words/base_words/" + ios_locale).GetSnapshotAsync().ContinueWith(task =>
        {

         //firestore.Collection("words/eng_root_10k/" + "base_words").GetSnapshotAsync().ContinueWithOnMainThread(task =>
         //{
            localesListArray[_locale] = new List<string>();
               
               var ww = new Wrapping_Word{
                    WordSet = wordList,
                };
            //localised_words.Clear();
            //wordlist = new List<Word>();
            if(task.IsFaulted) {
            // Handle the error...
            }
            else if (task.IsCompleted) {


               // QuerySnapshot qs = task.Result;
                //IEnumerable<DocumentSnapshot> t = qs.Documents;
                //string d  = qs.Count.ToString();
                //Debug.Log("locale  IS :" + ios_locale + " number of words is :" + d);
                //loc_in_progress = false;
            Debug.Log("asdfghjk");

           //Debug.Log(task.Result.());
                
               // Wrapping_Word ww = new Wrapping_Word();
                foreach (DocumentSnapshot l in task.Result.Documents)
                {
                   Word w = l.ConvertTo<Word>();
                    wörds += w.word+"\n";
                }
                // string json = JsonUtility.ToJson(ww);
                File.WriteAllText(Application.persistentDataPath +"/"+ ios_locale +"_WordsJson.json", wörds); 

                //wordList.Clear();
                Debug.Log(Application.persistentDataPath +"/"+ ios_locale +"_WordsJson.json");

                wörds = "";
                loc_in_progress = false;

            //         Dictionary<string, string> trans;
            //         l.TryGetValue<Dictionary<string, string>>("localised", out trans);

            //         foreach (KeyValuePair<string, string> w in trans)
            //         {

            //             if(w.Key == firestore_locale)
            //             {
            //                 //Debug.Log("KEY PAIR FOUND ---- " + w.Key +" "+ w.Value);
            //                 //Debug.Log("ios locale == " + ios_locale);
            //               //  string newWordfromFireStore_translation = w.Value;
    
            //                 localesListArray[_locale].Add(w.Value.ToString());
            //                // Upload_WordData(word, ios_locale);
            //                 Debug.Log("loc list array" + localesListArray[_locale].ToString());
            //                 Debug.Log("count" + localesListArray[_locale].Count.ToString());
            //             }
            //           // Debug.Log("trans" + w.Value + "loc " + w.Key);
            //         }
            //     }
            // StartCoroutine(uploadLocale_wordList(localesListArray[_locale], ios_locale));

            }


        });
    }

    private List<string> localised_words;
    public IEnumerator uploadLocale_wordList(List<string> words, string loc)
    {
        //Debug.Log("started loc " + loc + " upload");
        // if(loc == "fi-FI")
        // {
        //     for (int i = 6000; i < words.Count; i++)
        //     {
        //         Upload_WordData(words[i], loc);
        //         yield return new WaitForSeconds(.05f);
        //         //Debug.Log(words[i].ToString() + " uploaded tryna");
        //     }


        // }else{

            for (int i = 0; i < words.Count; i++)
            {
                Upload_WordData(words[i], loc);
                yield return new WaitForSeconds(.05f);
                //Debug.Log(words[i].ToString() + " uploaded tryna");
            }

        //}



       
        loc_in_progress = false;
    }

    int _locale = 0;
    int wordGoing = 1;
    public void Upload_WordData(string w, string locale)
    {   
        var firestore = FirebaseFirestore.DefaultInstance;
        //firestore.Document("words/eng_root_10k/" + Components.c.settings.thisPlayer.playerLocale + "/" + (w.word.ToString()))
            var word = new Word {

            word = w,
            times_tried = 0,
            times_skipped =0,
            times_right = 0,
            total_score = 0,
            tier = 0,
            set = 0,

            };

        if(w.Length > 1)
        {
            firestore.Document("words/" + "base_words/" + locale +"/"+ w.ToString())
            .SetAsync(word, SetOptions.MergeAll);
        }

        wordGoing++;
        Debug.Log( locale.ToString() + " - " + _locale.ToString() + " / 30 " + " ...word of " + locale + " @ " + wordGoing.ToString());
        //Debug.Log(w.ToString() + " uploaded  ---- done ??");

    }
    public void Upload_locData(UI_transalations ui_i)
    {   
        var firestore = FirebaseFirestore.DefaultInstance;
        firestore.Document("UI_translation/" + ui_i.variable)
        .SetAsync(ui_i, SetOptions.MergeAll);
        Debug.Log("uploaded source " + ui_i.source.ToString());
    }

    public Dictionary<string, string> ui_loc;
    public List<UI_transalations> ui_translations;

    public void Upload_variables_to_FireStore()
    {
        ui_translations = new List<UI_transalations>();
        ui_loc = new Dictionary<string, string>(){

            {"score_perfect", "Perfect"},
            {"score_good","Good"},
            {"score_ok","OK"},
            {"score_noScore","try again"},
            {"game_skip","Skip"},
            {"game_newWord","New Word is "},
            {"ui_leaderboards","Rankings"},
            {"ui_selectLanguage","Select Language"},
            {"ui_changeName","Change name"},
            {"ui_score","Score"},
            {"ui_OK","OK"},
            {"ui_cancel","Cancel"},
            {"ui_settings","Settings"},
        };
        //make em

        

        
        foreach (KeyValuePair<string,string> k in ui_loc)
        {

            var ui_i = new UI_transalations
            {
                variable = k.Key,
                source = k.Value,
            };
            ui_translations.Add(ui_i);
        }

        for (int i = 0; i < ui_translations.Count; i++)
        {
            Upload_locData(ui_translations[i]);
        }
    }
    
    
    public void DonaUI_translations()
    {
        var firestore = FirebaseFirestore.DefaultInstance;
        firestore.Collection("UI_translation/").GetSnapshotAsync().ContinueWith(task =>
        {

            if(task.IsFaulted)
            {
            // Handle the error...
            }
            else if (task.IsCompleted)
            {
            //  {"en-US", "en"},
            //  {"fi-FI", "fi"},
                foreach (KeyValuePair<string, string> k in locDic)
                {
                    var uilocwrap = new Wrapping_UI_loc();
                    var uilocList = new List<UI_Localised>();

                    foreach (DocumentSnapshot l in task.Result.Documents)
                    {
                        
                        //var ui_t = l.ConvertTo<UI_transalations>();
                        var _ui_loc = new UI_Localised();

                        Dictionary<string, string> trans;
                        l.TryGetValue<Dictionary<string, string>>("localised", out trans);

                        foreach(KeyValuePair<string, string> w in trans)
                        {
                            if(w.Key == k.Value)
                            {
                                _ui_loc.translation = w.Value;
                            }
                        }

                        _ui_loc.locale = k.Key;
                        _ui_loc.variable = l.GetValue<string>("variable");
                        uilocList.Add(_ui_loc);
                    }

                    uilocwrap.trans = uilocList;
                    string json = JsonUtility.ToJson(uilocwrap);
                    Debug.Log(json);
                    File.WriteAllText(Application.persistentDataPath +"/"+ k.Key.ToString() +"_ui_trans.json", json); 

                }
                //save per locale --- en_trans, fin trans yms.. ->
            }
        });
    }
}

[System.Serializable]
public class Wrapping_Word
{
    public List<Word> WordSet{get; set;}
    //public string lastUpdated{get; set;}
}

[System.Serializable]
public class Wrapping_UI_loc
{
    public List<UI_Localised> trans;
    //public string lastUpdated{get; set;}
}

public class Wrapping_LB
{
    public List<LeaderBoard_entry> BetterScores{get; set;}
    public string lastUpdated{get; set;}
}


[FirestoreData][System.Serializable]
public struct LeaderBoard_entry
{
    [FirestoreProperty] public string p_DisplayName{get; set;}
    [FirestoreProperty] public int p_score{get; set;}
    [FirestoreProperty] public byte[] UID{get; set;}
    [FirestoreProperty] public object scoreUploaded {get; set;}
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
    [FirestoreProperty] public int      arAE_score{get; set;}
    [FirestoreProperty] public int      caES_score{get; set;}
    [FirestoreProperty] public int      csCZ_score{get; set;}
    [FirestoreProperty] public int      daDK_score{get; set;}
    [FirestoreProperty] public int      esES_score{get; set;}
    [FirestoreProperty] public int      iwIL_score{get; set;}
    [FirestoreProperty] public int      hiIN_score{get; set;}
    [FirestoreProperty] public int      hrHR_score{get; set;}
    [FirestoreProperty] public int      huHU_score{get; set;}
    [FirestoreProperty] public int      idID_score{get; set;}
    [FirestoreProperty] public int      itIT_score{get; set;}
    [FirestoreProperty] public int      jaJP_score{get; set;}
    [FirestoreProperty] public int      koKR_score{get; set;}
    [FirestoreProperty] public int      msMY_score{get; set;}
    [FirestoreProperty] public int      nlNL_score{get; set;}
    [FirestoreProperty] public int      noNO_score{get; set;}
    [FirestoreProperty] public int      plPL_score{get; set;}
    [FirestoreProperty] public int      roRO_score{get; set;}
    [FirestoreProperty] public int      ruRU_score{get; set;}
    [FirestoreProperty] public int      skSK_score{get; set;}
    [FirestoreProperty] public int      svSE_score{get; set;}
    [FirestoreProperty] public int      thTH_score{get; set;}
    [FirestoreProperty] public int      trTR_score{get; set;}
    [FirestoreProperty] public int      ukUA_score{get; set;}
    [FirestoreProperty] public int      viVN_score{get; set;}
    [FirestoreProperty] public float    avgScore{get; set;}
    [FirestoreProperty] public int      current_Hearts{get; set;}
    [FirestoreProperty] public int      current_Skips{get; set;}
    [FirestoreProperty] public string   lastlogin{get; set;}
    [FirestoreProperty] public byte[]   UID{get; set;}
    [FirestoreProperty] public int      multiplier{get; set;}
    [FirestoreProperty] public int      shield_count{get; set;}
    [FirestoreProperty] public int      playerMaxMultiplier{get; set;}
}
// en - en-US - English
// fi - fi-FI - Finnish
// fr - fr-FR - French
// de - de-DE - German
// ar - ar-AE - Arabic

// ca - ca-ES - Catalan
// cs - cs-CZ - Czech
// da - da-DK - Danish
// es - es-ES - Spanish
// he - iw-IL - Hebrew
// hi - hi-IN - Hindi
// hr - hr-HR - Croatian
// hu - hu-HU - Hungarian
// id - id-ID - Indonesian
// it - it-IT - Italian

// ja - ja-JP - Japanese
// ko - ko-KR - Korean
// ms - ms-MY - Malay
// nl - nl-NL - Dutch
// no - no-NO - Norwegian
// pl - pl-PL - Polish
// ro - ro-RO - Romanian
// ru - ru-RU - Russian

// sk - sk-SK - Slovak
// sv - sv-SE - Swedish
// th - th-TH - Thai
// tr - tr-TR - Turkish
// uk - uk-UA - Ukrainian
// vi - vi-VN - Vietnamese

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

[FirestoreData][System.Serializable]
public struct UI_transalations
{

    [FirestoreProperty] public string variable {get; set;}
    [FirestoreProperty] public string source {get; set;}
//    [FirestoreProperty] public string set_locale {get; set;}
}

