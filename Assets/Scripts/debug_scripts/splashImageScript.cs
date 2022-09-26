using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class splashImageScript : MonoBehaviour
{

    public void SplahsEventClicked(PointerEventData eventData)
    {

        //aka start game
        Components.c.gameloop.NewRandomWORD();
        Components.c.settings.CloseBlindingPanel();
    }


  
    public void SplahsEventClicked_()
    {

        //aka start game
        Components.c.gameloop.NewRandomWORD();
        Components.c.settings.CloseBlindingPanel();


        //close menu if open
        if(Components.c.gameUIMan.settingsMenu.activeInHierarchy)
        {
          Components.c.gameUIMan.settingsMenu.SetActive(false);
        }

    }
}