using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    [SerializeField] private WordData _wordData = new WordData();

    public void SaveIntoJson()
    {
        string word = JsonUtility.ToJson(_wordData);
        System.IO.File.WriteAllText(Application.streamingAssetsPath + "/WordData_english_3000.json", word);
    }
}

[System.Serializable]
public class WordData
{
    public string word;

    public int timesTried;
    public int timesSkipped;
    public int timesQuessed;

    public float totalScore;
    public float avgScore;


    public static WordData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<WordData>(jsonString);
    }

}