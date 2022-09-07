using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apple.GameKit;
using System.Threading.Tasks;
using System;
using UnityEngine.SocialPlatforms;
using System.Threading;


public class GameManager : MonoBehaviour
{
    //public GKLocalPlayer _localPlayer;
    public void Init()
    {
        //StartAuth();
        StartCoroutine(waitTilAuth());
    }

    // Start is called before the first frame update
    private string gc_id;
    private bool isDone = false;
    private string gc_name;
    private void StartAuth()
    {
#if UNITY_EDITOR
//pisssdsd
#endif

    Social.localUser.Authenticate(success => {
    // --- ...    
        if (success)
        {
        
            Debug.Log("Authentication successful");
            string userInfo = "Username: " + Social.localUser.userName +
                "\nUser ID: " + Social.localUser.id +
                "\nIsUnderage: " + Social.localUser.underage;

            gc_id =  Social.localUser.id;
            gc_name = Social.localUser.userName;
            Debug.Log(userInfo);
            isDone = true;

            //return;
            //Components.c.dadabaseManager.CheckIfPlayerClassExists(Social.localUser.id);
            //Components.c.settings.LoadSavedPlayerSettings(Social.localUser.userName, Social.localUser.id);

        }
        else
        {
            Debug.Log("Authentication failed");
            isDone = true;
        }
    });

    Debug.Log("TASK TEST COMPLETE");
    }

    public IEnumerator waitTilAuth()
    {

        StartAuth();
        while (!isDone) yield return null;
        Debug.Log("AUTH DONE!!!");
        Components.c.dadabaseManager.isDone = false;
        Components.c.dadabaseManager.CheckIfPlayerClassExists(gc_id);
        while (!Components.c.dadabaseManager.isDone) yield return null;
        Debug.Log(" CHECKED FOR EXISTING DB SAVE!!!");

        if(Components.c.dadabaseManager.isDB_save)
        {
            //blablabla
            Debug.Log("PLAYER LOADED FROM DADABASE");
            //Components.c.dadabaseManager.isDone = false;
            // Components.c.dadabaseManager.CheckIfPlayerClassExists(gc_id);
            // while (!Components.c.dadabaseManager.isDone) yield return null;
            Components.c.settings.LoadSavedPlayerSettings(gc_name, gc_id);
            Components.c.dadabaseManager.isDone = false;
            // Debug.Log( 
            //     "name : " +
            //     currentPlayer.playerName
            //     +"\n" +
            //     "player ID: " +
            //     currentPlayer.playerID
            //     +"\n" + 
            //     "playtimes: " +
            //     currentPlayer.playTimesCount
            //     +"\n" + 
            //     "p_totalScore: " +
            //     currentPlayer.totalScore
            //     +"\n" +  
            //     "times quessed: " +
            //     currentPlayer.timesQuessed
            //     "times skipped: " +            
            //     currentPlayer.timesSkipped
            //     +"\n"+
            //     "total tries: "+
            //     currentPlayer.totalTries
            //     +"\n"+
            //     "lastlogin: "+
            //     +"\n"+ 
            //     currentPlayer.lastlogin.ToString());

            Components.c.runorder._continue();
            //
        }

        if(!Components.c.dadabaseManager.isDB_save)
        {
            //blablabla
            Debug.Log("PLAYER NOT IN DADABASE");
            // FETCH DEFAULT FROM DB
            //Components.c.dadabaseManager.isDone = false;

            Components.c.dadabaseManager.FetchDefaultPlayerClass();
            while (!Components.c.dadabaseManager.donaDone) yield return null;
            Debug.Log("RETRIEVED PLAYERCLASS FROM DB");
            //Components.c.dadabaseManager.isDone = false;
            Components.c.settings.MakeNewFromDBDefaultWith_GC_ID(gc_id, gc_name);

            while (!Components.c.settings.isDone) yield return null;
            //omponents.c.settings.isDone = false;
            Components.c.dadabaseManager.UploadNewPlayerTo_DB(Components.c.settings.currentPlayer);
            while (!Components.c.dadabaseManager.uploadDone) yield return null;
            Debug.Log("WROTE NEW PLAYER JSON");

            Components.c.settings.LoadSavedWordSettings();
            Components.c.settings.LoadDefaultConfigs();


            //MAKE NEW WITH ID
            //CHECK
            //HAVE IT LOAD TOO --- OK
            Components.c.runorder._continue();
        }

        // PlayerClass playerClass = new PlayerClass();
        //{
        // playerClass.playerName = "default";
        // playerClass.playerID = "default";
        // playerClass.playTimesCount = 1;
        // playerClass.multiplier = 1;
        // playerClass.totalScore = 0;
        // playerClass.timesQuessed = 0;
        // playerClass.timesSkipped = 0;
        // playerClass.totalTries = 0;
        // playerClass.playerMaxMultiplier = 5;

        // playerClass.current_Hearts = 3;
        // playerClass.current_Skips = 1;

        //playerClass.UID = ;//GenerateUUID.UUID();
        //playerClass.lastlogin = DateTime.UtcNow.ToString();
        //string playerJson = JsonUtility.ToJson(playerClass);
        //Components.c.dadabaseManager.UploadPlayerJson(playerJson);
        Debug.Log("DONE ------");
        //currentPlayer = playerClass;
    }

}
