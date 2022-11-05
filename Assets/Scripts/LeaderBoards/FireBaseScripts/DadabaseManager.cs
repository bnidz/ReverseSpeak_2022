using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.UI;
using System;
using System.IO;

public class DadabaseManager : MonoBehaviour
{
    private DatabaseReference dbRef_root;
    private DatabaseReference dbRef_words;
    private DatabaseReference dbRef_players;
    private DatabaseReference dbRef_leaderboards;

    string playerLocale = "eng_";

    string _pID;
    //public bool isInit = false;
    public void Init()
    {
        dbRef_root = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void StartUpdateHandler()
    {
        dbRef_words = FirebaseDatabase.DefaultInstance.RootReference.Child("live_words");
        dbRef_players = FirebaseDatabase.DefaultInstance.RootReference.Child("players");
                
        dbRef_leaderboards = FirebaseDatabase.DefaultInstance.RootReference.Child("leaderboards");
      //  dbRef_leaderboards.Child(Components.c.settings.thisPlayer.playerLocale).OrderByChild("p_score").ValueChanged += HandleValueChanged;

    }
    private bool updateingLB = true;
    public void Update_LB_UserEntry(PlayerClass n)
    {

        // int scoreVal = 0;
        // if(Components.c.settings.thisPlayer.playerLocale == "en-US")
        // {
        //     scoreVal = n.enUS_score;
        // }
        // if(Components.c.settings.thisPlayer.playerLocale == "fi-FI")
        // {
        //     scoreVal = n.fiFI_score;
        // }
        // if(Components.c.settings.thisPlayer.playerLocale == "fr-FR")
        // {
        //     scoreVal = n.frFR_score;
        // }
        // if(Components.c.settings.thisPlayer.playerLocale == "de-DE")
        // {
        //     scoreVal = n.deDE_score;
        // }

        LB_entry _updateVal = new LB_entry(n.playerName, Components.c.settings.localeScore, n.UID);

        // --- 


        string json =  JsonUtility.ToJson(_updateVal);
        dbRef_leaderboards.Child(Components.c.settings.thisPlayer.playerLocale).Child(n.playerID).SetRawJsonValueAsync(json);
        updateingLB = false;


    }
    public bool updateFrom_debug = true;
    public bool waiting_ = false;
    public void Update_WordData(WordClass word)
    {

        
        string json =  JsonUtility.ToJson(word);
        dbRef_words.Child(Components.c.settings.thisPlayer.playerLocale).Child(word.word.ToUpper()).SetRawJsonValueAsync(json);
        Debug.Log("UPDATED WORD DATA WITH GAMEWORD DATA");

        // waiting_ = true;
        // //int ogTotalScore;
        // if(updateFrom_debug)
        // {
        //     word.word = word.word.ToUpper();
        //     string _json =  JsonUtility.ToJson(word);
        //     dbRef_words.Child(Components.c.settings.thisPlayer.playerLocale).Child(word.word.ToUpper()).SetRawJsonValueAsync(_json);
        //     return;
        // }
        // //wait_(Update);
        // dbRef_words.Child(Components.c.settings.thisPlayer.playerLocale).Child(word.word.ToUpper()).GetValueAsync().ContinueWithOnMainThread(task => {
        //     if (task.IsFaulted) {

        //         // Handle the error...Debug.Log();
        //         waiting_ = false;
        //         Debug.Log("error somehow wetching word");

        //     }
        //     else if (task.IsCompleted) 
        //     {
        //         DataSnapshot snapshot = task.Result;
        //         // Do something with snapshot...
        //         WordClass donaWord = JsonUtility.FromJson<WordClass>(snapshot.GetRawJsonValue());
        //         donaWord.UpdateWithPlayValues(word);
        //         string json =  JsonUtility.ToJson(donaWord);
        //         dbRef_words.Child(Components.c.settings.thisPlayer.playerLocale).Child(word.word.ToUpper()).SetRawJsonValueAsync(json);
        //         Debug.Log("UPDATED WORD DATA WITH GAMEWORD DATA");
        //         waiting_ = false;
        //     }
        // });  
    }

    public void UpdateALLwords()
    {
        updateFrom_debug = true;
        for (int i = 0; i < Components.c.settings.gameWords.Count; i++)
        {
            Update_WordData(Components.c.settings.gameWords[i]);
            Debug.Log(Components.c.settings.gameWords[i].word + "ADDED TO DB" );
            StartCoroutine(wait_(0.02f));
        } 
        //dbRef_root.Child("eng_words_3000").RemoveValueAsync();
        updateFrom_debug = false;     
    }
    public IEnumerator wait_(float wait)
    {
        yield return new WaitForSeconds(wait);
    }
    public Text db_top1_text;
    private int idx;
    string top10json;
    void HandleValueChanged(object sender, ValueChangedEventArgs args) {
        if (args.DatabaseError != null) {
        
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        idx = 0;
        Debug.Log(args.Snapshot.ChildrenCount +  "TJE ARGS CHILD COUNT IS THIS VALUE" );
        foreach (DataSnapshot leader in args.Snapshot.Children) {

            idx++;
            if(leader.Child("UID").GetRawJsonValue() == getUIDraw())
            { 
                int rank = (Convert.ToInt32(args.Snapshot.ChildrenCount) - (idx-1));
                db_top1_text.text = rank.ToString() + "# rank ";

            }
            if((Convert.ToInt32(args.Snapshot.ChildrenCount) - (idx)) < 10)
            {
                top10json += leader.GetRawJsonValue() + "ITEM NUMBER : " + idx;

                //FIRST IS NUMBER 10
                //THEN DOWN
                string lb_score = leader.Child("p_score").Value.ToString();
                string lb_name = leader.Child("p_DisplayName").Value.ToString();
                int lb_rank = Convert.ToInt32(args.Snapshot.ChildrenCount) - (idx-1);
                //Components.c.displayHighScores.AddToLB(lb_rank, lb_name, lb_score);
            }
        }

        idx = 0;
        top10json = "";
    }
private string p_UID;
    public string getUIDraw()
    {

        dbRef_root.Child("players").Child(Components.c.settings.thisPlayer.playerID)
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

    // public void ReadDB()
    // {
    //     FirebaseDatabase.DefaultInstance.GetReference("players")
    //         .GetValueAsync().ContinueWithOnMainThread(task => {
    //             if (task.IsFaulted) {
    //                 // Handle the error...
    //                 }
    //             else if (task.IsCompleted) {
    //                 DataSnapshot snapshot = task.Result;
    //                 // Do something with snapshot...
    //                 Debug.Log(snapshot.GetRawJsonValue().ToString());
    //             }
    //   });
    //     FirebaseDatabase.DefaultInstance.GetReference("players").EqualTo("")
    //         .GetValueAsync().ContinueWithOnMainThread(task => {
    //             if (task.IsFaulted) {
    //                 // Handle the error...
    //                 }
    //             else if (task.IsCompleted) {
    //                 DataSnapshot snapshot = task.Result;
    //                 // Do something with snapshot...
    //                 Debug.Log(snapshot.GetRawJsonValue().ToString());
    //             }
    //   });
    // }

    private string wordJson;
    public string getWORDraw(string word)
    {
        dbRef_root.Child(Components.c.settings.thisPlayer.playerLocale + "_words_live").Child(word.ToLower())
        .GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsFaulted) {
                    // Handle the error...Debug.Log
                    Debug.Log("ERROR GETTING WORD DATA WITH SPESIFIC KEY");
                    }
                else if (task.IsCompleted) {
                    DataSnapshot snapshot = task.Result;
                    wordJson = snapshot.GetRawJsonValue();
                    // Do something with snapshot...
                   // p_UID = snapshot.Child("UID").GetRawJsonValue().ToString();
                    //Debug.Log("............... homo snapshot value " +snapshot.Child("UID").GetRawJsonValue().ToString());
                }
            });
        return wordJson;
    }

    public void populateLB()
    {
        StartCoroutine(PopulateLeaderBoards(.02f));   
    }

    const string glyphs= "abcdefghijklmnopqrstuvwxyz0123456789"; //add the characters you want
    private string n_name;
    private PlayerClass _y;
    public IEnumerator PopulateLeaderBoards(float interval)
    {   
       // _y = new PlayerClass();
        for (int i = 0; i < 25; i++)
        {
            n_name = "";
            int scroe = UnityEngine.Random.Range(Components.c.settings.thisPlayer.totalScore, Components.c.settings.thisPlayer.totalScore + 20000); 
            int charAmount = UnityEngine.Random.Range(6, 12); //set those to the minimum and maximum length of your string
            for(int x=0; x <charAmount; x++)
            {
                n_name += glyphs[UnityEngine.Random.Range(0, glyphs.Length)];
                updateingLB = false;
         //   _y.playerName = n_name;
         //   _y.totalScore = scroe;
         //   _y.UID = GenerateUUID.UUID();
            }
            byte[] iudee = GenerateUUID.UUID();
            //Update_LB_UserEntry(_y);
            LB_entry _updateVal = new LB_entry(n_name, scroe, iudee);
            string json =  JsonUtility.ToJson(_updateVal);
            dbRef_root.Child(Components.c.settings.thisPlayer.playerLocale + "_leaderboards").Child(n_name).SetRawJsonValueAsync(json);
            updateingLB = false;
            yield return new WaitForSeconds(interval);
            //while (updateingLB) yield return null;
        }
    }

    public void DoneDefConfigs()
    {
        dbRef_root.Child("configs").Child("default")
        .GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted) {
                // Handle the error...Debug.Log
                Debug.Log("ERROR GETTING WORD DATA WITH SPESIFIC KEY");
                isDone = true;
                }
            else if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;
                // Do something with snapshot...
               // Components.c.settings.thisConfigs = JsonUtility.FromJson<GameConfigs>(snapshot.GetRawJsonValue().ToString());
                isDone = true;//Debug.Log("............... homo snapshot value " +snapshot.Child("UID").GetRawJsonValue().ToString());
            }

          //  Components.c.fireStore_Manager.Upload_Configs(Components.c.fireStore_Manager.GameConfigsToConfigs(Components.c.settings.thisConfigs));

      });
    }
    public bool uploadDone = false;
    public void UploadNewPlayerTo_DB(Player n)
    {
        Components.c.fireStore_Manager.Save_Player_to_DB(n);
        uploadDone = true;
    }
    public void UploadDefaultConfig(GameConfigs n)
    {
        string njson = JsonUtility.ToJson(n);
        dbRef_root.Child("configs").Child("default").SetRawJsonValueAsync(njson);
        Debug.Log("UPLOADED NEW PLAYER JSON TO SERVER");
        isDone = true;
    }

    public void UploadPlayerJson(string json)
    {
        PlayerClass n = new PlayerClass();
        dbRef_root.Child("default_player").SetRawJsonValueAsync(json);
    }
    // TEST WITH CLEAN BUILD IF CAN FETCH THE PLAYER FROM DB
    public bool isDB_save = false;

    public bool isDone = false;
    public void CheckIfPlayerClassExists(string gc_id)
    {
        Components.c.fireStore_Manager.GetData_Player(gc_id);
        // if(p.playerID != "default")
        // {
        //     Components.c.settings.thisPlayer = p;
        //     isDB_save = true;
        // }
        // elsew
        // {
        //     isDB_save = false;
        // }
        // isDone = true;
    }

    private PlayerClass _default;
    public bool donaDone = false;
    public void  FetchDefaultPlayerClass()
    {

        //Components.c.settings.thisPlayer = Components.c.fireStore_Manager.GetData_Default_Player();
        donaDone = true;
    }

    public void Passed_WordData(WordClass word)
    {
        word.word = word.word.ToUpper();
        string _json =  JsonUtility.ToJson(word);
        dbRef_root.Child("fin_words_passed").Child(word.word.ToUpper()).SetRawJsonValueAsync(_json);
        //dbRef_root.Child("checkIndex").Child("value").SetValueAsync(Components.c.gameloop.checkIndex);
        return;
    }

    public List<WordClass> temp;
    public bool fetchingWords = false;
    public void fin_get_all_words_from_DB()
    {
        temp = new List<WordClass>();
        dbRef_root.Child("fin_words_passed").
        GetValueAsync().ContinueWith(task =>
        {
            int totalChildren = (int)task.Result.ChildrenCount;
            foreach (DataSnapshot word in task.Result.Children) {

            WordClass w = JsonUtility.FromJson<WordClass>(word.GetRawJsonValue());
            temp.Add(w);
            Debug.Log("word data added ---- " + w.ToString());
        }
        Components.c.settings.gameWords = temp;
        fetchingWords = true;
        }); 
    }

    public void en_get_all_words_from_DB()
    {
            temp = new List<WordClass>();
            dbRef_root.Child("eng_words_passed").
            GetValueAsync().ContinueWith(task =>
            {
                int totalChildren = (int)task.Result.ChildrenCount;
                foreach (DataSnapshot word in task.Result.Children) {
                    WordClass w = JsonUtility.FromJson<WordClass>(word.GetRawJsonValue());
                    temp.Add(w);
                    Debug.Log("word data added ---- " + w.ToString());
            }
            Components.c.settings.gameWords = temp;
            fetchingWords = true;
            }); 
    }

    public void fr_get_all_words_from_DB()
    {
        temp = new List<WordClass>();
        dbRef_root.Child("fr_words_passed").
        GetValueAsync().ContinueWith(task =>
        {
            int totalChildren = (int)task.Result.ChildrenCount;
            foreach (DataSnapshot word in task.Result.Children) {

                WordClass w = JsonUtility.FromJson<WordClass>(word.GetRawJsonValue());
                temp.Add(w);
                                    Debug.Log("word data added ---- " + w.ToString());
        }
        Components.c.settings.gameWords = temp;
        fetchingWords = true;
        }); 
    }
        public void de_get_all_words_from_DB()
    {
            temp = new List<WordClass>();
            dbRef_root.Child("de_words_passed").
            GetValueAsync().ContinueWith(task =>
            {
                int totalChildren = (int)task.Result.ChildrenCount;
                foreach (DataSnapshot word in task.Result.Children) {
                
                    WordClass w = JsonUtility.FromJson<WordClass>(word.GetRawJsonValue());
                    temp.Add(w);
                    Debug.Log("word data added ---- " + w.ToString());

            }
            Components.c.settings.gameWords = temp;
            fetchingWords = true;
            });
    }
    public void Rejected_WordData(WordClass word)
    {
        word.word = word.word.ToUpper();
        string _json =  JsonUtility.ToJson(word);
        dbRef_root.Child("spes_words_rejected").Child(word.word.ToUpper()).SetRawJsonValueAsync(_json);
        return;
    }
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