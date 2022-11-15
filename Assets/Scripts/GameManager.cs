using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apple.GameKit;
using System.Threading.Tasks;
using System;
using UnityEngine.SocialPlatforms;
using System.Threading;
using Firebase.Auth;
using TMPro;
using UnityEngine.UI;

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
  public GameObject nameChangeDG;
  public GameObject localeChangeDG;
  public TextMeshProUGUI splash_name;
  public TextMeshProUGUI splash_streak;
  public TextMeshProUGUI splash_tasks;
  public TextMeshProUGUI splash_rank;
  public TextMeshProUGUI splash_score;
  public TextMeshProUGUI ui_name;
  public TextMeshProUGUI ui_streak;
  public TextMeshProUGUI ui_tasks;
  public TextMeshProUGUI ui_rank;
  public TextMeshProUGUI ui_score;

  public bool startSplashInfo = false;

  void  Update()
  {
    if(startSplashInfo)
    {
      splash_name.text     = ui_name.text;
      splash_streak.text   = ui_streak.text;
      splash_tasks.text    = ui_tasks.text;
      splash_rank.text     = ui_rank.text;
      splash_score.text    = ui_score.text;
    }
  }
    public IEnumerator waitTilAuth()
    {

      while (!isDone) yield return null; // new WaitForSeconds(5f);
      Debug.Log("AUTH DONE!!!");
      Components.c.dadabaseManager.isDone = false;
      Components.c.fireStore_Manager.GetData_Player(gc_id);
      while (!Components.c.fireStore_Manager.isDone) yield return null;

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

          //int daysInMonth = DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month);
          // here daily streak stuff --- 
          
      while (!Components.c.settings.updateBetweenPlays) yield return null;
          //calculate how many days left in month
          Components.c.settings.CheckStreak();
          Components.c.gameUIMan.DailyQuestHolder.transform.parent = Components.c.gameUIMan.DailyQuest_splash_parent.transform;

          //Components.c.gameUIMan.UpdateSplashScreenDailyStreak(Components.c.settings.thisPlayer.dailyTaskStreak);
          Components.c.gameUIMan.Update_UI_DailyStreak();
          //uiman --- set splashscreen values
          int value;
          Components.c.settings.loc_sel_inv.TryGetValue(Components.c.settings.thisPlayer.playerLocale, out value);
          locale_selection = value;
          Components.c.settings.ChangeLocale(locale_selection);
          Components.c.settings.ExecuteLocaleChange();

          Components.c.settings.StartGameSplashScreenButton.gameObject.SetActive(true);// = true;
          startSplashInfo = true;
          Components.c.gameUIMan.UpdateRankText();

          yield break;
        }

        locale = "en-US";
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

          /// first time info spawn

          ///          
          //FETCH DEFAULT FROM DB
          Debug.Log("RETRIEVED PLAYERCLASS FROM DB");
          Components.c.fireStore_Manager.GetData_Default_Player();
          Debug.Log("get data def done");
          while (!Components.c.fireStore_Manager.isDoneDefPlayer) yield return null;
          yield return new WaitForSeconds(.2f);
          Debug.Log("tryna make def with gc id");
          Components.c.settings.MakeNewFromDBDefaultWith_GC_ID(gc_id, gc_name, locale);
          Debug.Log("done new gcid player");
          Debug.Log("tryna upload");
          Components.c.fireStore_Manager.Save_Player_to_DB(Components.c.settings.thisPlayer);
          Debug.Log("done upload");
          Debug.Log("WROTE NEW PLAYER JSON");
          StartCoroutine(Components.c.settings.LoadDefaultConfigs());
          // startSplashInfo = true;
          Components.c.settings.StartGameSplashScreenButton.gameObject.SetActive(true);// = true;

          //while (Components.c.fireStore_Manager.isDoneConfigs == false) yield return null;

          localeChangeDG.SetActive(true);
          nameChangeDG.SetActive(true);
         // Components.c.settings.CheckStreak();

          //Components.c.gameUIMan.UpdateSplashScreenDailyStreak(Components.c.settings.thisPlayer.dailyTaskStreak);


          //Components.c.settings.ChangeLocale(locale_selection);
          //Components.c.gameUIMan.UpdateScoreTo_UI();

        }
        // SPAWN PLAYER NAME CHANGE FOR THE FIRST TIME ---
    }
    public GameObject DG_dailyTaskInfo;
    public GameObject DG_howToPlay;
}