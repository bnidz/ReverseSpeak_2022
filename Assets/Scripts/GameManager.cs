using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apple.GameKit;
using System.Threading.Tasks;
using System;
using UnityEngine.SocialPlatforms;

public class GameManager : MonoBehaviour
{
    //public GKLocalPlayer _localPlayer;
    public void Init()
    {
        StartAuth();
    }
    // Start is called before the first frame update
    private void StartAuth()
    {

#if UNITY_EDITOR
//pisssdsd
#endif
        Social.localUser.Authenticate(success => {
            if (success)
            {
                Debug.Log("Authentication successful");
                string userInfo = "Username: " + Social.localUser.userName +
                    "\nUser ID: " + Social.localUser.id +
                    "\nIsUnderage: " + Social.localUser.underage;

                Debug.Log(userInfo);
                Components.c.settings.LoadSavedPlayerSettings(Social.localUser.userName, Social.localUser.id);
            }
            else
            {
                    Debug.Log("Authentication failed");
            }
        });
    } 
}
