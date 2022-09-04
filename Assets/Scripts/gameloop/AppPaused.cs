using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AppPaused : MonoBehaviour
{
    bool isPaused = false;

//     void OnGUI()
//     {
// //        if (isPaused)
// //            GUI.Label(new Rect(100, 100, 50, 30), "Game paused");
//     }

    private bool hasPausedYet = false;
    void OnApplicationFocus(bool hasFocus)
    {
        isPaused = !hasFocus;
        if(hasPausedYet)
        {
            focusTime = DateTime.Now;
            CompareTimes();
        }
        Debug.Log("GAME CAME BACK FROM BACKGROUND!!!!!!!!!");
        //compare timestamp if any
    }

    private void CompareTimes()
    {
        int betweenSeconds = Convert.ToInt32((focusTime - pauseTime).TotalSeconds);
        Debug.Log("Total seconds between pause and foreground + " + betweenSeconds);
    }

    private DateTime pauseTime;
    private DateTime focusTime;

    void OnApplicationPause(bool pauseStatus)
    {
        isPaused = pauseStatus;
        // save timeStamp for generation 
        Debug.Log("GAME WENT ON BACKGROUND !!!!!!!!!!!!!");
        pauseTime = DateTime.Now;
        hasPausedYet = true;
    }
}
