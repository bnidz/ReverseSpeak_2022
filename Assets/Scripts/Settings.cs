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
    private string playerJsonDefaultName = "PlayerJson.json";


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

        StartCoroutine(RequestAuthorization());
    }
    IEnumerator RequestAuthorization()
    {
        var authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge;
        using (var req = new AuthorizationRequest(authorizationOption, true))
        {
            while (!req.IsFinished)
            {
                yield return null;
            };

            string res = "\n RequestAuthorization:";
            res += "\n finished: " + req.IsFinished;
            res += "\n granted :  " + req.Granted;
            res += "\n error:  " + req.Error;
            res += "\n deviceToken:  " + req.DeviceToken;
            Debug.Log(res);
        }
    }



    //also load and save player stats
    //leaderboard data

    public void LoadSavedWordSettings()
    {
        string path = localWordsFolder_fullpath + "WordsJson.json";
        if (!File.Exists(path))
        {
            Components.c.filereader.MakeNewWordItems();
            gameWords = Components.c.filereader.allWords;
            var allWords = new WrappingClass() { Allwords = gameWords };
            string allWordData = JsonUtility.ToJson(allWords);

            File.WriteAllText(localWordsFolder_fullpath + "WordsJson.json", allWordData); 
            Debug.Log("Generated fresh wordlist");
            return;
        }
        else
        {
            WrappingClass allwordsClass = new WrappingClass(); 
            allwordsClass = JsonUtility.FromJson<WrappingClass>(File.ReadAllText(path));
            gameWords = allwordsClass.Allwords;
            Debug.Log(path);
        }
    }

    public void LoadSavedPlayerSettings(string name, string id)
    {
        string path = localPlayerFolder_fullpath + "PlayerJson.json";
        if (!File.Exists(path))
        {

            PlayerClass playerClass = new PlayerClass();
            playerClass.playerName = name;
            playerClass.playerID = id;
            playerClass.playTimesCount = 1;

            playerClass.totalScore = 0;
            playerClass.timesQuessed = 0;
            playerClass.timesSkipped = 0;
            playerClass.totalTries = 0;

            playerClass.current_Hearts = 0;
            playerClass.current_Skips = 0;

            playerClass.lastlogin = DateTime.UtcNow.ToString();
            string playerJson = JsonUtility.ToJson(playerClass);
            currentPlayer = playerClass;

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
                "lastlogin: " +
                currentPlayer.lastlogin.ToString();


            File.WriteAllText(localPlayerFolder_fullpath + "PlayerJson.json", playerJson); 
            Debug.Log("Generated fresh PlayerJson");
            difference = 0;
            
            // LoadSavedWordSettings();
            // LoadDefaultConfigs();
            // return;

            Debug.Log("----------------------------- NEW PLAYER JSNONN");
        }
        else
        {

            Debug.Log("-----------------------------OLD PLAYER JSON LOADED");
            PlayerClass playerClass = new PlayerClass();
            playerClass = JsonUtility.FromJson<PlayerClass>(File.ReadAllText(path));
            //gameWords = allwordsClass.Allwords;
            currentPlayer = playerClass;
            currentPlayer.playTimesCount++;

            //DateTime dif = (DateTime.UtcNow - currentPlayer.lastlogin).TotalSeconds;
            TimeSpan _difference = DateTime.UtcNow.Subtract(DateTime.Parse(currentPlayer.lastlogin));
            double __difference = Math.Floor(_difference.TotalSeconds);
            Debug.Log("double floor total seconds : " + __difference);
            difference = Convert.ToInt32(__difference);
            Debug.Log("int difference : " + difference);
            Debug.Log("DIFFERENCE SUBSTRACKT   :   " + difference);
            currentPlayer.lastlogin = DateTime.UtcNow.ToString();

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
                difference.ToString();
            //SavePlayerdDataToFile();

            Debug.Log("player class loaded from file");
        }

        LoadSavedWordSettings();
        LoadDefaultConfigs();
    }
    private int difference;
    public void SavePlayerdDataToFile()
    {
        currentPlayer.lastlogin = DateTime.UtcNow.ToString();
        string playerJson = JsonUtility.ToJson(currentPlayer);
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
            //currentPlayer.UID.ToString();
            Components.c.dadabaseManager.getUIDraw();
            //upload new score to LB
            //Components.c.highScores.UploadScore(currentPlayer.playerName ,currentPlayer.totalScore);
            //how to make reference to user ID 
            if(currentPlayer.UID.Length < 1)
            {
                currentPlayer.UID = GenerateUUID.UUID();
            }

            Components.c.dadabaseManager.Update_LB_UserEntry(currentPlayer.playerID, currentPlayer.playerName,currentPlayer.totalScore, currentPlayer.UID);

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
            PlayerClass playerClass = new PlayerClass();
            int charAmount = UnityEngine.Random.Range(6, 12); //set those to the minimum and maximum length of your string
            for(int i=0; i<charAmount; i++)
            {
                playerClass.playerName += glyphs[UnityEngine.Random.Range(0, glyphs.Length)];
            }
            //playerClass.playerName = name;
            playerClass.playerID = playerClass.playerName;
            playerClass.playTimesCount = 1;

            playerClass.totalScore = 0;
            playerClass.timesQuessed = 0;
            playerClass.timesSkipped = 0;
            playerClass.totalTries = 0;
            playerClass.current_Hearts = 0;
            playerClass.current_Skips = 0;
            playerClass.lastlogin =  DateTime.UtcNow.ToString();

            string playerJson = JsonUtility.ToJson(playerClass);
            //File.WriteAllText(localPlayerFolder_fullpath + playerClass.playerName +".json", playerJson);
            File.WriteAllText(path, playerJson);

            playerJsonDefaultName =  playerClass.playerName +".json";
            Debug.Log("Generated fresh PlayerJson");
            currentPlayer = playerClass;
            //SavePlayerdDataToFile();
        }
    }
    // public void populateLB(int howmany)
    // {
    //     StartCoroutine(PopulateLeaderBoards(250));
    // }
    // public IEnumerator PopulateLeaderBoards(int count)
    // {
    //     for (int i = 0; i < count; i++)
    //     {
    //         int scroe = UnityEngine.Random.Range(0, 10000); 
            
    //         int charAmount = UnityEngine.Random.Range(6, 12); //set those to the minimum and maximum length of your string
    //         for(int y=0; i<charAmount; y++)
    //         {
    //             p_name += glyphs[UnityEngine.Random.Range(0, glyphs.Length)];
    //         }
    //         Components.c.dadabaseManager.Update_LB_UserEntry(name, name, scroe);
    //         p_name = "";
    //         yield return new WaitForSeconds(.02f);
    //     }
    // }
    private string p_name;

    public GameConfigs currentConfigs;
    public void GenerateDefaultConfigs()
    {
        currentConfigs = new GameConfigs();
        currentConfigs.configVersion = 1;
        currentConfigs.max_Skip_Amount = 15;
        currentConfigs.max_Hearts = 10;
        currentConfigs.heart_CoolDown = 60; //sec
        currentConfigs.skip_CoolDown = 30; //sec

        string defconfigs = JsonUtility.ToJson(currentConfigs);
        File.WriteAllText(localConfigFolder_FullPath + configFilename, defconfigs);
        Debug.Log("Generated def configs");
        //update UI
        Components.c.gameUIMan.UpdateUIToConfigs();
        //update timer values
        Components.c.filetotext.skipCoolDown = Components.c.settings.currentConfigs.skip_CoolDown;
        Components.c.filetotext.heartCoolDown = Components.c.settings.currentConfigs.heart_CoolDown;
        Components.c.filetotext.startUpdates = true;
    }

    public void LoadDefaultConfigs()
    {
        string path = localConfigFolder_FullPath + configFilename;
        if (!File.Exists(path))
        {
            GenerateDefaultConfigs();
            return;

        }else
        {
            GameConfigs _loadConfigs = new GameConfigs();
            _loadConfigs = JsonUtility.FromJson<GameConfigs>(File.ReadAllText(path));
            currentConfigs = new GameConfigs();
            currentConfigs = _loadConfigs;

            //update UI
            Components.c.gameUIMan.UpdateUIToConfigs();
            //update timer values
            Components.c.filetotext.skipCoolDown = Components.c.settings.currentConfigs.skip_CoolDown;
            Components.c.filetotext.heartCoolDown = Components.c.settings.currentConfigs.heart_CoolDown;
            Components.c.filetotext.startUpdates = true;

            Debug.Log("DIFFERENCE ---------------- : "  + difference.ToString());
            if(currentPlayer.current_Hearts < currentConfigs.max_Hearts)
            {
                int possibleHeartAddition = (difference / currentConfigs.heart_CoolDown);
                if((possibleHeartAddition + currentPlayer.current_Hearts) >= currentConfigs.max_Hearts)
                {
                    currentPlayer.current_Hearts = currentConfigs.max_Hearts;
                }
                else
                {
                    currentPlayer.current_Hearts += possibleHeartAddition;
                }

                Debug.Log("possibleskipAddition = " + possibleHeartAddition);
                Components.c.gameUIMan.UpdateLifesIndicator();
            }
            if(currentPlayer.current_Skips < currentConfigs.max_Skip_Amount)
            {
                int possibleSkipAddition = (difference / currentConfigs.skip_CoolDown);

                Debug.Log("possibleskipAddition = " + possibleSkipAddition);
                if((possibleSkipAddition + currentPlayer.current_Skips) >= currentConfigs.max_Skip_Amount)
                {
                    currentPlayer.current_Skips = currentConfigs.max_Skip_Amount;
                }
                else
                {
                    currentPlayer.current_Skips += possibleSkipAddition;
                }   
                Components.c.gameUIMan.UpdateSkipsIndicator();
            } 
            // IMPLEMENT NOTIFICATION SYSTEM TO NOTIFY WHEN LIFES/SKIPS ARE REGENERATED
        }
    }

// update timed stuff from pause - seconds
    public void UpdateFromPauseTime(int seconds)
    {
        //hearts
        if(currentPlayer.current_Hearts < currentConfigs.max_Hearts)
        {
            int howManyToAdd = (seconds / currentConfigs.heart_CoolDown);
            Debug.Log("HOW MANY TO ADD INT " + howManyToAdd);
            if(howManyToAdd >= currentConfigs.max_Hearts)
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
            if(howManyToAdd >= currentConfigs.max_Skip_Amount)
            {
                currentPlayer.current_Skips = currentConfigs.max_Skip_Amount;
            }else
            {
                currentPlayer.current_Skips += howManyToAdd;
            }
        }
        //update UI
        string defconfigs = JsonUtility.ToJson(currentConfigs);
        File.WriteAllText(localConfigFolder_FullPath + configFilename, defconfigs);
        Debug.Log("saved from pausetime configs");
        Components.c.gameUIMan.UpdateUIToConfigs();
    }


// /// timed notifications 
    private bool thereIsActiveNotification_hearts = false;
    public void ScheduledNotification_HeartsFull(int total_seconds)
    {
        TimeSpan t = TimeSpan.FromSeconds( total_seconds );

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
}
