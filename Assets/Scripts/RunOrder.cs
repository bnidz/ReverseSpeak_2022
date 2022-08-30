using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunOrder : MonoBehaviour
{
    public void Awake()
    {
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
        blindingPanel.SetActive(false);

    }
}