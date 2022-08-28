using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class Settings : MonoBehaviour
{

    public string localWordsFolder;
    public string localWordsFolder_fullpath;
    public List<WordClass> gameWords; 

    public string activeWORD;

    public void Init()
    {
        localWordsFolder_fullpath = Application.persistentDataPath + localWordsFolder;
        if (!Directory.Exists(localWordsFolder_fullpath))
        {
            //if it doesn't, create it
            Directory.CreateDirectory(localWordsFolder_fullpath);
            Debug.Log("directory " + localWordsFolder_fullpath + " created");
        }
        LoadSavedSettings();
    }

    //also load and save player stats
    //leaderboard data

    public void LoadSavedSettings()
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

    [System.Serializable]
    public class WrappingClass
    {
        public List<WordClass> Allwords;
    }
}
