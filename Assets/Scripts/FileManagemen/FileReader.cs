using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileReader : MonoBehaviour
{
    int counter = 0;
    string line;
    
    public string eng_filename;
    public List<string> words_from_txt_file;
    public List<string> eng_words;
    public bool done = false;
    public List<Word> allWords = new List<Word>();
    public List<Word> newWords;
    public List<WordClass> _allWords;

    public bool isDoing = true;
    public void MakeNewWordItems(string filename)
    {     
        string filepath = Path.Combine(Application.streamingAssetsPath, filename);
        // Read the file and display it line by line.  
        StreamReader file = new StreamReader(filepath);
        while ((line = file.ReadLine()) != null)
        {
            // System.Console.WriteLine(line);
            words_from_txt_file.Add(line);
            counter++;
        }

        file.Close();
        Debug.Log("words loaded: " + counter);
        //done = true;
        //// Create30lists();

       // const string glyphs= "öäåüÿẞçéàèùâêîôûëïü";
        for (int i = 0; i < words_from_txt_file.Count; i++)
        {
            Word w = new Word{
            word = words_from_txt_file[i],
            times_tried = 0,
            times_right = 0,
            times_skipped = 0,
            total_score = 0,
            avg_score = 0,
            };

            newWords.Add(w);
        }
        isDoing = false;


        Components.c.fireStore_Manager.Upload_all_worsd(newWords);

        // var allWords = new WrappingClass() { Allwords = _allWords };
        // string allWordData = JsonUtility.ToJson(allWords);
        // Debug.Log(allWordData);
        // //File.WriteAllText(Components.c.settings.localWordsFolder_fullpath + "WordsJson_" +filename + ".json", allWordData); 
        // //string json = JsonUtility()
        // _allWords.Clear();
        //eng_words.Clear();

    }

    //     for (int i = 0; i < eng_words.Count; i++)
    //     {
    //         WordClass wordclass = new WordClass();

    //         wordclass.word = eng_words[i].ToUpper();Debug.Log("");
    //         wordclass.times_tried = 0;
    //         wordclass.times_right = 0;
    //         wordclass.times_skipped = 0;
    //         wordclass.total_score = 0;
    //         wordclass.avg_score = 0;

    //         allWords.Add(wordclass);
    //     }
    //     Debug.Log("new wordclass item count = " + allWords.Count);
    

    public void DoAllSpecialWords()
    {

       //string f1 = "english3000.txt";
       string f2 = "finnish_1000.txt";
       string f3 = "1000_deutch.txt";
       string f4 = "fr_1000.txt";

        //MakeNewWordItems(f1);
        MakeNewWordItems(f2);
        MakeNewWordItems(f3);
        MakeNewWordItems(f4);
 
    }


    // public void DoAndWrite50_word_sets()
    // {

    //     for (int y = 0; y < eng_words.Count; y++)
    //     {
            
    //     }
    //     for (int i = 0; i < 50; i++)
    //     {
    //         WordClass wordclass = new WordClass();

    //         wordclass.word = eng_words[UnityEngine.Random.Range(1,2999)].ToUpper();
    //         wordclass.times_tried = 0;
    //         wordclass.times_right = 0;
    //         wordclass.times_skipped = 0;
    //         wordclass.total_score = 0;
    //         wordclass.avg_score = 0;

    //         allWords.Add(wordclass);
    //     }

    // }

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