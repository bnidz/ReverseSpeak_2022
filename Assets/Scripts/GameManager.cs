using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apple.GameKit;
using System.Threading.Tasks;
using System;
using UnityEngine.SocialPlatforms;
using System.Threading;
using Firebase.Auth;

public class GameManager : MonoBehaviour
{
    //public GKLocalPlayer _localPlayer;
    // public void Init()
    // {
    //     //StartAuth();
    //     //StartCoroutine(waitTilAuth());
    //     isDone = false;
    //     StartAuth();


    // }

    // Start is called before the first frame update
    private string gc_id;
    public bool isDone = false;
    private string gc_name;
    public void StartAuth()
    {
#if UNITY_EDITOR
//pisssdsd
#endif
    //yield return new WaitForSeconds(3f);

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

            //Social.localUser
            //FirebaseUser user;
           // user.LinkWithCredentialAsync
            //Credential
           // Credential credential;
            
           // Components.c.filereader.MakeNewWordItems();


            //FirebaseAuth.DefaultInstance.

            //FirebaseUser currentUser;
            Debug.Log("ABOUT TO SIGN IN :DDDDDD");
            Debug.Log("ABOUT TO SIGN IN :DDDDDD");
            Debug.Log("ABOUT TO SIGN IN :DDDDDD");
            Debug.Log("ABOUT TO SIGN IN :DDDDDD");
            Debug.Log("ABOUT TO SIGN IN :DDDDDD");
            Debug.Log("ABOUT TO SIGN IN :DDDDDD");
            SignIn();
            Debug.Log("SING IN DONE :DDDD!!!");
            Debug.Log("SING IN DONE :DDDD!!!");
            Debug.Log("SING IN DONE :DDDD!!!");
            Debug.Log("SING IN DONE :DDDD!!!");
            Debug.Log("SING IN DONE :DDDD!!!");
            Debug.Log("SING IN DONE :DDDD!!!");
            Debug.Log("SING IN DONE :DDDD!!!");
        }
        else
        {
            Debug.Log("Authentication failed");
            Debug.Log("Authentication failed");
            Debug.Log("Authentication failed");

            Debug.Log("Authentication failed");
            Debug.Log("IMPLEMET TO CHECK LOCALSAVE WITH UID IF NEEDED");
            Debug.Log("IMPLEMET TO CHECK LOCALSAVE WITH UID IF NEEDED");
            Debug.Log("IMPLEMET TO CHECK LOCALSAVE WITH UID IF NEEDED");
            Debug.Log("IMPLEMET TO CHECK LOCALSAVE WITH UID IF NEEDED");
            Debug.Log("IMPLEMET TO CHECK LOCALSAVE WITH UID IF NEEDED");
            Debug.Log("IMPLEMET TO CHECK LOCALSAVE WITH UID IF NEEDED");
            Debug.Log("IMPLEMET TO CHECK LOCALSAVE WITH UID IF NEEDED");
            Debug.Log("IMPLEMET TO CHECK LOCALSAVE WITH UID IF NEEDED");
            Debug.Log("IMPLEMET TO CHECK LOCALSAVE WITH UID IF NEEDED");
            Debug.Log("IMPLEMET TO CHECK LOCALSAVE WITH UID IF NEEDED");
            isDone = true;
           // return true;
        }
    });

}
    public static Task<FirebaseUser> SignIn() {
      if (Firebase.Auth.GameCenterAuthProvider.IsPlayerAuthenticated()) {
        var credentialFuture = Firebase.Auth.GameCenterAuthProvider.GetCredentialAsync();
        var retUserFuture = credentialFuture.ContinueWith(credentialTask => {
          if(credentialTask.IsFaulted)
            throw credentialTask.Exception;
          if(!credentialTask.IsCompleted)
            // throw new FetchCredentialFailedException(
            //         "Game Center SignIn() failed to fetch credential.");
            Debug.Log("error");
          var credential = credentialTask.Result;
          var userFuture = FirebaseAuth.DefaultInstance.SignInWithCredentialAsync(credential);
          return userFuture;
        }).Unwrap().ContinueWith(userTask => {
          if(userTask.IsFaulted)
            throw userTask.Exception;
          if(!userTask.IsCompleted)
            // throw new SignInFailedException(
            //         "Game Center SignIn() failed to Sign In with Credential.");
            Debug.Log("error");
          
          //SignInState.SetState(SignInState.State.GameCenter);
          return userTask.Result;
        });

        return retUserFuture;
      } else {
        TaskCompletionSource<FirebaseUser> taskCompletionSource =
          new TaskCompletionSource<FirebaseUser>();

          //taskCompletionSource.SetException(
          //new SignInFailedException(
           // "Game Center SignIn() failed - User not authenticated to Game Center."));
          return taskCompletionSource.Task;
      }
    }



    public IEnumerator waitTilAuth()
    {
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
            Components.c.settings.LoadSavedPlayerSettings(gc_name, gc_id);
            Components.c.dadabaseManager.isDone = false;
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
