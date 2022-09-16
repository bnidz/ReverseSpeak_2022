using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileReader : MonoBehaviour
{
    int counter = 0;
    string line;

    public string eng_filename;
    public List<string> eng_words;
    public bool done = false;
    public List<WordClass> allWords;

    public void MakeNewWordItems()
    {    
    //     string filepath = Path.Combine(Application.streamingAssetsPath, eng_filename);
    //     // Read the file and display it line by line.  
    //     StreamReader file = new StreamReader(filepath);
    //     while ((line = file.ReadLine()) != null)
    //     {
    //         // System.Console.WriteLine(line);
    //         eng_words.Add(line);
    //         counter++;
    //     }

    //     file.Close();
    //     Debug.Log("words loaded: " + counter);
    //     done = true;
    //    // Create30lists();
    //     // for (int i = 0; i < 50; i++)
    //     // {
    //     //     WordClass wordclass = new WordClass();

    //     //     wordclass.word = eng_words[UnityEngine.Random.Range(1,2999)].ToUpper();
    //     //     wordclass.times_tried = 0;
    //     //     wordclass.times_right = 0;
    //     //     wordclass.times_skipped = 0;
    //     //     wordclass.total_score = 0;
    //     //     wordclass.avg_score = 0;

    //     //     allWords.Add(wordclass);
    //     // }

    //     for (int i = 0; i < eng_words.Count; i++)
    //     {
    //         WordClass wordclass = new WordClass();

    //         wordclass.word = eng_words[i].ToUpper();
    //         wordclass.times_tried = 0;
    //         wordclass.times_right = 0;
    //         wordclass.times_skipped = 0;
    //         wordclass.total_score = 0;
    //         wordclass.avg_score = 0;

    //         allWords.Add(wordclass);
    //     }
    //     Debug.Log("new wordclass item count = " + allWords.Count);

    

    }

    public void DoAndWrite50_word_sets()
    {

        for (int y = 0; y < eng_words.Count; y++)
        {
            
        }
        for (int i = 0; i < 50; i++)
        {
            WordClass wordclass = new WordClass();

            wordclass.word = eng_words[UnityEngine.Random.Range(1,2999)].ToUpper();
            wordclass.times_tried = 0;
            wordclass.times_right = 0;
            wordclass.times_skipped = 0;
            wordclass.total_score = 0;
            wordclass.avg_score = 0;

            allWords.Add(wordclass);
        }

    }



    [System.Serializable]
    public class WrappingClass
    {
        public List<WordClass> Allwords;
    }
  public List<string> temp_100_eng;
 public List<WordClass> tempWords;


    public void Create30lists()
    {

        for (int y = 0; y < 30; y++)
        {
            for (int i = 0; i < 99; i++)
            {
                int index = Random.Range(0, eng_words.Count);
                string randomWord = eng_words[index];
                eng_words.RemoveAt(index);
                temp_100_eng.Add(randomWord);
            }
            for (int x = 0; x < temp_100_eng.Count; x++)
            {
                WordClass wordclass = new WordClass();

                wordclass.word = temp_100_eng[x].ToUpper();
                wordclass.times_tried = 0;
                wordclass.times_right = 0;
                wordclass.times_skipped = 0;
                wordclass.total_score = 0;
                wordclass.avg_score = 0;

                tempWords.Add(wordclass);
            }

         
        var tempWordz = new WrappingClass() { Allwords = tempWords};
        string allWordData = JsonUtility.ToJson(tempWordz);

        File.WriteAllText(Application.streamingAssetsPath + "/word_set_" +y.ToString()+ ".json", allWordData);    
        tempWords.Clear();

        Debug.Log(Application.streamingAssetsPath + "/word_set_" +y.ToString() + "PATH!!!");
        }
    }
}