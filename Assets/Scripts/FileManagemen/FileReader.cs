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
        string filepath = Path.Combine(Application.streamingAssetsPath, eng_filename);
        // Read the file and display it line by line.  
        StreamReader file = new StreamReader(filepath);
        while ((line = file.ReadLine()) != null)
        {
            // System.Console.WriteLine(line);
            eng_words.Add(line);
            counter++;
        }

        file.Close();
        Debug.Log("words loaded: " + counter);
        done = true;

        for (int i = 0; i < eng_words.Count; i++)
        {
            WordClass wordclass = new WordClass();
            wordclass.word = eng_words[i];
            allWords.Add(wordclass);
        }
        Debug.Log("new wordclass item count = " + allWords.Count);
    }
}