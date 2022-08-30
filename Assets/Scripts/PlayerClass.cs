using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerClass
{
            //save login timestamp
            //save player language
            //skill
            //playtimes

    public string playerName;
    public string playerID;
    public int skillLevel;

    public int playTimesCount;
    public string playerLocale;

    public int timesSkipped;
    public int timesQuessed;
    public int totalTries;

    public float totalScore;
    public float avgScore;

}

