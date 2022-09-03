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

    public void LoadComponents()
    {
        Components.c.textToSpeech.Init();
        Components.c.filetotext.Init();
        Components.c.settings.Init();
        Components.c.gameManager.Init();
        Components.c.gameloop.Init();
        //Components.c.highScores.Init();
        Components.c.dadabaseManager.Init();
        blindingPanel.SetActive(false);
    }
    //  private void Update() {
    

    //     Debug.Log("ASPECT. "  + Camera.main.aspect);
    // }
}