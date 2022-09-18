using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerClass
{
    public string   playerName;
    public string   playerID;
    public int      skillLevel;
    public int      playTimesCount;
    public string   playerLocale;
    public int      timesSkipped;
    public int      timesQuessed;
    public int      totalTries;
    public int      totalScore;

    //locale score
    public int      enUS_score;
    public int      fiFI_score;
    public int      frFR_score;
    public int      deDE_score;

    public float    avgScore;
    public int      current_Hearts;
    public int      current_Skips;
    public string   lastlogin;
    public byte[]   UID;
    public int      multiplier;
    //maybe this--- 
    public int playerMaxMultiplier;

    public void UpdateValuesFromAnotherPlayerClass(PlayerClass u)
    {
        playerName = u.playerName;
        playerID = u.playerID;
        skillLevel = u.skillLevel;
        playTimesCount = u.playTimesCount;
        playerLocale = u.playerLocale;
        timesSkipped = u.timesSkipped;
        timesQuessed = u.timesQuessed;
        totalTries = u.totalTries;
        totalScore = u.totalScore;
        avgScore = u.avgScore;
        current_Hearts = u.current_Hearts;
        current_Skips = u.current_Skips;
        lastlogin = u.lastlogin;
        UID = u.UID;
        multiplier = u.multiplier;
        playerMaxMultiplier = u.playerMaxMultiplier;

        enUS_score = u.enUS_score;
        fiFI_score = u.fiFI_score;
        frFR_score = u.frFR_score;
        deDE_score = u.deDE_score;
    }
}