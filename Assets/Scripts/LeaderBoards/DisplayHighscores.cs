using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHighscores : MonoBehaviour 
{
    public TMPro.TextMeshProUGUI[] rNames;
    public TMPro.TextMeshProUGUI[] rScores;
    public TMPro.TextMeshProUGUI[] rRanks;
    //HighScores myScores;

    public GameObject[] leaderboardListings;
    //public Transform leaderboardHolder;
    //public int maxLBentriesToShow;

    public void Init() //Fetches the Data at the beginning
    {
        for (int i = 0; i < leaderboardListings.Length; i++)
        {
            //GameObject _leaderboardListing = new leaderboardListing();
            //GameObject _listing = new GameObject(Instantiate(leaderboardListing, leaderboardHolder));
            LB_item lb_item = leaderboardListings[i].GetComponent<LB_item>();

            rNames[i] = lb_item.rName;
            rScores[i] = lb_item.rScore;
            rRanks[i] = lb_item.rRank;
        }
        for (int i = 0; i < rNames.Length;i ++)
        {
            rNames[i].text = i + 1 + ". Fetching...";
        }
        //maxLBentriesToShow = 10;

        Debug.Log("DISPLAY HIGHSCORE INIT....... IS ....... EMPTY");

    }

    public void AddToLB(int rank, string name, string score)
    {        
     //   EmptyLB_view();
        rNames[rank -1].text = name; 
        rScores[rank -1].text = score.ToString();
        rRanks[rank -1].text = rank.ToString();
    }

    public void EmptyLB_view()
    {
        for (int i = 0; i < rNames.Length; i++)
        {
            rRanks[i].text = " ";
            rScores[i].text = " ";
            rNames[i].text = " ";
        }
    }
    private int idx;
    public void SetScoresToMenu(PlayerScore[] highscoreList) //Assigns proper name and score for each text value
    {
//        idx = highscoreList.Length;
        for (int i = 0; i < rNames.Length; i++)
        {
            rRanks[i].text = " ";// + rank.ToString();
            rScores[i].text = " ";//highscoreList[i].score.ToString();
            rNames[i].text = " ";//highscoreList[i].username;
        }
        for (int i = 0; i < highscoreList.Length;i ++)
        {
            rNames[i].text = i + 1 + ". ";
            if (highscoreList.Length > i)
            {
                int rank = (i+1);
                rRanks[i].text = "# " + rank.ToString();
                rScores[i].text = highscoreList[i].score.ToString();
                rNames[i].text = highscoreList[i].username;
            }
        }  
    }
}
