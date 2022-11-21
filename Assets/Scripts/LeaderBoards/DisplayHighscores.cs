using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHighscores : MonoBehaviour 
{
    //public GameObject LB_prefab;
    public LB_item lb_item = new LB_item();
    public LB_item lb_item_empty;
    public GameObject lb_itemGO;
    public Transform leaderboardHolder;
    //public int maxLBentriesToShow;
    public void Init() //Fetches the Data at the beginning
    {
        for (int i = 0; i < lbGO_list_100_showing.Count; i++)
        {
            lbITEM_list_Showing[i] = lbGO_list_100_showing[i].GetComponent<LB_item>();

            //lb_item = lb_itemGO.GetComponent<LB_item>();
            lbITEM_list_Showing[i].rRank.text = "#"+(i+1).ToString();
            lbITEM_list_Showing[i].rScore.text = (000).ToString();
            lbITEM_list_Showing[i].rName.text = "???".ToString();

            // //lbITEM_List_y
            // lbE_list_week.Add(new LeaderBoard_entry{
            //     p_DisplayName = "???",
            //     p_score = 000,
            //     rank100 = "#" + i.ToString(),
            // });
            // lbE_list_month.Add(new LeaderBoard_entry{
            //     p_DisplayName = "???",
            //     p_score = 000,
            //     rank100 = "#" + i.ToString(),
            // });
            // lbE_list_year.Add(new LeaderBoard_entry{
            //     p_DisplayName = "???",
            //     p_score = 000,
            //     rank100 = "#" + i.ToString(),
            // });
            // lbE_list_alltime.Add(new LeaderBoard_entry{
            //     p_DisplayName = "???",
            //     p_score = 000,
            //     rank100 = "#" + i.ToString(),
            // });
        }
    }

    // public List<GameObject> lbGO_list_100_week = new List<GameObject>();
    // public List<GameObject> lbGO_list_100_month = new List<GameObject>();
    // public List<GameObject> lbGO_list_100_year = new List<GameObject>();
    // public List<GameObject> lbGO_list_100_alltime = new List<GameObject>();
    public List<GameObject> lbGO_list_100_showing = new List<GameObject>();




    public List<LB_item> lbITEM_list_Showing = new List<LB_item>();
   // public List<GameObject> lbGO_list_month = new List<GameObject>();

    public void AddToLB(int rank, LeaderBoard_entry lbe)
    {

        lb_item = lb_itemGO.GetComponent<LB_item>();
        lb_item.rRank.text = (rank+1).ToString();
        lb_item.rScore.text = lbe.p_score.ToString();
        lb_item.rName.text = lbe.p_DisplayName.ToString();
    }

    public bool isClear = false;
    public void EmptyLB_view()
    {
        // GameObject[] myArray = lbGO_list.ToArray();
        // for (int i = 0; i < myArray.Length; i++)
        // {
        //     Destroy(myArray[i]);
        // }
        // lbGO_list.Clear();
        // Resources.UnloadUnusedAssets();
        // isClear = true;
    }

    // public void Show_weekly()
    // {
    //     for (int i = 0; i < lbITEM_list_Showing.Count; i++)
    //     {
    //         lbITEM_list_Showing[i].rName.text = lbE_list_week[i].p_DisplayName;
    //         lbITEM_list_Showing[i].rScore.text = lbE_list_week[i].p_score.ToString();
    //         //lbITEM_list_Showing[i].rName.text = lbE_list_week[i].p_DisplayName;
    //     }
    // }
    // public void Show_monthly()
    // {
    //     for (int i = 0; i < lbITEM_list_Showing.Count; i++)
    //     {
    //         lbITEM_list_Showing[i].rName.text = lbE_list_month[i].p_DisplayName;
    //         lbITEM_list_Showing[i].rScore.text = lbE_list_month[i].p_score.ToString();
    //         //lbITEM_list_Showing[i].rName.text = lbE_list_week[i].p_DisplayName;
    //     }
    // }
    // public void Show_yearly()
    // {
    //     for (int i = 0; i < lbITEM_list_Showing.Count; i++)
    //     {
    //         lbITEM_list_Showing[i].rName.text = lbE_list_year[i].p_DisplayName;
    //         lbITEM_list_Showing[i].rScore.text = lbE_list_year[i].p_score.ToString();
    //         //lbITEM_list_Showing[i].rName.text = lbE_list_week[i].p_DisplayName;
    //     }
    // }
    // public void Show_alltime()
    // {
    //     for (int i = 0; i < lbE_list_alltime.Count; i++)
    //     {
    //         lbITEM_list_Showing[i].rName.text = lbE_list_alltime[i].p_DisplayName;
    //         lbITEM_list_Showing[i].rScore.text = lbE_list_alltime[i].p_score.ToString();

    //         Debug.Log("lb ebtry  ++ " + lbE_list_alltime[i].p_DisplayName);
    //         //lbITEM_list_Showing[i].rName.text = lbE_list_week[i].p_DisplayName;
    //     }
    // }




    private int idx;
    public void SetScoresToMenu(PlayerScore[] highscoreList) //Assigns proper name and score for each text value
    {
// //        idx = highscoreList.Length;
//         for (int i = 0; i < rNames.Length; i++)
//         {
//             rRanks[i].text = " ";// + rank.ToString();
//             rScores[i].text = " ";//highscoreList[i].score.ToString();
//             rNames[i].text = " ";//highscoreList[i].username;
//         }
//         for (int i = 0; i < highscoreList.Length;i ++)
//         {
//             rNames[i].text = i + 1 + ". ";
//             if (highscoreList.Length > i)
//             {
//                 int rank = (i+1);
//                 rRanks[i].text = "# " + rank.ToString();
//                 rScores[i].text = highscoreList[i].score.ToString();
//                 rNames[i].text = highscoreList[i].username;
//             }
//         }  
    }
}
