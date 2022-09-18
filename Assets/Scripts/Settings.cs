using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;
using Unity.Notifications.iOS;

public class Settings : MonoBehaviour
{

    public string localWordsFolder;
    public string localWordsFolder_fullpath;
    public string localPlayerFolder;
    public string localPlayerFolder_fullpath;
    public List<WordClass> gameWords; 
    public string activeWORD;
    public GameObject blindingPanel;
    public PlayerClass currentPlayer;
    public bool readyToGame = false;
    public string playerJsonDefaultName = "PlayerJson.json";
    private string configFilename = "ConfigsJson.json";
    public string localConfigFolder;// = "/ConfigsFolder/";
    private string localConfigFolder_FullPath;

    public Text debugText;

    public void Init()
    {

        localWordsFolder_fullpath = Application.persistentDataPath + localWordsFolder;
        localPlayerFolder_fullpath = Application.persistentDataPath + localPlayerFolder;
        localConfigFolder_FullPath = Application.persistentDataPath + localConfigFolder;

        if (!Directory.Exists(localWordsFolder_fullpath))
        {
            //if it doesn't, create it
            Directory.CreateDirectory(localWordsFolder_fullpath);
            Debug.Log("directory " + localWordsFolder_fullpath + " created");
            //create locale word files
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
    }

    public IEnumerator waitWords()
    {
        Components.c.dadabaseManager.get_all_words_from_DB();        

        while (!Components.c.dadabaseManager.fetchingWords) yield return null;

        gameWords = Components.c.dadabaseManager.temp;
        WrappingClass _allwordsClass = new WrappingClass(); 
        _allwordsClass.Allwords = gameWords;
        //File.WriteAllText(localWordsFolder_fullpath + "fin_passed.json", JsonUtility.ToJson(_allwordsClass));
        Debug.Log("Wrote passed words to file---- " + gameWords.Count.ToString());
        Debug.Log(JsonUtility.ToJson(_allwordsClass));
    }
    public void LoadSavedWordSettings(string locale)
    {

        //StartCoroutine(waitWords());

        // gameWords = Components.c.dadabaseManager.temp;
        // WrappingClass _allwordsClass = new WrappingClass(); 
        // _allwordsClass.Allwords = gameWords;
        // File.WriteAllText(localWordsFolder_fullpath + "WordsJson.json", JsonUtility.ToJson(_allwordsClass));
        // Debug.Log("Wrote passed words to file---- " + gameWords.Count.ToString());

        //maybe good place to implement word logic, on the cleared eng json
        //Components.c.dadabaseManager.get_all_words_from_DB();        


        //HAVE LOCALE IN FRONT OF WORDS JSON FILE --- GATHER FINAL VERSION TO DEVICE FIRST
        string path = localWordsFolder_fullpath + "WordsJson.json";
        if (!File.Exists(path))
        {
            // Components.c.filereader.MakeNewWordItems();
            // gameWords = Components.c.filereader.allWords;
            // var allWords = new WrappingClass() { Allwords = gameWords };
            // string allWordData = JsonUtility.ToJson(allWords);
            // File.WriteAllText(localWordsFolder_fullpath + "WordsJson.json", allWordData); 
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


    public IEnumerator MakeFinnishWordJson()
    {
        StartCoroutine(waitWords());
        //waitWords()
        yield break;
        // string path = localWordsFolder_fullpath + "fi-FI_WordsJson.json";
        // Components.c.filereader.MakeNewWordItems();
        // while (Components.c.filereader.isDoing) yield return null;
        // WrappingClass allwordsClass = new WrappingClass(); 
        // allwordsClass.Allwords = Components.c.filereader._allWords;

        // gameWords  = allwordsClass.Allwords;
        // File.WriteAllText(localWordsFolder_fullpath + "fi-FI_WordsJson.json", JsonUtility.ToJson(allwordsClass));

    }

        public IEnumerator MakeGermanWordJson()
    {
        // StartCoroutine(waitWords());
        // //waitWords()
        // yield break;
        string path = localWordsFolder_fullpath + "de-DE_WordsJson.json";
        Components.c.filereader.MakeNewWordItems();
        while (Components.c.filereader.isDoing) yield return null;
        WrappingClass allwordsClass = new WrappingClass(); 
        allwordsClass.Allwords = Components.c.filereader._allWords;

        gameWords  = allwordsClass.Allwords;
        File.WriteAllText(localWordsFolder_fullpath + "de-DE_WordsJson.json", JsonUtility.ToJson(allwordsClass));
        Debug.Log(JsonUtility.ToJson(allwordsClass));
        LoadLocale("de-DE");



    }


    public IEnumerator MakeFRENCHWordJson()
    {
        // StartCoroutine(waitWords());
        // //waitWords()
        // yield break;
        string path = localWordsFolder_fullpath + "fr-FR_WordsJson.json";
        Components.c.filereader.MakeNewWordItems();
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
        string EN_path = localWordsFolder_fullpath + "en-US_WordsJson.json";
        string FI_path = localWordsFolder_fullpath + "fi-FI_WordsJson.json";
        string FR_path = localWordsFolder_fullpath + "fr-FR_WordsJson.json";
        string DE_path = localWordsFolder_fullpath + "de-DE_WordsJson.json";

        string path = localWordsFolder_fullpath + locale + "_WordsJson.json";
        /// in according to dropdown selection as 0 = en-US 1 = fi-FI etc ... 
        WrappingClass allwordsClass = new WrappingClass(); 
        allwordsClass = JsonUtility.FromJson<WrappingClass>(File.ReadAllText(path));
        Debug.Log("loaded locale " + locale + " words!");
        gameWords = allwordsClass.Allwords;
        //start game
        Components.c.gameloop.NewRandomWORD();
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
        currentPlayer.playerName = name;
        currentPlayer.playerID = id;
        currentPlayer.lastlogin = DateTime.UtcNow.ToString();
        currentPlayer.UID = GenerateUUID.UUID();
        currentPlayer.playerLocale = plocale;
        //WRITE
        string playerJson = JsonUtility.ToJson(currentPlayer);
        File.WriteAllText(localPlayerFolder_fullpath + playerJsonDefaultName, playerJson); 
        //UPLOAD
        
        isDone = true;

        //LoadSavedWordSettings();
        //LoadDefaultConfigs();


        //UpdateFrom_BetweenPlays(betweenSeconds);

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

        string playerJson = JsonUtility.ToJson(playerClass);
        Components.c.dadabaseManager.UploadPlayerJson(playerJson);
    }

    public void LoadSavedPlayerSettings(string name, string id)
    {


        string path = localPlayerFolder_fullpath + playerJsonDefaultName;
        currentPlayer.playTimesCount++;
        debugText.text =
            "name : " +
            currentPlayer.playerName
            +"\n" +
            "player ID: " +
            currentPlayer.playerID
            +"\n" + 
            "playtimes: " +
            currentPlayer.playTimesCount
            +"\n" + 
            "p_totalScore: " +
            currentPlayer.totalScore
            +"\n" +  
            "times quessed: " +
            currentPlayer.timesQuessed
            +"\n" + 
            "times skipped: " +            
            currentPlayer.timesSkipped
            +"\n" + 
            "total tries: " +
            currentPlayer.totalTries
            +"\n" + 
            "last login: " +
            currentPlayer.lastlogin.ToString()
            +"\n"+ 
            "difference: " +
            0.ToString();

        Debug.Log(difference);
        Debug.Log("player class loaded from file");

        int betweenSeconds = Convert.ToInt32((DateTime.UtcNow - DateTime.Parse(Components.c.settings.currentPlayer.lastlogin)).TotalSeconds);
        Components.c.settings.currentPlayer.lastlogin = DateTime.UtcNow.ToString();
        Debug.Log("Total seconds between pause and foreground + " + betweenSeconds);

        //LoadSavedWordSettings();
        LoadDefaultConfigs();
        UpdateFrom_BetweenPlays(betweenSeconds);

    }
    private int difference;
    public void SavePlayerdDataToFile()
    {
        currentPlayer.lastlogin = DateTime.UtcNow.ToString();
        string playerJson = JsonUtility.ToJson(currentPlayer);
        //int scoreVal = 0;
        if(Components.c.settings.currentPlayer.playerLocale == "en-US")
        {
            currentPlayer.enUS_score = localeScore;
        }
        if(Components.c.settings.currentPlayer.playerLocale == "fi-FI")
        {
            currentPlayer.fiFI_score = localeScore;;
        }
        if(Components.c.settings.currentPlayer.playerLocale == "fr-FR")
        {
            currentPlayer.frFR_score = localeScore;;
        }
        if(Components.c.settings.currentPlayer.playerLocale == "de-DE")
        {
            currentPlayer.deDE_score = localeScore;;
        }
        
        File.WriteAllText(localPlayerFolder_fullpath + playerJsonDefaultName, playerJson);

        //update hearts full notification

        if(currentPlayer.current_Hearts < currentConfigs.max_Hearts)
        {
            int timeToFullhearts_seconds = (currentConfigs.max_Hearts - currentPlayer.current_Hearts) * currentConfigs.heart_CoolDown;
            ScheduledNotification_HeartsFull(timeToFullhearts_seconds);
        }

        debugText.text = 
            "name : " +
            currentPlayer.playerName
            +"\n" +
            "player ID: " +
            currentPlayer.playerID
            +"\n" + 
            "playtimes: " +
            currentPlayer.playTimesCount
            +"\n" + 
            "p_totalScore: " +
            currentPlayer.totalScore
            +"\n" +  
            "times quessed: " +
            currentPlayer.timesQuessed
            +"\n" + 
            "times skipped: " +            
            currentPlayer.timesSkipped
            +"\n" + 
            "total tries: " +
            currentPlayer.totalTries
            + "\n" + 
            "last login: " +
            currentPlayer.lastlogin.ToString()
            +"\n" + 
            "difference: " +
            difference.ToString()
            +"\n" + 
            "UID: " +
            currentPlayer.UID.ToString();
            //upload new score to LB
            //Components.c.highScores.UploadScore(currentPlayer.playerName ,currentPlayer.totalScore);
            //how to make reference to user ID

            if(currentPlayer.UID.Length < 1)
            {
                currentPlayer.UID = GenerateUUID.UUID();
            }

            //upload playerclass to DB
            Components.c.dadabaseManager.UploadNewPlayerTo_DB(currentPlayer);
            Components.c.dadabaseManager.getUIDraw();
            Components.c.dadabaseManager.Update_LB_UserEntry(currentPlayer);

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
    const string glyphs= "abcdefghijklmnopqrstuvwxyz0123456789"; //add the characters you want
    public void GenerateNewPlayer()
    {

        string path = localPlayerFolder_fullpath + playerJsonDefaultName;

        if(!File.Exists(path))
        {

            // PlayerClass playerClass = new PlayerClass();
            // int charAmount = UnityEngine.Random.Range(6, 12); //set those to the minimum and maximum length of your string
            // for(int i=0; i<charAmount; i++)
            // {
            //     playerClass.playerName += glyphs[UnityEngine.Random.Range(0, glyphs.Length)];
            // }
            // //playerClass.playerName = name;
            // playerClass.playerID = playerClass.playerName;
            // playerClass.playTimesCount = 1;
            // playerClass.multiplier = 1;
            // playerClass.totalScore = 0;
            // playerClass.timesQuessed = 0;
            // playerClass.timesSkipped = 0;
            // playerClass.totalTries = 0;
            // playerClass.current_Hearts = 0;
            // playerClass.current_Skips = 0;
            // playerClass.playerMaxMultiplier = 5;
            // playerClass.lastlogin =  DateTime.UtcNow.ToString();
            // string playerJson = JsonUtility.ToJson(playerClass);
            // //File.WriteAllText(localPlayerFolder_fullpath + playerClass.playerName +".json", playerJson);
            // File.WriteAllText(path, playerJson);

            // playerJsonDefaultName =  playerClass.playerName +".json";
            // Debug.Log("Generated fresh PlayerJson");
            // currentPlayer = playerClass;
            // //SavePlayerdDataToFile();

        }
    }
    private string p_name;

    public GameConfigs currentConfigs;
    public void GenerateDefaultConfigs()
    {
        currentConfigs = new GameConfigs();
        currentConfigs.configVersion = 1;
        currentConfigs.max_Skip_Amount = 5;
        currentConfigs.max_Hearts = 10;
        currentConfigs.heart_CoolDown = 60; //sec
        currentConfigs.skip_CoolDown = 30; //sec

        string defconfigs = JsonUtility.ToJson(currentConfigs);
        File.WriteAllText(localConfigFolder_FullPath + configFilename, defconfigs);
        Debug.Log("Generated def configs");
        StartCoroutine(waitConfigUpload());
        Debug.Log("UPLOADED TO DB DEF CONFIGS");
        //update UI
        //Components.c.gameUIMan.UpdateUIToConfigs();
        //update timer values
        //Components.c.filetotext.startUpdates = true;
    }
    public IEnumerator waitConfigUpload()
    {
        Components.c.dadabaseManager.isDone = false;
        Components.c.dadabaseManager.UploadDefaultConfig(currentConfigs);
        while (!Components.c.dadabaseManager.isDone) yield return null;
        Components.c.dadabaseManager.isDone = false;
    }

    
    public TMPro.TextMeshProUGUI AD_TEXT_hearts;
    public TMPro.TextMeshProUGUI AD_TEXT_skips;
    public IEnumerator donaConfigs()
    {
        Components.c.dadabaseManager.isDone = false;
        Components.c.dadabaseManager.DoneDefConfigs();
        while (!Components.c.dadabaseManager.isDone) yield return null;
        Components.c.dadabaseManager.isDone = false;
        Debug.Log("LOADED CONFIGS FROM DB yo");

        Components.c.filetotext.skipCoolDown = Components.c.settings.currentConfigs.skip_CoolDown;
        Components.c.filetotext.heartCoolDown = Components.c.settings.currentConfigs.heart_CoolDown;

        AD_TEXT_hearts.text = currentConfigs.ad_heart_reward.ToString();
        AD_TEXT_skips.text = currentConfigs.ad_skip_reward.ToString();

    }
    public void LoadDefaultConfigs()
    {
                currentConfigs = new GameConfigs();
        //GenerateDefaultConfigs();
        StartCoroutine(donaConfigs());
        string path = localConfigFolder_FullPath + configFilename;
        // if (!File.Exists(path))
        // {

        // }
        // {
        // GameConfigs _loadConfigs = new GameConfigs();
        // _loadConfigs = JsonUtility.FromJson<GameConfigs>(File.ReadAllText(path));
        // currentConfigs = new GameConfigs();
        // currentConfigs = _loadConfigs;

        // //update UI
        // Components.c.gameUIMan.UpdateUIToConfigs();
        // //update timer values



            //Components.c.filetotext.startUpdates = true;

            // Debug.Log("DIFFERENCE ---------------- : "  + difference.ToString());
            // if(currentPlayer.current_Hearts < currentConfigs.max_Hearts)
            // {
            //     int possibleHeartAddition = (difference / currentConfigs.heart_CoolDown);
            //     if((possibleHeartAddition + currentPlayer.current_Hearts) >= currentConfigs.max_Hearts)
            //     {
            //         currentPlayer.current_Hearts = currentConfigs.max_Hearts;
            //     }
            //     else
            //     {
            //         currentPlayer.current_Hearts += possibleHeartAddition;
            //     }

            //     Debug.Log("possibleskipAddition = " + possibleHeartAddition);
            //     Components.c.gameUIMan.UpdateLifesIndicator();
            // }
            // if(currentPlayer.current_Skips < currentConfigs.max_Skip_Amount)
            // {
            //     int possibleSkipAddition = (difference / currentConfigs.skip_CoolDown);

            //     Debug.Log("possibleskipAddition = " + possibleSkipAddition);
            //     if((possibleSkipAddition + currentPlayer.current_Skips) >= currentConfigs.max_Skip_Amount)
            //     {
            //         currentPlayer.current_Skips = currentConfigs.max_Skip_Amount;
            //     }
            //     else
            //     {
            //         currentPlayer.current_Skips += possibleSkipAddition;
            //     }   
            //     Components.c.gameUIMan.UpdateSkipsIndicator();
            // } 
            // IMPLEMENT NOTIFICATION SYSTEM TO NOTIFY WHEN LIFES/SKIPS ARE REGENERATED
        //}
    }

// update timed stuff from pause - seconds
// have this working as from app launch too, implement it on the pause aswell

    public void UpdateFrom_BetweenPlays(int seconds)
    {
        Components.c.settings.currentPlayer.lastlogin = DateTime.UtcNow.ToString();
        //hearts
        // if(currentPlayer.current_Hearts > currentConfigs.max_Hearts)
        // {
            
        //     return;
        // }

        if(currentPlayer.current_Hearts < currentConfigs.max_Hearts)
        {
            int howManyToAdd = (seconds / currentConfigs.heart_CoolDown);
            Debug.Log("HOW MANY TO ADD INT " + howManyToAdd);
            if((howManyToAdd + currentPlayer.current_Hearts) >= currentConfigs.max_Hearts)
            {
                currentPlayer.current_Hearts = currentConfigs.max_Hearts;
            }else
            {
                currentPlayer.current_Hearts += howManyToAdd;
            }
        }
        //skips
        if(currentPlayer.current_Skips < currentConfigs.max_Skip_Amount)
        {
            int howManyToAdd = (seconds / currentConfigs.skip_CoolDown);
            Debug.Log("HOW MANY TO ADD INT " + howManyToAdd);
            if((howManyToAdd + currentPlayer.current_Skips) >= currentConfigs.max_Skip_Amount)
            {
                currentPlayer.current_Skips = currentConfigs.max_Skip_Amount;
            }else
            {
                currentPlayer.current_Skips += howManyToAdd;
            }
        }
        //update UI
        string saveJson = JsonUtility.ToJson(currentPlayer);
        //File.WriteAllText(localPlayerFolder_fullpath + playerJsonDefaultName, saveJson);
        Debug.Log("saved from pausetime configs");
        Components.c.gameUIMan.UpdateUIToConfigs();
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
        string configJson = JsonUtility.ToJson(currentConfigs);
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
            currentPlayer.playerName = name;
            SavePlayerdDataToFile();
        }
        Components.c.gameUIMan.HideLogin();
        name = "";
    }
    public string locale;
    public void ChangeLocale(int selection)
    {

        Debug.Log("selection = " + selection.ToString());
        if(selection == 0)
        {
            locale = "en-US";
            //LoadLocale(locale);
            Components.c.sampleSpeechToText.SetSettings(locale, .75f,.75f);
            localeScore = Components.c.settings.currentPlayer.enUS_score;
        }
        if(selection == 1)
        {
            //finnish fi-FI
            locale = "fi-FI";
            //load finnish words
            //change LB and player stuff
            //StartCoroutine(MakeFinnishWordJson());
            //LoadLocale(locale);
            //Debug.Log("changed to FINNISH GAME");
            ///SPeak Something to inidicate change
            //Components.c.gameloop.Wait_and_Speak("TERVETULOA REVERSE SPEAK ON NYT SUOMEKSI!");
            ///blabla have load locale from here laterz --- have change to speech recog settigns too
            localeScore = Components.c.settings.currentPlayer.fiFI_score;
            Components.c.sampleSpeechToText.SetSettings(locale, .75f,.75f);

        }
        if(selection == 2)
        {
            locale = "fr-FR";
            //LoadLocale(locale);
            //StartCoroutine(MakeGermanWordJson());
            //englis en-UK  // Setting("en-US");
            localeScore = Components.c.settings.currentPlayer.frFR_score;
            Components.c.sampleSpeechToText.SetSettings(locale, .6f,.75f);
        }
        if(selection == 3)
        {
            locale = "de-DE";
            //StartCoroutine(waitWords());
            //StartCoroutine(MakeFRENCHWordJson());
            //LoadLocale(locale);
            Debug.Log("DE LOLCAL" + selection);
            Debug.Log("MADE NEW GERMAN JSON ------");
            Components.c.sampleSpeechToText.SetSettings(locale, .75f,.75f);
            localeScore = Components.c.settings.currentPlayer.deDE_score;
            //LoadLocale(locale);
            //englis en-UK  // Setting("en-US");
        }
        Debug.Log("SELECTION : "  + selection);
        Components.c.localisedStrings.ChangeLanguage(selection);
        LoadLocale(locale);


        currentPlayer.playerLocale = locale;
        Components.c.speechToText.Setting(locale);
        //set per locale
        

    }
    public int localeScore; 
    //public GameObject fontManager;
}
