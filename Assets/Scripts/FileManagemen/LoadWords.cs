using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadWords : MonoBehaviour
{

    public List<WordData> gameWordsList;
    private string filePath;
    public string json_file_name;
    public bool loadWords;

    void Init()
    {
        filePath = Application.streamingAssetsPath + "/" + json_file_name +".json";
        string fileContents = File.ReadAllText(filePath);

    //for now to make the build usable - later fetch the words.json from the server
        if(loadWords)
        GenerateGameWords(fileContents);
    }

    GenerateJsonFile.WrappingClass wrapping;
    public void GenerateGameWords(string jsonString)
    {

        jsonString = jsonString.Substring(0, jsonString.Length - 1);
        gameWordsList.Clear();

        string[] wordDataString = jsonString.Split('|');
        //wordDataString[wordDataString.Length]

        foreach (string wData in wordDataString)
        {
            //WordData wordData = JsonUtility.FromJson<WordData>(JsonUtility.ToJson(wData));
            WordData wordData = WordData.CreateFromJSON(wData);
            gameWordsList.Add(wordData);
        }
        //loadWords = true;
    }

    public void new_GenerateGameWords(string jsonString)
    {

        gameWordsList.Clear();
        wrapping = JsonUtility.FromJson<GenerateJsonFile.WrappingClass>(jsonString);

        foreach (var word in wrapping.Allwords)
        {
            gameWordsList.Add(word);
        }

    }
}
