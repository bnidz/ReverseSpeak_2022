using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.UI;

public class DadabaseManager : MonoBehaviour
{
    private DatabaseReference dbRef;
    string _pID;
    public void Init()
    {
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;

        FirebaseDatabase.DefaultInstance
            .GetReference("players").OrderByChild("p_score").LimitToLast(1)
            .ValueChanged += HandleValueChanged;
    }

    public void Update_LB_UserEntry(string pID, string p_DisplayName, int totalScore)
    {
        LB_entry _updateVal = new LB_entry(p_DisplayName, totalScore);
        string json =  JsonUtility.ToJson(_updateVal);

        dbRef.Child("players").Child(pID).SetRawJsonValueAsync(json);

        ReadDB();
    }

    public Text db_top1_text;
    void HandleValueChanged(object sender, ValueChangedEventArgs args) {
        if (args.DatabaseError != null) {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        //do
        foreach (DataSnapshot leader in args.Snapshot.Children) {
        db_top1_text.text = ("Received value for leader: "+ leader.Child("p_score").Value);
        }
    }

    public void ReadDB()
    {
        FirebaseDatabase.DefaultInstance.GetReference("players")
            .GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsFaulted) {
                    // Handle the error...
                    }
                else if (task.IsCompleted) {
                    DataSnapshot snapshot = task.Result;
                    // Do something with snapshot...
                    Debug.Log(snapshot.ToString());
                }
      });
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