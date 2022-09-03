using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerClass
{
    public string playerName;
    public string playerID;
    public int skillLevel;

    public int playTimesCount;
    public string playerLocale;

    public int timesSkipped;
    public int timesQuessed;
    public int totalTries;

    public int totalScore;
    public float avgScore;

    public int current_Hearts;
    public int current_Skips;
    public string lastlogin;
}

