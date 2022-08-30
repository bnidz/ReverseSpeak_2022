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
    public QuessLoop quessloop;
    public Settings settings;
    public FileToText filetotext;
    public GameLoop gameloop;
    public TextToSpeech textToSpeech;

    public GameManager gameManager;

    public void Awake()
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
        settings = FindObjectOfType<Settings>();
        runorder = FindObjectOfType<RunOrder>();
        filereader = FindObjectOfType<FileReader>();
        loadwords = FindObjectOfType<LoadWords>();
        quessloop = FindObjectOfType<QuessLoop>();
        filetotext = FindObjectOfType<FileToText>();
        gameloop = FindObjectOfType<GameLoop>();
        //Init
        textToSpeech = FindObjectOfType<TextToSpeech>();
        gameManager = FindObjectOfType<GameManager>();

        runorder.Init();
    

    }
}