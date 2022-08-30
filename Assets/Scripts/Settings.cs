using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

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

            string playerJson = JsonUtility.ToJson(playerClass);
            File.WriteAllText(localPlayerFolder_fullpath + "PlayerJson.json", playerJson); 
            Debug.Log("Generated fresh PlayerJson");
            currentPlayer = playerClass;
            currentPlayer.lastLogin = System.DateTime.Now.ToString();

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
                currentPlayer.lastLogin;


            
            LoadSavedWordSettings();
            LoadDefaultConfigs();
            return;
        }
        else
        {
            PlayerClass playerClass = new PlayerClass();
            playerClass = JsonUtility.FromJson<PlayerClass>(File.ReadAllText(path));
            //gameWords = allwordsClass.Allwords;
            currentPlayer = playerClass;
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
                currentPlayer.totalTries;

            LoadSavedWordSettings();
            LoadDefaultConfigs();

            Debug.Log("player class loaded from file");
        }
    }

    public void SavePlayerdDataToFile()
    {
        string playerJson = JsonUtility.ToJson(currentPlayer);
        File.WriteAllText(localPlayerFolder_fullpath + playerJsonDefaultName, playerJson);

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
            currentPlayer.totalTries;
            
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

        if (File.Exists(path))
        {
            PlayerClass playerClass = new PlayerClass();
            int charAmount = Random.Range(6, 12); //set those to the minimum and maximum length of your string
            for(int i=0; i<charAmount; i++)
            {
                playerClass.playerName += glyphs[Random.Range(0, glyphs.Length)];
            }
            //playerClass.playerName = name;
            playerClass.playerID = playerClass.playerName;
            playerClass.playTimesCount = 1;

            playerClass.totalScore = 0;
            playerClass.timesQuessed = 0;
            playerClass.timesSkipped = 0;
            playerClass.totalTries = 0;

            string playerJson = JsonUtility.ToJson(playerClass);
            File.WriteAllText(localPlayerFolder_fullpath + playerClass.playerName +".json", playerJson);
            playerJsonDefaultName =  playerClass.playerName +".json";
            Debug.Log("Generated fresh PlayerJson");
            currentPlayer = playerClass;
            //SavePlayerdDataToFile();
        }
    }

    public GameConfigs currentConfigs;
    public void GenerateDefaultConfigs()
    {
        currentConfigs = new GameConfigs();
        currentConfigs.configVersion = 0.1f;
        currentConfigs.max_Skip_Amount = 10;
        currentConfigs.max_Hearts = 10;
        currentConfigs.heart_CoolDown = 3 * 60; //sec
        currentConfigs.skip_CoolDown = 2 * 60; //sec

        currentConfigs.current_Hearts = currentConfigs.max_Hearts;
        currentConfigs.current_Skips = currentConfigs.max_Skip_Amount;

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
        }
    }

    public void SavePlayerConfigs()
    {
        string configJson = JsonUtility.ToJson(currentConfigs);
        File.WriteAllText(localConfigFolder_FullPath + configFilename, configJson); 
    }
}
