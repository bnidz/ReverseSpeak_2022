using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunOrder : MonoBehaviour
{
    public void Init()
    {
        LoadComponents();   
    }

    private void LoadComponents()
    {

        Components.c.settings.Init();
        //Components.c.filereader.Init();
        Components.c.filetotext.Init();
        Components.c.textToSpeech.Init();
        Components.c.gameloop.Init();

        Components.c.gameloop.NewRandomWORD();
    }
}