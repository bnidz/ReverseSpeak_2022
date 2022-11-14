using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FireBaseConnectionCheck : MonoBehaviour
{

    public Button Ok_Button;
    public void Init()
    {
        Debug.Log("internet check init");
        Ok_Button.interactable = true;

    }
    public void TestInternet()
    {
        StartCoroutine(CheckRoutine());

            if(Components.c.gameUIMan.DG_noInternet.activeInHierarchy)
            {
                //Components.c.gameUIMan.DG_noInternet.SetActive(false);
                Ok_Button.interactable = false;
            }
    }

    public bool internets = false;
    public IEnumerator CheckRoutine()
    {
        UnityWebRequest request = new UnityWebRequest("https://www.google.com/");
        yield return request.SendWebRequest();

        if (string.IsNullOrEmpty(request.error))
        {
            //yes has internets
            if(Components.c.gameUIMan.DG_noInternet.activeInHierarchy)
            {
                Ok_Button.interactable = true;
                Components.c.gameUIMan.DG_noInternet.SetActive(false);
            }
            internets = true;
        }
        else
        {
            //no nets connections
            //Components.c.gameUIMan.dg
            if(!Components.c.gameUIMan.DG_noInternet.activeInHierarchy)
            {
                Components.c.gameUIMan.DG_noInternet.SetActive(true);

            }
            Ok_Button.interactable = true;
            internets = false;
        }
    }
}