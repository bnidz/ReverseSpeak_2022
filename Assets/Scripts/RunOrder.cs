using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunOrder : MonoBehaviour
{
    public void Awake()
    {
        float h = 2;
        float w = 3;

        if (Camera.main.aspect > (h/w))
        {
            Debug.Log("3:2");
            //free aspect
            Screen.autorotateToPortrait = true;
            Screen.autorotateToPortraitUpsideDown = true;
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight   = true;
            Screen.orientation = ScreenOrientation.AutoRotation;
            Debug.Log("4:3");
        }

        FindObjectOfType<Components>().Init();
        blindingPanel.SetActive(true);
    }
    public GameObject blindingPanel;

    public void startLoadComponents()
    {
        StartCoroutine(LoadComponents());
    }
    public IEnumerator LoadComponents()
    {

        Debug.Log("text to speech init");
        Components.c.textToSpeech.Init();
        Debug.Log("text to speech done");

        Components.c.dadabaseManager.Init();
        Debug.Log("auth req init");
        Components.c.auhtRequestScript.Init();
        //CHECK FOR AUTHORIZATIONS FIRTS
        //Components.c.filetotext.Init();
        //Components.c.authRequestScript.Init();
        Debug.Log("waiting ");
        while (!Components.c.auhtRequestScript.allAuthed) yield return null;
        Debug.Log("all  authed next settings init");
        Components.c.settings.Init();
        Debug.Log("settings init done ---->");

        Debug.Log("SAMPLE SPEECHTOTEXT INIT START");
        Components.c.sampleSpeechToText.Init();
        Debug.Log("SAMPLE SPEECHTO TEXT INIT DONE");
        //while (!Components.c.filetotext.micConnected) yield return null;
        //GAMECENTER --- WHICH LOADS LOCAL SAVES
        // or creates

        Debug.Log("GAME MAN INIT START");
        Components.c.gameManager.Init();
        StartCoroutine(delay());
        Debug.Log("GAME MAN INIT DONE");
        Debug.Log("DISPLAY HIGHSCORESINIT");
        Components.c.displayHighScores.Init();
        Debug.Log("HIGHSCORES INIT DONE");
        //blindingPanel.SetActive(false);
        //start only after some value
        //the game is on 
        // have to work clean and with saves
        _continue();

    }
    public void _continue()
    {

        Debug.Log("CONTINUE RUNORDER");
        Debug.Log("GAMELOOP INIT START");
        Components.c.gameloop.Init();
        Debug.Log("GAME LOOP INIT DONE");
        Debug.Log("GAME LOOP NEW RANDOM WORD");
        Components.c.gameloop.NewRandomWORD();
        Debug.Log("start button updates on filetotext");
        Components.c.filetotext.startUpdates = true;
        Components.c.filetotext.canPushButton = true;
        Debug.Log("done - --- -- set starting screen offf ---- ");
        blindingPanel.SetActive(false);
        //Components.c.dadabaseManager.Init();
        //Debug.Log("DADAMAN INIT DONE");
        Debug.Log("GAME DADABASEMAN IIT START");
        Components.c.dadabaseManager.StartUpdateHandler();
        Components.c.appPaused.isActive = true;

    }
    public void StartGame()
    {

        StartCoroutine(delay());
        
        //StartCoroutine(delay());
        //StartCoroutine(delay());
    }

    public IEnumerator delay()
    {
        yield return new WaitForSeconds(3f);
    }
}