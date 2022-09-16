using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WordClass
{
    public string word;
    public int times_tried;
    public int times_skipped;
    public int times_right;
    public float total_score;
    public float avg_score;
    public int tier;
    public int set;

    public void UpdateWithPlayValues(WordClass nV)
    {
        times_tried += nV.times_tried;
        times_skipped += nV.times_skipped;
        times_right += nV.times_right;
        total_score += nV.total_score;
    }
    
}