using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Notifications.iOS;

public class AuthRequestScript : MonoBehaviour
{

    //public Action<string> onAuthCallback;
    // Start is called before the first frame update
    public void Init()
    {
        StartCoroutine(AUTH_DEVICE());
    }
    IEnumerator RequestAuthorization()
    {
        var authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge;
        using (var req = new AuthorizationRequest(authorizationOption, true))
        {
            while (!req.IsFinished)
            {
                yield return null;
            };

            string res = "\n RequestAuthorization:";
            res += "\n finished: " + req.IsFinished;
            res += "\n granted :  " + req.Granted;
            res += "\n error:  " + req.Error;
            res += "\n deviceToken:  " + req.DeviceToken;
            Debug.Log(res);
        }
    }
    // Update is called once per frame

    public bool allAuthed = false;
    public IEnumerator AUTH_DEVICE()
    {
        //SPEECH RECOG AUTH
        Debug.Log("FILETO TEXT INIT");
        Components.c.filetotext.Init();
        Debug.Log("DONE");

        while (!Components.c.filetotext.micConnected) yield return null;
        Debug.Log("MIC AUTH DONE");
        Debug.Log("NOTIF AUTH");
        StartCoroutine(RequestAuthorization());
        Debug.Log("NOTIF AUTH DONE");
        //while (authing) yield return null;
        //MIC AUTH
        while (authing) yield return null;
        allAuthed = true;
    }

    public bool authing = true;
    public void onAuthCallback(string message)
    {
        //FIRST THE SPEECHRECOQ AUTH
        if(message == "N")
        {
            Debug.Log("SPEECH RECVOQ AUTH DONE---- NOT GRANTED ---- AGIAIN PLS");
            authing = true;
        }
        if(message == "Y")
        {
            //continue it was AUTHED
            Debug.Log("speech RECOQ AUTH DONE -- GRANTED");
            authing = false;
        }
    }
}
