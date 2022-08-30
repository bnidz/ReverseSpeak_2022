using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunOrder : MonoBehaviour
{
    public void Init()
    {
        LoadComponents();   
    }

    public GameObject blindingPanel;

    private void LoadComponents()
    {
        blindingPanel.SetActive(true);

        Components.c.settings.Init();
        Components.c.filetotext.Init();
        Components.c.textToSpeech.Init();
        //Components.c.gameManager.Init();


        //Components.c.gameloop.Init();
        //Components.c.filereader.Init();
        //Components.c.gameloop.NewRandomWORD();
    }
}