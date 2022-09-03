using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
public class DadabaseManager : MonoBehaviour
{

    DatabaseReference dbRef;
    public void Init()
    {
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void Update_LB_UserEntry(string pID, string p_DisplayName, int totalScore)
    {
        LB_entry _updateVal = new LB_entry(p_DisplayName, totalScore);
        string json =  JsonUtility.ToJson(_updateVal);

        dbRef.Child("players").Child(pID).SetRawJsonValueAsync(json);
    }
}

public class LB_entry
{
    public string p_DisplayName;
    public int p_score;

    public LB_entry(string p_DisplayName, int p_score)
    {
        this.p_DisplayName = p_DisplayName;
        this.p_score = p_score;
    }
}