using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class AppPaused : MonoBehaviour
{
    bool isPaused = false;

//     void OnGUI()
//     {
// //        if (isPaused)
// //            GUI.Label(new Rect(100, 100, 50, 30), "Game paused");
//     }

    public bool isActive = false;
    void OnApplicationFocus(bool hasFocus)
    {
        //isPaused = !hasFocus;
        if(isActive)
        {
            focusTime = DateTime.UtcNow;
            CompareTimes();
            Debug.Log(" TO COMPARE - --- --- GAME CAME BACK FROM BACKGROUND!!!!!!!!!");
        }
        //compare timestamp if any
    }

    private void CompareTimes()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/PlayerFiles/" + "PlayerJson.json"))
        {
            Components.c.settings.currentPlayer.lastlogin = pauseTime.ToString();
            int betweenSeconds = Convert.ToInt32((focusTime - DateTime.Parse(Components.c.settings.currentPlayer.lastlogin)).TotalSeconds);
            Debug.Log("Total seconds between pause and foreground + " + betweenSeconds);
            //update hearts/skips if needed
            Components.c.settings.UpdateFrom_BetweenPlays(betweenSeconds);
        }
    }

    private DateTime pauseTime;
    private DateTime focusTime;

    void OnApplicationPause(bool pauseStatus)
    {

        if(isActive)
        {
            //isPaused = pauseStatus;
            // save timeStamp for generation 
            Debug.Log("GAME WENT ON BACKGROUND !!!!!!!!!!!!!");
            pauseTime = DateTime.UtcNow;
            //Components.c.settings.currentPlayer.lastlogin = DateTime.UtcNow.ToString();
        }
    }
}
