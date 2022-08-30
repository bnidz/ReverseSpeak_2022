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
    private void StartAuth()//async Task StartAuth()
    {

        // try{
        //     _localPlayer = await GKLocalPlayer.Authenticate();
        //     // for (int i = 0; i < _localPlayer.friends.Count; i++)
        //     // {
        //     //     Debug.Log(_localPlayer.friends[i].ToString());
        //     // }
        // Debug.Log(_localPlayer.ToString());

        // }catch(Exception exception)
        // {
        //     //handle exception
        // }

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

    
            //check if playerData already exists
            //load the player data to game 
            //save login timestamp
            //save player language
            //skill
            //playtimes



            //first login successs -- load saveplayers --- not ? -> create
            //then load words ->

            //-> then show UI ->


        }
        else{

            Debug.Log("Authentication failed");
        }

        });


    }
 
}
