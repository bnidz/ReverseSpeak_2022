using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GenerateJsonFile : MonoBehaviour
{
    //Do automatic failsave for existing json and for LoadWords too
    private FileReader fr;
    private SaveData sData;
    private WordData wordData;

    private string jsonString;
    private bool firstEntry = true;

    // Start is called before the first frame update
    void Start()
    {
        fr =  FindObjectOfType<FileReader>();
        sData =  FindObjectOfType<SaveData>();
        Debug.Log(Application.streamingAssetsPath);

        //for oneshot test
        StartCoroutine(new_MakeJsonFromFile());
    }

    public float timeElapsed = 0;
    private bool generatingJson = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            generatingJson = true;
            MakeJsonFromFile();
        }
        if(generatingJson)
        {
            timeElapsed += Time.deltaTime;
        }
    }

    public void MakeJsonFromFile()
    {
        for (int i = 0; i < fr.eng_words.Count; i++)
        {

            wordData = new WordData();
            wordData.word = fr.eng_words[i].ToString().ToUpper();
            wordData.timesTried = 0;
            wordData.timesSkipped = 0;
            wordData.timesQuessed = 0;
            wordData.totalScore = 0;
            wordData.avgScore = 0;
            jsonString += JsonUtility.ToJson(wordData) + "|";
        }

        File.WriteAllText(Application.streamingAssetsPath + "/english3000_5.json", jsonString);
        generatingJson = false;
        Debug.Log("time to done json :" + timeElapsed);
    }

    [System.Serializable]
    public class WrappingClass
    {
        public List<WordData> Allwords;
    }

    public List<WordData> tempList;
    public IEnumerator new_MakeJsonFromFile()
    {
        //new gameobject list from the new json objects
        //new json from gameobject list
        yield return new WaitUntil(() => fr.done == true);

        for (int i = 0; i < fr.eng_words.Count; i++)
        {
            wordData = new WordData();

            wordData.word = fr.eng_words[i].ToString().ToUpper();
            //wordData.object.name = wordData.word;

            wordData.timesTried = 0;
            wordData.timesSkipped = 0;
            wordData.timesQuessed = 0;
            wordData.totalScore = 0;
            wordData.avgScore = 0;

            tempList.Add(wordData);
        }

        var allWords = new WrappingClass() { Allwords = tempList };
        string allWordData = JsonUtility.ToJson(allWords);

        File.WriteAllText(Application.streamingAssetsPath + "/new_format_test.json", allWordData); 
        generatingJson = false;
        Debug.Log("done with new json test!");
    }

}