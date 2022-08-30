using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;

public class Components : MonoBehaviour
{
    public static Components c;
    //list all components and declare on init
    public RunOrder runorder;
    public FileReader filereader;
    public LoadWords loadwords;
    public Settings settings;
    public FileToText filetotext;
    public GameLoop gameloop;
    public TextToSpeech textToSpeech;
    public GameUIMan gameUIMan;
    public SpeechToText speechToText;

    public SampleSpeechToText sampleSpeechToText;
    public GameManager gameManager;

    public void Init()
    {
        if (c == null)
        {
            c = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (c != this)
        {
            Destroy(gameObject);
        }
        InitializeComponents();
    }

    public void InitializeComponents()
    {
        FindObjectOfType<TextToSpeech>().Init();
        settings = FindObjectOfType<Settings>();
        runorder = FindObjectOfType<RunOrder>();
        filereader = FindObjectOfType<FileReader>();
        loadwords = FindObjectOfType<LoadWords>();
        filetotext = FindObjectOfType<FileToText>();
        gameloop = FindObjectOfType<GameLoop>();
        speechToText = FindObjectOfType<SpeechToText>();
        //Init
        gameManager = FindObjectOfType<GameManager>();
        sampleSpeechToText = FindObjectOfType<SampleSpeechToText>();
        gameUIMan = FindObjectOfType<GameUIMan>();

        //runorder.Init();
    

    }
}