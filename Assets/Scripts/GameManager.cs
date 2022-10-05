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
    private string gc_id;
    public bool isDone = false;
    private string gc_name;
    public Player thisPlayer;

    public void StartAuth()
    {
#if UNITY_EDITOR
//pisssdsd
#endif

    Social.localUser.Authenticate(success => {
    //--- ...  
        if (success)
        {
            Debug.Log("Authentication successful");
            string userInfo = "Username: " + Social.localUser.userName +
                "\nUser ID: " + Social.localUser.id +
                "\nIsUnderage: " + Social.localUser.underage;

            gc_id =  Social.localUser.id;
            gc_name = Social.localUser.userName;
            Debug.Log(userInfo);
            SignIn();
            isDone = true;
            Debug.Log("ABOUT TO SIGN IN :DDDDDD");
        }
        else
        {
            Debug.Log("Authentication failed");
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
            Debug.Log("error");
          var credential = credentialTask.Result;
          var userFuture = FirebaseAuth.DefaultInstance.SignInWithCredentialAsync(credential);
          return userFuture;
        }).Unwrap().ContinueWith(userTask => {
          if(userTask.IsFaulted)
            throw userTask.Exception;
          if(!userTask.IsCompleted)
            Debug.Log("error");
          return userTask.Result;
        });
        return retUserFuture;
        }
        else
        {
          TaskCompletionSource<FirebaseUser> taskCompletionSource =
          new TaskCompletionSource<FirebaseUser>();
          return taskCompletionSource.Task;
        }
    }
    public string locale = "en-US";
    int locale_selection = 0;

  public bool player_DB_save;

    public IEnumerator waitTilAuth()
    {

      while (!isDone) yield return null; // new WaitForSeconds(5f);
      Debug.Log("AUTH DONE!!!");
      Components.c.dadabaseManager.isDone = false;
      //Components.c.dadabaseManager.CheckIfPlayerClassExists(gc_id);
      Components.c.fireStore_Manager.GetData_Player(gc_id);
      while (!Components.c.fireStore_Manager.isDone) yield return null;


      // if(Components.c.settings.thisPlayer.playerName != null)
      // {
      //   Debug.Log("Player found");
      //   //Components.c.settings.thisPlayer = Components.c.fireStore_Manager.fs_Player;
      //   Components.c.dadabaseManager.isDB_save = true;

      // }else
      // Components.c.dadabaseManager.isDB_save = false;


      Debug.Log(" CHECKED FOR EXISTING DB SAVE!!!");

        if(player_DB_save)
        {
          Debug.Log("PLAYER LOADED FROM DADABASE");
          Components.c.settings.LoadSavedPlayerSettings();
          Components.c.dadabaseManager.isDone = false;

          if(Components.c.settings.thisPlayer.playerLocale == "en-US")
          {
            locale = "en-US";
            locale_selection = 0;
          }
          if(Components.c.settings.thisPlayer.playerLocale == "fi-FI")
          {
            locale = "fi-FI";
            locale_selection = 1;
          }
          if(Components.c.settings.thisPlayer.playerLocale == "fr-FR")
          {
            locale = "fr-FR";
            locale_selection = 2;
          }
          if(Components.c.settings.thisPlayer.playerLocale == "de-DE")
          {
            locale = "de-DE";
            locale_selection = 3;
          }
          Components.c.settings.ChangeLocale(locale_selection);
          //Components.c.runorder._continue();
          yield break;

        }

        if(!player_DB_save)
        {
          Debug.Log("PLAYER NOT IN DADABASE");
          if(Application.systemLanguage == SystemLanguage.English)
          {
            locale = "en-US";
            locale_selection = 0;
          }
          if(Application.systemLanguage == SystemLanguage.Finnish)
          {
            locale = "fi-FI";
            locale_selection = 1;
          }
          if(Application.systemLanguage == SystemLanguage.French)
          {
            locale = "fr-FR";
            locale_selection = 2;
          }
          if(Application.systemLanguage == SystemLanguage.German)
          {
            locale = "de-DE";
            locale_selection = 3;
          }

          //FETCH DEFAULT FROM DB
          Debug.Log("RETRIEVED PLAYERCLASS FROM DB");
          Components.c.fireStore_Manager.GetData_Default_Player();
          while (!Components.c.fireStore_Manager.isDone) yield return null;
          Components.c.settings.MakeNewFromDBDefaultWith_GC_ID(gc_id, gc_name, locale);
          //while (!Components.c.settings.isDone) yield return null;
          //Components.c.dadabaseManager.UploadNewPlayerTo_DB(Components.c.settings.thisPlayer);
          //while (!Components.c.dadabaseManager.uploadDone) yield return null;
          Components.c.fireStore_Manager.Save_Player_to_DB(Components.c.settings.thisPlayer);
          Debug.Log("WROTE NEW PLAYER JSON");
          Components.c.settings.LoadDefaultConfigs();
          Components.c.settings.ChangeLocale(locale_selection);

        }
        // SPAWN PLAYER NAME CHANGE FOR THE FIRST TIME ---
    }
}