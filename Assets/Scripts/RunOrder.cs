using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RunOrder : MonoBehaviour
{
    public void Awake()
    {
        blindingPanel.SetActive(true);

        float h = 2;
        float w = 3;
        if (m_StartGameEvent == null)
            m_StartGameEvent = new UnityEvent();

            m_StartGameEvent.AddListener(Ping);
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
        }else
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
        FindObjectOfType<Components>().Init();
        //blindingPanel.SetActive(true);
    }

    public GameObject blindingPanel;
    public void startLoadComponents()
    {
        StartCoroutine(LoadComponents());
    }
    public IEnumerator LoadComponents()
    {

        Components.c.gameManager.StartAuth();
        Debug.Log("text to speech init");
        Components.c.textToSpeech.Init();
        Debug.Log("text to speech done");
        Components.c.dadabaseManager.Init();
        Debug.Log("auth req init");
        //Components.c.auhtRequestScript.Init();
        Debug.Log("waiting 10s");
        //yield return new WaitForSeconds(2.4f);
        Debug.Log("SAMPLE SPEECHTOTEXT INIT START");
        Components.c.sampleSpeechToText.Init();
        yield return StartCoroutine(Components.c.auhtRequestScript.AUTH_DEVICE());
        Debug.Log("all  authed next settings init");
        Components.c.settings.Init();

        /// HOXXXX
        //Components.c.settings.UploadNewDefaultPlayerJson();
        /// HOXXXX
        
        yield return StartCoroutine(Components.c.gameManager.waitTilAuth());
        Debug.Log("GAME MAN INIT DONE");
        //_continue();
    }

    public bool launch = true;
   public UnityEvent m_StartGameEvent;
    public void _continue()
    {

        if(launch)
        {

            Components.c.dadabaseManager.StartUpdateHandler();
            Debug.Log("CONTINUE RUNORDER");
            Debug.Log("GAMELOOP INIT START");
            Components.c.gameloop.Init();
            Debug.Log("start button updates on filetotext");
            Components.c.filetotext.startUpdates = true;
            Components.c.filetotext.canPushButton = true;
            Debug.Log("done - --- -- set starting screen offf ---- ");
            Components.c.appPaused.isActive = true;
            Debug.Log("CHECK THIS");
            Components.c.displayHighScores.Init();
            Components.c.rewardedAdsButton.Init();
            Components.c.gameUIMan.UpdateUIToConfigs();
            //m_StartGameEvent.Invoke();
            launch = false;
            //return;
        }

        Components.c.settings.LoadSplashScreenDefaults();
        
        Components.c.settings.StartGameSplashScreenButton.interactable = true;

    }

    void Ping()
    {
        Components.c.gameUIMan.UpdateUIToConfigs();
        //m_StartGameEvent.Invoke();

        if(Components.c.gameUIMan.settingsMenu.activeInHierarchy)
        {
            Components.c.gameUIMan.settingsMenu.SetActive(false);
        }
        Components.c.gameloop.NewRandomWORD();
    }
}