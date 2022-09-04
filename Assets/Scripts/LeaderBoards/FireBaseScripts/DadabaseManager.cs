using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.UI;
using System;

public class DadabaseManager : MonoBehaviour
{
    private DatabaseReference dbRef;
    string _pID;
    public void Init()
    {
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;

        FirebaseDatabase.DefaultInstance
            .GetReference("players").OrderByChild("p_score")
            .ValueChanged += HandleValueChanged;
    }

    public void Update_LB_UserEntry(string pID, string p_DisplayName, int totalScore, byte[] UID)
    {
        LB_entry _updateVal = new LB_entry(p_DisplayName, totalScore, UID);
        string json =  JsonUtility.ToJson(_updateVal);

        dbRef.Child("players").Child(pID).SetRawJsonValueAsync(json);

    }

    public void Update_WordData(WordClass word)
    {
        string json =  JsonUtility.ToJson(word);
        dbRef.Child("eng_words").Child(word.word).SetRawJsonValueAsync(json);
    }

    public void UpdateALLwords()
    {
        for (int i = 0; i < Components.c.settings.gameWords.Count; i++)
        {
            Update_WordData(Components.c.settings.gameWords[i]);
            Debug.Log(Components.c.settings.gameWords[i].word + "ADDED TO DB" );
            StartCoroutine(wait_());
        }       
    }
    public IEnumerator wait_()
    {
        yield return new WaitForSeconds(.02f);

    }
    public Text db_top1_text;
    private int idx;
    void HandleValueChanged(object sender, ValueChangedEventArgs args) {


        if (args.DatabaseError != null) {
        
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        idx = 0;

        Debug.Log(args.Snapshot.ChildrenCount +  "TJE ARGS CHILD COUNT IS THIS VALUE" );
        foreach (DataSnapshot leader in args.Snapshot.Children) {
            //rankList.Add(leader.Child("UID").Value.ToString());
            idx++;
            if(leader.Child("UID").GetRawJsonValue() == getUIDraw())
            {
                int rank = (Convert.ToInt32(args.Snapshot.ChildrenCount) - (idx-1));
                db_top1_text.text = rank.ToString() + "# rank ";

            }

        }
        idx = 0;
    }
private string p_UID;
    public string getUIDraw()
    {

        dbRef.Child("players").Child(Components.c.settings.currentPlayer.playerID)
        .GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsFaulted) {
                    // Handle the error...
                    }
                else if (task.IsCompleted) {
                    DataSnapshot snapshot = task.Result;
                    // Do something with snapshot...
                    p_UID = snapshot.Child("UID").GetRawJsonValue().ToString();
                    //Debug.Log("............... homo snapshot value " +snapshot.Child("UID").GetRawJsonValue().ToString());
                }
      });
        return p_UID;
    }

    public class thisUID{
        public byte[] bytes;
    }
    private List<string> rankList;

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
                    Debug.Log(snapshot.GetRawJsonValue().ToString());
                }
      });
        FirebaseDatabase.DefaultInstance.GetReference("players").EqualTo("")
            .GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsFaulted) {
                    // Handle the error...
                    }
                else if (task.IsCompleted) {
                    DataSnapshot snapshot = task.Result;
                    // Do something with snapshot...
                    Debug.Log(snapshot.GetRawJsonValue().ToString());
                }
      });
    }
    public void populateLB()
    {
        for (int i = 0; i < 25; i++)
        {
            StartCoroutine(PopulateLeaderBoards(.02f));
            
        }
        
    }
    const string glyphs= "abcdefghijklmnopqrstuvwxyz0123456789"; //add the characters you want
    private string n_name;
    public IEnumerator PopulateLeaderBoards(float interval)
    {

            int scroe = UnityEngine.Random.Range(20000, 40000); 
            
            int charAmount = UnityEngine.Random.Range(6, 12); //set those to the minimum and maximum length of your string
            for(int y=0; y <charAmount; y++)
            {
                n_name += glyphs[UnityEngine.Random.Range(0, glyphs.Length)];
            }
            Update_LB_UserEntry(n_name, n_name, scroe, GenerateUUID.UUID());
            n_name = "";
            //yield return new WaitForSeconds(.02f);
        yield return new WaitForSeconds(interval);
        
    }
    // private void AddScoreToLeaders(string email, 
    //                            long score,
    //                            DatabaseReference leaderBoardRef) {

    //     leaderBoardRef.RunTransaction(mutableData => {
    //     List<object> leaders = mutableData.Value as List<object>;

    //     if (leaders == null) {
    //         leaders = new List<object>();
    //     } else if (mutableData.ChildrenCount >= MaxScores) {
    //         long minScore = long.MaxValue;
    //         object minVal = null;
    //         foreach (var child in leaders) {
    //         if (!(child is Dictionary<string, object>)) continue;
    //         long childScore = (long)
    //                     ((Dictionary<string, object>)child)["score"];
    //         if (childScore < minScore) {
    //             minScore = childScore;
    //             minVal = child;
    //         }
    //         }
    //         if (minScore > score) {
    //         // The new score is lower than the existing 5 scores, abort.
    //         return TransactionResult.Abort();
    //         }

    //         // Remove the lowest score.
    //         leaders.Remove(minVal);
    //     }
    //     // Add the new high score.
    //     Dictionary<string, object> newScoreMap =
    //                     new Dictionary<string, object>();
    //     newScoreMap["score"] = score;
    //     newScoreMap["email"] = email;
    //     leaders.Add(newScoreMap);
    //     mutableData.Value = leaders;
    //     return TransactionResult.Success(mutableData);
    //     });
    // }
}

public class LB_entry
{
    public string p_DisplayName;
    public int p_score;
    public byte[] UID;
    public LB_entry(string p_DisplayName, int p_score, byte[] UID)
    {
        this.UID = UID;
        this.p_DisplayName = p_DisplayName;
        this.p_score = p_score;
    }
}

public class LB_entryList
{
    public List<LB_entry> lb_list;
}