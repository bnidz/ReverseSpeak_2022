using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;
using Unity.Notifications.iOS;
using TMPro;
using System.Reflection;

public class Settings : MonoBehaviour
{

    public string localWordsFolder;
    public string localWordsFolder_fullpath;
    public string localPlayerFolder;
    public string localPlayerFolder_fullpath;
    public List<WordClass> gameWords; 
    public string activeWORD;
    public GameObject blindingPanel;
    //public PlayerClass thisPlayer;
    public bool readyToGame = false;
    public string playerJsonDefaultName = "PlayerJson.json";
    private string configFilename = "ConfigsJson.json";
    public string localConfigFolder;// = "/ConfigsFolder/";
    private string localConfigFolder_FullPath;
    public Text debugText;

    ///// --------------- 
    public Player thisPlayer;
    //ADS STUFF
    public bool lastShields = false;
    public bool isActiveShield = false;


    public string lb_cache_Path;
    private string lb_cache = "lb_cache.Json";
    //public Wrapping_LB locale_ranklist;
    public List<int> localeRankList;
    public lbrankWrap lb_wrap;

    public void Init()
    {
        localWordsFolder_fullpath = Application.persistentDataPath + localWordsFolder;
        localPlayerFolder_fullpath = Application.persistentDataPath + localPlayerFolder;
        localConfigFolder_FullPath = Application.persistentDataPath + localConfigFolder;
        lb_cache_Path = Application.persistentDataPath + "/lb_cache/";


        if (!Directory.Exists(localWordsFolder_fullpath))
        {
            //if it doesn't, create it
            Directory.CreateDirectory(localWordsFolder_fullpath);
            Debug.Log("directory " + localWordsFolder_fullpath + " created");
            //create locale word files
        }
        if (!Directory.Exists(Application.persistentDataPath + "/lb_cache/"))
        {
            //if it doesn't, create it
            Directory.CreateDirectory(Application.persistentDataPath + "/lb_cache/");
        }

        string EN_path = localWordsFolder_fullpath + "en-US_WordsJson.json";
        string FI_path = localWordsFolder_fullpath + "fi-FI_WordsJson.json";
        string FR_path = localWordsFolder_fullpath + "fr-FR_WordsJson.json";
        string DE_path = localWordsFolder_fullpath + "de-DE_WordsJson.json";

        if (!File.Exists(EN_path))
        {
            string n_path = Application.streamingAssetsPath + "/en-US_passed_words.json";
            WrappingClass _allwordsClass = new WrappingClass(); 
            _allwordsClass = JsonUtility.FromJson<WrappingClass>(File.ReadAllText(n_path));
            File.WriteAllText(EN_path, JsonUtility.ToJson(_allwordsClass));
            //gameWords = _allwordsClass.Allwords;
            Debug.Log("DONE en-US_WordsJson.json -------------------------");
        }
        if (!File.Exists(FI_path))
        {
            string n_path = Application.streamingAssetsPath + "/fi-FI_passed_words.json";
            WrappingClass _allwordsClass = new WrappingClass(); 
            _allwordsClass = JsonUtility.FromJson<WrappingClass>(File.ReadAllText(n_path));
            File.WriteAllText(FI_path, JsonUtility.ToJson(_allwordsClass));
            //gameWords = _allwordsClass.Allwords;

            Debug.Log("DONE fi-FI_WordsJson.json -------------------------");
        }
        if (!File.Exists(DE_path))
        {
            string n_path = Application.streamingAssetsPath + "/de-DE_passed_words.json";
            WrappingClass _allwordsClass = new WrappingClass(); 
            _allwordsClass = JsonUtility.FromJson<WrappingClass>(File.ReadAllText(n_path));
            File.WriteAllText(DE_path, JsonUtility.ToJson(_allwordsClass));
            //gameWords = _allwordsClass.Allwords;
            Debug.Log("DONE DE_path_WordsJson.json -------------------------");
        }
        if (!File.Exists(FR_path))
        {
            string n_path = Application.streamingAssetsPath + "/fr-FR_passed_words.json";
            WrappingClass _allwordsClass = new WrappingClass(); 
            _allwordsClass = JsonUtility.FromJson<WrappingClass>(File.ReadAllText(n_path));
            File.WriteAllText(FR_path, JsonUtility.ToJson(_allwordsClass));
            //gameWords = _allwordsClass.Allwords;

            Debug.Log("DONE FR_path_WordsJson.json -------------------------");
        }
        if (!Directory.Exists(localPlayerFolder_fullpath))
        {
            //if it doesn't, create it
            Directory.CreateDirectory(localPlayerFolder_fullpath);
            Debug.Log("directory " + localPlayerFolder_fullpath + " created");
        }
        if (!Directory.Exists(localConfigFolder_FullPath))
        {
            //if it doesn't, create it
            Directory.CreateDirectory(localConfigFolder_FullPath);
            Debug.Log("directory " + localConfigFolder_FullPath + " created");
        }
        

        thisPlayer = new Player();
        localeRankList = new List<int>();

        lb_wrap = new lbrankWrap();

    }

    public void LoadSavedWordSettings(string locale)
    {

        string path = localWordsFolder_fullpath + "WordsJson.json";
        if (!File.Exists(path))
        {
            string n_path = Application.streamingAssetsPath + "/eng_passed.json";
            WrappingClass _allwordsClass = new WrappingClass(); 
            _allwordsClass = JsonUtility.FromJson<WrappingClass>(File.ReadAllText(n_path));
            File.WriteAllText(localWordsFolder_fullpath + "WordsJson.json", JsonUtility.ToJson(_allwordsClass));
            gameWords = _allwordsClass.Allwords;
            Debug.Log("DONE NEW WORDS -------------------------");
            return;
        }

        WrappingClass allwordsClass = new WrappingClass(); 
        allwordsClass = JsonUtility.FromJson<WrappingClass>(File.ReadAllText(path));
        gameWords = allwordsClass.Allwords;
        Debug.Log("LOADED OLD WORDS FROM FILE -------------");
        Debug.Log("gamewords lengs" + gameWords.Count);
        Debug.Log(path);
    }

    public IEnumerator MakeGermanWordJson()
    {
        // StartCoroutine(waitWords());
        // //waitWords()
        // yield break;
        string path = localWordsFolder_fullpath + "de-DE_WordsJson.json";
        //Components.c.filereader.MakeNewWordItems();
        while (Components.c.filereader.isDoing) yield return null;
        WrappingClass allwordsClass = new WrappingClass(); 
        allwordsClass.Allwords = Components.c.filereader._allWords;

        gameWords  = allwordsClass.Allwords;
        File.WriteAllText(localWordsFolder_fullpath + "de-DE_WordsJson.json", JsonUtility.ToJson(allwordsClass));
        Debug.Log(JsonUtility.ToJson(allwordsClass));
        LoadLocale("de-DE");
    }

    public IEnumerator updatelocaleWorsdFROM_DB(string locale)
    {
        if(Components.c.settings.thisPlayer.playerLocale == "en-US")
        {
            Components.c.dadabaseManager.en_get_all_words_from_DB();
            while (Components.c.dadabaseManager.fetchingWords == false) yield return null;
            WrappingClass allwordsClass = new WrappingClass(); 
            allwordsClass.Allwords = gameWords; //  Components.c.filereader._allWords;

            //gameWords  = allwordsClass.Allwords;
            File.WriteAllText(localWordsFolder_fullpath + "en-US_WordsJson.json", JsonUtility.ToJson(allwordsClass));
            Debug.Log(JsonUtility.ToJson(allwordsClass));
            Components.c.dadabaseManager.fetchingWords = false;

            Components.c.dadabaseManager.UpdateALLwords();
            while (Components.c.dadabaseManager.updateFrom_debug == true) yield return null;

        }
        if(Components.c.settings.thisPlayer.playerLocale == "fi-FI")
        {

            Components.c.dadabaseManager.fin_get_all_words_from_DB();
            while (Components.c.dadabaseManager.fetchingWords == false) yield return null;
            WrappingClass allwordsClass = new WrappingClass(); 
            allwordsClass.Allwords = gameWords; //  Components.c.filereader._allWords;

            //gameWords  = allwordsClass.Allwords;
            File.WriteAllText(localWordsFolder_fullpath + "fi-FI_WordsJson.json", JsonUtility.ToJson(allwordsClass));
            Debug.Log(JsonUtility.ToJson(allwordsClass));
            Components.c.dadabaseManager.fetchingWords = false;

            Components.c.dadabaseManager.UpdateALLwords();
            while (Components.c.dadabaseManager.updateFrom_debug == true) yield return null;

        }
        if(Components.c.settings.thisPlayer.playerLocale == "fr-FR")
        {
            Components.c.dadabaseManager.fr_get_all_words_from_DB();
            while (Components.c.dadabaseManager.fetchingWords == false) yield return null;
            WrappingClass allwordsClass = new WrappingClass(); 
            allwordsClass.Allwords = gameWords; //  Components.c.filereader._allWords;

            //gameWords  = allwordsClass.Allwords;
            File.WriteAllText(localWordsFolder_fullpath + "fr-FR_WordsJson.json", JsonUtility.ToJson(allwordsClass));
            Debug.Log(JsonUtility.ToJson(allwordsClass));
            Components.c.dadabaseManager.fetchingWords = false;

            Components.c.dadabaseManager.UpdateALLwords();
            while (Components.c.dadabaseManager.updateFrom_debug == true) yield return null;
        }
        if(Components.c.settings.thisPlayer.playerLocale == "de-DE")
        {
            Components.c.dadabaseManager.de_get_all_words_from_DB();
            while (Components.c.dadabaseManager.fetchingWords == false) yield return null;
            WrappingClass allwordsClass = new WrappingClass(); 
            allwordsClass.Allwords = gameWords; //  Components.c.filereader._allWords;

            //gameWords  = allwordsClass.Allwords;
            File.WriteAllText(localWordsFolder_fullpath + "de-DE_WordsJson.json", JsonUtility.ToJson(allwordsClass));
            Debug.Log(JsonUtility.ToJson(allwordsClass));
            Components.c.dadabaseManager.fetchingWords = false;

            Components.c.dadabaseManager.UpdateALLwords();
            while (Components.c.dadabaseManager.updateFrom_debug == true) yield return null;
        }
    }

    public IEnumerator MakeFRENCHWordJson()
    {
        string path = localWordsFolder_fullpath + "fr-FR_WordsJson.json";
        //Components.c.filereader.MakeNewWordItems();
        while (Components.c.filereader.isDoing) yield return null;
        WrappingClass allwordsClass = new WrappingClass(); 
        allwordsClass.Allwords = Components.c.filereader._allWords;

        gameWords  = allwordsClass.Allwords;
        File.WriteAllText(localWordsFolder_fullpath + "fr-FR_WordsJson.json", JsonUtility.ToJson(allwordsClass));
        Debug.Log(JsonUtility.ToJson(allwordsClass));
        LoadLocale("fr-FR");

    }

    public void LoadLocale(string locale)
    {
        //load translated ui... 

        string ui_path = Application.streamingAssetsPath + "/ui_translations/" + locale + "_ui_trans.json";
        string ui_path_2 = Application.streamingAssetsPath + "/ui_translations/" + locale + "_ui_trans_2.json";
        
        Wrapping_UI_loc uiwrap = new Wrapping_UI_loc();
        uiwrap = JsonUtility.FromJson<Wrapping_UI_loc>(File.ReadAllText(ui_path));

        Wrapping_UI_loc uiwrap_2 = new Wrapping_UI_loc();
        uiwrap_2 = JsonUtility.FromJson<Wrapping_UI_loc>(File.ReadAllText(ui_path_2));


        // COMBINE HERRE ---- 
        uiwrap.trans.AddRange(uiwrap_2.trans); 
        Components.c.localisedStrings.ChangeLocale(uiwrap.trans);
        // debug
        Debug.Log("translations list combined: ");
        for (int i = 0; i < uiwrap.trans.Count; i++)
        {
            Debug.Log(i.ToString() + " : " + uiwrap.trans[i].translation.ToString());
        }
    
        // load local gamewords
        string path = Application.streamingAssetsPath + "/locale_words/" + locale + "_WordsJson.json";
        /// in according to dropdown selection as 0 = en-US 1 = fi-FI etc ... 
        WrappingClass allwordsClass = new WrappingClass(); 
        allwordsClass = JsonUtility.FromJson<WrappingClass>(File.ReadAllText(path));
        Debug.Log("loaded locale " + locale + " words!");
        gameWords = allwordsClass.Allwords;
        //start game

        Debug.Log(gameWords.Count +  "  gamewords count :) ");
        Debug.Log("START GAME FROM LOAD LOCALE!!!!!");
        if(!Components.c.runorder.launch)
        {
            if(fromSplashScreen)
            {
                return;
            }
            Components.c.runorder.m_StartGameEvent.Invoke();

        }else
        Components.c.runorder._continue();
    }

    public void _LoadSavedWordSettings()
    {
        //maybe good place to implement word logic, on the cleared eng json
        string path = Application.streamingAssetsPath + "eng_passed.json";
        WrappingClass allwordsClass = new WrappingClass(); 
        allwordsClass = JsonUtility.FromJson<WrappingClass>(File.ReadAllText(path));
        File.WriteAllText(localWordsFolder_fullpath + "WordsJson.json", JsonUtility.ToJson(allwordsClass));
        gameWords = allwordsClass.Allwords;

        if (!File.Exists(path))
        {
            var allWords = new WrappingClass() { Allwords = gameWords };
            string allWordData = JsonUtility.ToJson(allWords);
            Debug.Log("DONE NEW WORDS -------------------------");
        }

        //WrappingClass allwordsClass = new WrappingClass(); 
        allwordsClass = JsonUtility.FromJson<WrappingClass>(File.ReadAllText(path));
        //gameWords = allwordsClass.Allwords;

        Debug.Log("LOADED OLD WORDS FROM FILE -------------");
        Debug.Log("gamewords lengs" + gameWords.Count);
        Debug.Log(path);
    }

    public PlayerClass defaultplayer;
    public bool isDone = false;
    
    public void MakeNewFromDBDefaultWith_GC_ID(string id, string name, string plocale)
    {
        //PlayerClass playerClass = new PlayerClass();
        string path = localPlayerFolder_fullpath + playerJsonDefaultName;
        //playerClass = JsonUtility.FromJson<PlayerClass>(File.ReadAllText(path));


        thisPlayer.playerName = name;
        thisPlayer.playerID = id;
        thisPlayer.lastlogin = DateTime.UtcNow.ToString();
        thisPlayer.UID = GenerateUUID.UUID();
        thisPlayer.playerLocale = plocale;
        //thisPlayer.multiplier = 1;

        //WRITE
        string playerJson = JsonUtility.ToJson(thisPlayer);
        File.WriteAllText(localPlayerFolder_fullpath + playerJsonDefaultName, playerJson); 
        //UPLOAD
        isDone = true;
    }
    // }
    public void UploadNewDefaultPlayerJson()
    {

        PlayerClass playerClass = new PlayerClass();
        playerClass.playerName = "default";
        playerClass.playerID = "default";
        playerClass.playTimesCount = 1;
        playerClass.multiplier = 1;
        playerClass.totalScore = 0;
        playerClass.timesQuessed = 0;
        playerClass.timesSkipped = 0;
        playerClass.totalTries = 0;
        playerClass.playerMaxMultiplier = 5;
        playerClass.current_Hearts = 3;
        playerClass.current_Skips = 1;
        playerClass.shield_count = 0;

        string playerJson = JsonUtility.ToJson(playerClass);
        Components.c.dadabaseManager.UploadPlayerJson(playerJson);
    }

    public void LoadSavedPlayerSettings()
    {

        thisPlayer.playTimesCount++;
        Debug.Log(thisPlayer.playerName + thisPlayer.playerID + thisPlayer.playTimesCount);
        betweenSeconds = Convert.ToInt32((DateTime.UtcNow - DateTime.Parse(Components.c.settings.thisPlayer.lastlogin)).TotalSeconds);
        Components.c.settings.thisPlayer.lastlogin = DateTime.UtcNow.ToString();
        Debug.Log("Total seconds between pause and foreground + " + betweenSeconds);
        StartCoroutine(LoadDefaultConfigs());

    }
    private int betweenSeconds;
    public IEnumerator LoadDefaultConfigs()
    {

        Components.c.fireStore_Manager.isDoneConfigs = false;
        Components.c.fireStore_Manager.GetConfigs();

        while (Components.c.fireStore_Manager.isDoneConfigs == false) yield return null;
            Components.c.filetotext.skipCoolDown = thisConfigs.skip_CoolDown;
            Components.c.filetotext.heartCoolDown = thisConfigs.heart_CoolDown;
            AD_TEXT_hearts.text = thisConfigs.ad_heart_reward.ToString();
            AD_TEXT_skips.text = thisConfigs.ad_skip_reward.ToString();


            UpdateFrom_BetweenPlays(betweenSeconds);

    }

    public Configs thisConfigs;
    private int difference;
    public void SavePlayerdDataToFile()
    {
        thisPlayer.lastlogin = DateTime.UtcNow.ToString();
        string playerJson = JsonUtility.ToJson(thisPlayer);

        File.WriteAllText(localPlayerFolder_fullpath + playerJsonDefaultName, playerJson);
        //update hearts full notification
        if(thisPlayer.current_Hearts < thisConfigs.max_Hearts)
        {
            int timeToFullhearts_seconds = (thisConfigs.max_Hearts - thisPlayer.current_Hearts) * thisConfigs.heart_CoolDown;
            ScheduledNotification_HeartsFull(timeToFullhearts_seconds);
        }

        debugText.text = 
            "name : " +
            thisPlayer.playerName
            +"\n" +
            "player ID: " +
            thisPlayer.playerID
            +"\n" + 
            "playtimes: " +
            thisPlayer.playTimesCount
            +"\n" + 
            "p_totalScore: " +
            thisPlayer.totalScore
            +"\n" +  
            "times quessed: " +
            thisPlayer.timesQuessed
            +"\n" + 
            "times skipped: " +            
            thisPlayer.timesSkipped
            +"\n" + 
            "total tries: " +
            thisPlayer.totalTries
            + "\n" + 
            "last login: " +
            thisPlayer.lastlogin.ToString()
            +"\n" + 
            "difference: " +
            difference.ToString()
            +"\n" + 
            "UID: " +
            thisPlayer.UID.ToString();
            //upload new score to LB
            //Components.c.highScores.UploadScore(thisPlayer.playerName ,thisPlayer.totalScore);
            //how to make reference to user ID

            if(thisPlayer.UID.Length < 1)
            {
                thisPlayer.UID = GenerateUUID.UUID();
            }

            //upload playerclass to DB
            Components.c.fireStore_Manager.Save_Player_to_DB(thisPlayer);
            //Components.c.dadabaseManager.getUIDraw();
            Components.c.fireStore_Manager.Update_LB(thisPlayer);
            Components.c.gameUIMan.UpdateScoreTo_UI();
    }

    public void SaveWordDataToFile()
    {
        var allWords = new WrappingClass() { Allwords = gameWords };
        string allWordData = JsonUtility.ToJson(allWords);
        File.WriteAllText(localWordsFolder_fullpath + "WordsJson.json", allWordData); 
    }

    [System.Serializable]
    public class WrappingClass
    {
        public List<WordClass> Allwords;
    }
    [System.Serializable]
    public class lbrankWrap
    {
        public List<int> rank_scores;
     //   public string last_updated;
    }
    const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789"; //add the characters you want
    private string p_name;
    // thisConfigs;
    public TMPro.TextMeshProUGUI AD_TEXT_hearts;
    public TMPro.TextMeshProUGUI AD_TEXT_skips;
    public bool updateBetweenPlays = false;
    public void UpdateFrom_BetweenPlays(int seconds)
    {
        Components.c.settings.thisPlayer.lastlogin = DateTime.UtcNow.ToString();
        if(thisPlayer.current_Hearts < thisConfigs.max_Hearts)
        {
            int howManyToAdd = (seconds / thisConfigs.heart_CoolDown);
            Debug.Log("HOW MANY TO ADD INT " + howManyToAdd);

            if((howManyToAdd + thisPlayer.current_Hearts) >= thisConfigs.max_Hearts)
            {
                thisPlayer.current_Hearts = thisConfigs.max_Hearts;
            }else
            {
                thisPlayer.current_Hearts += howManyToAdd;
            }
        }
        //skips
        if(thisPlayer.current_Skips < thisConfigs.max_Skip_Amount)
        {
            int howManyToAdd = (seconds / thisConfigs.skip_CoolDown);
            Debug.Log("HOW MANY TO ADD INT " + howManyToAdd);
            if((howManyToAdd + thisPlayer.current_Skips) >= thisConfigs.max_Skip_Amount)
            {
                thisPlayer.current_Skips = thisConfigs.max_Skip_Amount;
            }else
            {
                thisPlayer.current_Skips += howManyToAdd;
            }
        }
        //update UI
        string saveJson = JsonUtility.ToJson(thisPlayer);
        Debug.Log("saved from pausetime configs");

        updateBetweenPlays = true;
        //Components.c.gameUIMan.UpdateUIToConfigs();
    }

    /// timed notifications 
    private bool thereIsActiveNotification_hearts = false;
    public void ScheduledNotification_HeartsFull(int total_seconds)
    {
        int saveBufferSeconds = 10;
        TimeSpan t = TimeSpan.FromSeconds( total_seconds + saveBufferSeconds);

        string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms", 
                        t.Hours, 
                        t.Minutes, 
                        t.Seconds, 
                        t.Milliseconds);

        var timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = new TimeSpan(t.Hours, t.Minutes, t.Seconds),
            Repeats = false
        };

        var notification = new iOSNotification()
        {
            // You can specify a custom identifier which can be used to manage the notification later.
            // If you don't provide one, a unique string will be generated automatically.

            Identifier = "hearts_full",
            Title = "Reverse Speak",
            Body = "Scheduled at: " + DateTime.Now.ToShortDateString() + " triggered in 5 seconds",
            Subtitle = "Your Lifes have replenished, it's time to claim your dominance!",
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = timeTrigger,

        };

        if(thereIsActiveNotification_hearts)
        {

            RemoveNotification_HeartsFull();
            thereIsActiveNotification_hearts = false;

        }

        iOSNotificationCenter.ScheduleNotification(notification);
        thereIsActiveNotification_hearts = true;
    }


    public void RemoveNotification_HeartsFull()
    {
        iOSNotificationCenter.RemoveScheduledNotification("hearts_full");
    }

    public void SavePlayerConfigs()
    {
        string configJson = JsonUtility.ToJson(thisConfigs);
        File.WriteAllText(localConfigFolder_FullPath + configFilename, configJson); 
    }
    
    public void ChangeName(string name)
    {
        Debug.Log("CHANGE NAME" + name);
        newname = name;
    }
    private string newname;
    public void SubmitName()
    {
        if(name.Length >0)
        {
            thisPlayer.playerName = name;
            SavePlayerdDataToFile();
        }
        Components.c.gameUIMan.HideLogin();
        name = "";
        SubmitNameChangeButton.gameObject.SetActive(false);
        LoadSplashScreenDefaults();
    }

    public IEnumerator LoadLocaleLB_cache()
    {

        lb_wrap.rank_scores = localeRankList;
        lb_wrap.rank_scores.Clear();
        string path = lb_cache_Path + locale + lb_cache;
        Debug.Log("path to chekc for daily json ranklist  " + path);

        Components.c.fireStore_Manager.donaRankdone = false;
        if (!File.Exists(lb_cache_Path + locale + lb_cache))
        {
            localeRankList.Clear();
            Debug.Log("no daily ranklist dona tryna");
            Components.c.fireStore_Manager.Get_Daily_ScoreList_for_Rank();
            while (!Components.c.fireStore_Manager.donaRankdone) yield return null;

            lb_wrap.rank_scores = localeRankList;
            string localeRankListJson = JsonUtility.ToJson(lb_wrap);

            Debug.Log(localeRankListJson);
            Components.c.fireStore_Manager.donaRankdone = false;
            File.WriteAllText(lb_cache_Path + locale + lb_cache, localeRankListJson);
        }
        Debug.Log("yes ---- daily donaranklist exists ---- ");
        Debug.Log("between seconds! "   + betweenSeconds);

        if(betweenSeconds > (3 * 60)) //change to day or something :D
        {
            localeRankList.Clear();
            Debug.Log("LAST LOGIN SO OLD REFRESHING RANKLIST :O");
            //dona it again --- 
            Components.c.fireStore_Manager.Get_Daily_ScoreList_for_Rank();
            while (!Components.c.fireStore_Manager.donaRankdone) yield return null;

            lb_wrap.rank_scores = localeRankList;
            string localeRankListJson = JsonUtility.ToJson(lb_wrap);
            
            Debug.Log(localeRankListJson);
            File.WriteAllText(lb_cache_Path + locale + lb_cache, localeRankListJson);
            Components.c.fireStore_Manager.donaRankdone = false;
        }
        lb_wrap = JsonUtility.FromJson<lbrankWrap>(File.ReadAllText(lb_cache_Path + locale + lb_cache));
        Debug.Log("items in locale ranklist" + lb_wrap.rank_scores.Count);
        Components.c.gameUIMan.UpdateRankText();

        if(Components.c.settings.blindingPanel.activeInHierarchy)
        {
            //update rank there
            yield return new WaitForSeconds(.2f);
            splashRankTEXT.text = Components.c.gameUIMan.rank.ToString();
        }
    }

    public List<string> locLB_id;
    public string locale;
    public TextMeshProUGUI splashRankTEXT;

   private static string GetPropertyValues(Player player, string variable)
   {
    string value;
      Type t = player.GetType();
      Debug.LogFormat("Type is: {0}", t.Name);
      PropertyInfo[] props = t.GetProperties();
      Debug.LogFormat("Properties (N = {0}):", 
                        props.Length);
      foreach (var prop in props)
        if (prop.GetIndexParameters().Length == 0)
        {
            Debug.LogFormat("   {0} ({1}): {2}", prop.Name,
                              prop.PropertyType.Name,
                              prop.GetValue(player));
        if(prop.Name == variable)
        {
            value = prop.GetValue(player).ToString();
            return value;
        }
        }
        else
        {
            Debug.LogFormat("   {0} ({1}): <Indexed>", prop.Name,
                              prop.PropertyType.Name);
            value = prop.GetValue(player).ToString();
            if(prop.Name == variable)
            {
                value = prop.GetValue(player).ToString();
                return value;
            }
        }

    return "mukbang :D";
    //return value;
   }

    private void SetPropertyValues(Player player, string variable, int value)
    {

      Type t = player.GetType();
      Debug.LogFormat("Type is: {0}", t.Name);
      PropertyInfo[] props = t.GetProperties();
      Debug.LogFormat("Properties (N = {0}):", 
                        props.Length);

    foreach (var prop in props)
    if (prop.GetIndexParameters().Length == 0)
    {
        Debug.LogFormat("   {0} ({1}): {2}", prop.Name,
                            prop.PropertyType.Name,
                            prop.GetValue(player));
        if(prop.Name == variable)
        {
            prop.SetValue(player, value);
        }
    }
    else
    {
        Debug.LogFormat("   {0} ({1}): <Indexed>", prop.Name,
                            prop.PropertyType.Name);
        //prop.SetValue(player, value);
        if(prop.Name == variable)
        {
            prop.SetValue(player, value);
        }
    }
   }

    private int selection;
    public void ChangeLocale(int selection)
    {
        sessionScore = 0;
        // locLB_id = new List<string>(){
        //      {"enUS_score"},
        //      {"fiFI_score"},
        //      {"frFR_score"},
        //      {"deDE_score"},
        //      {"arAE_score"},
        //      {"caES_score"},
        //      {"csCZ_score"},
        //      {"daDK_score"},
        //      {"esES_score"},
        //      {"iwIL_score"},
        //      {"hiIN_score"},
        //      {"hrHR_score"},
        //      {"huHU_score"}, 
        //      {"idID_score"},
        //      {"itIT_score"},
        //      {"jaJP_score"},
        //      {"koKR_score"},
        //      {"msMY_score"},
        //      {"nlNL_score"},
        //      {"noNO_score"},
        //      {"plPL_score"},
        //      {"roRO_score"},
        //      {"ruRU_score"},
        //      {"skSK_score"},
        //      {"svSE_score"},
        //      {"thTH_score"},
        //      {"trTR_score"},
        //      {"ukUA_score"},
        //      {"viVN_score"},
        // };
        // if(fromSplashScreen)
        // return;

        string loc;
        loc_sel.TryGetValue(selection, out loc);

        locale = loc;
        //localeScore = int.Parse(GetPropertyValues(thisPlayer, locLB_id[selection]));
        Components.c.sampleSpeechToText.SetSettings(locale, .95f,.95f);

        thisPlayer.playerLocale = locale;
        Components.c.fireStore_Manager.Init();
        StartCoroutine(Components.c.fireStore_Manager.DonaLB_values());
        //load locale rank-list
        StartCoroutine(LoadLocaleLB_cache());
        Components.c.speechToText.Setting(locale);
        Components.c.gameUIMan.UpdateScoreTo_UI();

        LoadLocale(locale);
        Components.c.gameUIMan.Update_UI_DailyStreak();
    }

    public void _LoadLocale(string path)
    {
        WrappingClass allwordsClass = new WrappingClass(); 
        allwordsClass = JsonUtility.FromJson<WrappingClass>(File.ReadAllText(path));
        Debug.Log("loaded locale " + locale + " words!");
        gameWords = allwordsClass.Allwords;
    }

    private bool eng_last_word = false;
    private bool fin_last_word = false;
    private bool fr_last_word = false;
    private bool de_last_word = false;

    public void UpdateLastLocaleWord()
    {
        if(Components.c.settings.thisPlayer.playerLocale == "en-US")
        {
            last_eng_word = Components.c.gameloop.activeWord;
        }
        if(Components.c.settings.thisPlayer.playerLocale == "fi-FI")
        {
            last_fin_word = Components.c.gameloop.activeWord;
        }
        if(Components.c.settings.thisPlayer.playerLocale == "fr-FR")
        {
            last_fr_word = Components.c.gameloop.activeWord;
        }
        if(Components.c.settings.thisPlayer.playerLocale == "de-DE")
        {
            last_de_word = Components.c.gameloop.activeWord;
        }
        Debug.Log("updated last localeword");
    }

    public WordClass lastLocaleWord()
    {

        WordClass lastLocaleWord = new WordClass();
            if(Components.c.settings.thisPlayer.playerLocale == "en-US")
            {
              lastLocaleWord =  last_eng_word;
            }
            if(Components.c.settings.thisPlayer.playerLocale == "fi-FI")
            {
                lastLocaleWord = last_fin_word;
            }
            if(Components.c.settings.thisPlayer.playerLocale == "fr-FR")
            {
                lastLocaleWord = last_fr_word;
            }
            if(Components.c.settings.thisPlayer.playerLocale == "de-DE")
            {
                lastLocaleWord = last_de_word;
            }
            Debug.Log("updated last localeword");
            return lastLocaleWord;
    }
    public Dictionary<int, string> loc_sel = new Dictionary<int, string>(){

            {0, "en-US"},
            {1, "fi-FI"},
            {2, "fr-FR"},
            {3, "de-DE"},
            {4, "ar-AE"},
            {5, "ca-ES"},
            {6, "cs-CZ"},
            {7, "da-DK"},
            {8, "es-ES"},
            {9, "iw-IL"},
            {10, "hi-IN"},
            {11, "hr-HR"},
            {12, "hu-HU"},
            {13, "id-ID"},
            {14, "it-IT"},
            {15, "ja-JP"},
            {16, "ko-KR"},
            {17, "ms-MY"},
            {18, "nl-NL"},
            {19, "no-NO"},
            {20, "pl-PL"},
            {21, "ro-RO"},
            {22, "ru-RU"},
            {23, "sk-SK"},
            {24, "sv-SE"},
            {25, "th-TH"},
            {26, "tr-TR"},
            {27, "uk-UA"},
            {28, "vi-VN"},
    };


    public int sessionScore = 0;
    public int lastScore;

    public void AddToScore(int score)
    {

        lastScore = score;
        sessionScore += lastScore;
        localeScore += score;

        Components.c.fireStore_Manager.score_locale_all_time += score;
        Components.c.fireStore_Manager.score_locale_yearly += score;
        Components.c.fireStore_Manager.score_locale_monthly += score;
        Components.c.fireStore_Manager.score_locale_weekly += score;
    }

    public void DailyTaskWordComplete()
    {
        var today = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);
        if((today - DateTime.Parse(thisPlayer.DailyTasksDoneDate)).Days == 0)
        {
            if(thisPlayer.dailyTaskStreak == 0)
            {
                Debug.Log("streak 0.... ");
            }else
            {
                Debug.Log("dates between : "  + (DateTime.UtcNow - DateTime.Parse(thisPlayer.DailyTasksDoneDate)).Days.ToString());
                return;
            }
        }
        if(thisPlayer.dailyTaskWordsComplete < (thisConfigs.dailyTask_baseValue + (thisPlayer.dailyTaskStreak * thisConfigs.dailyTask_increment)))
        {
            thisPlayer.dailyTaskWordsComplete++;
            //return;
        }
        if(thisPlayer.dailyTaskWordsComplete == (thisConfigs.dailyTask_baseValue + (thisPlayer.dailyTaskStreak * thisConfigs.dailyTask_increment)))
        {
            DailyTaskComplete();
        }
        Components.c.gameUIMan.Update_UI_DailyStreak();
    }

    public void DailyTaskComplete()
    {
        thisPlayer.dailyTaskStreak++;
        thisPlayer.dailyTaskWordsComplete = 0;
        Components.c.gameUIMan.ui_streakText.text = thisPlayer.dailyTaskStreak.ToString();
        thisPlayer.DailyTasksDoneDate = DateTime.UtcNow.ToString();

        Components.c.gameUIMan.SpawnCongratz();
    }
    public void CheckStreak()
    {

        if(thisPlayer.dailyTaskStreak == 0)
        {
            thisPlayer.DailyTasksDoneDate = DateTime.UtcNow.ToString();
        }

        if(thisPlayer.dailyTaskStreak > 0)
        {
            // if((DateTime.UtcNow - DateTime.Parse(thisPlayer.DailyTasksDoneDate)).Days == 0)
            // {
            //     // no change
            //    // UpdateSplashScreenDailyStreak()

            // }
            var today = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);
            if((today - DateTime.Parse(thisPlayer.DailyTasksDoneDate)).TotalHours > 24)// && thisPlayer.dailyTaskWordsComplete == (thisConfigs.dailyTask_baseValue + (thisPlayer.dailyTaskStreak * thisConfigs.dailyTask_increment)))
            {
                // have new missions
                thisPlayer.dailyTaskWordsComplete = 0;
                thisPlayer.dailyTaskStreak = 0;                

            }
            // if((DateTime.UtcNow - DateTime.Parse(thisPlayer.DailyTasksDoneDate)).Days >= 2)
            // {
            //     thisPlayer.dailyTaskStreak = 0;                
            //     thisPlayer.dailyTaskWordsComplete = 0;
            //     //new missions

            // }
        }
    }


    public WordClass last_fr_word;
    public WordClass last_de_word;
    public WordClass last_eng_word;
    public WordClass last_fin_word;
    public int localeScore = 0;
    public void CloseBlindingPanel()
    {
        Components.c.runorder.blindingPanel.SetActive(false);
    }

    public void StartGameButtonPress()
    {
        Components.c.gameUIMan.DailyQuestHolder.transform.parent = Components.c.gameUIMan.DailyQuest_OG_parent.transform;
        Components.c.settings.CloseBlindingPanel();
        //close menu if open
        if(Components.c.gameUIMan.settingsMenu.activeInHierarchy)
        {
          Components.c.gameUIMan.settingsMenu.SetActive(false);
        }
        fromSplashScreen = false;
        ChangeLocale(changelocale_dropDown.value);
        pentagramButton.SetActive(true);
    }

    public void MakeSubmitChangeNameButtonVisible()
    {
        SubmitNameChangeButton.gameObject.SetActive(true);
    }

    public Button StartGameSplashScreenButton;
    public Button SubmitNameChangeButton;
    public GameObject pentagramButton;
    public TMP_InputField changeName_inputField;
    public TextMeshProUGUI changeName_inputField_placeholder;
    public TMP_Dropdown changelocale_dropDown;

    public void LoadSplashScreenDefaults()
    {
        fromSplashScreen = true;
            //change current name to placeholder
        changeName_inputField_placeholder.text = thisPlayer.playerName;
        //load last locale to dropdowns
        if(Components.c.settings.thisPlayer.playerLocale == "en-US")
        {
            changelocale_dropDown.value = 0;
        }
        if(Components.c.settings.thisPlayer.playerLocale == "fi-FI")
        {
            changelocale_dropDown.value = 1;
        }
        if(Components.c.settings.thisPlayer.playerLocale == "fr-FR")
        {
            changelocale_dropDown.value = 2;
        }
        if(Components.c.settings.thisPlayer.playerLocale == "de-DE")
        {
            changelocale_dropDown.value = 3;
        }
    }
    private bool fromSplashScreen = false;
}