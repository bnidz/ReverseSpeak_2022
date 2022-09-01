using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIMan : MonoBehaviour
{
    public GameObject skipButton;
    public GameObject gameButton;

    public Text lifesIndicator;
    public Text skipsIndicator;

    public Text skipsTimer;
    public Text lifesTimer;

    public void ActivateSkipButton()
    {
        skipButton.GetComponent<Button>().interactable = true;
    }
    public void DeactivateSkipButton()
    {
        skipButton.GetComponent<Button>().interactable = false;
    }
    public void ActivateGameButton()
    {
        gameButton.GetComponent<Button>().interactable = true;
    }
    public void DeactivateGameButton()
    {
        gameButton.GetComponent<Button>().interactable = false;
    }
    //update game UI to match configs

    public void UpdateUIToConfigs()
    {
        UpdateLifesIndicator();
        UpdateSkipsIndicator();
    }

    //update skips

    public Sprite noHearts, yesHearts;
    public void UpdateLifesIndicator()
    {

        if(Components.c.settings.currentPlayer.current_Hearts == 0)
        {
            lifesIndicator.text = "";
            lifesIndicator.gameObject.GetComponentInParent<Image>().sprite = noHearts;
        }
        else
        {
            lifesIndicator.text = Components.c.settings.currentPlayer.current_Hearts.ToString() + "x";
            lifesIndicator.gameObject.GetComponentInParent<Image>().sprite = yesHearts;

        }
        if( Components.c.settings.currentPlayer.current_Hearts == 1)
        {
            lifesIndicator.text = "";
            lifesIndicator.gameObject.GetComponentInParent<Image>().sprite = yesHearts;
        }

    }

    //update lifes
    public void UpdateSkipsIndicator()
    {


        if(Components.c.settings.currentPlayer.current_Skips == 0 || Components.c.settings.currentPlayer.current_Skips == 1 )
        {
            skipsIndicator.text = "";
        }else
        {
            skipsIndicator.text = Components.c.settings.currentPlayer.current_Skips.ToString() + "x";

        }


    }
    public GameObject leaderboards;
    public void ShowLeaderboards()
    {
        if(leaderboards.activeInHierarchy)
        {
            leaderboards.SetActive(false);
            return;
        }
        if(!leaderboards.activeInHierarchy)
        {
            leaderboards.SetActive(true);
        }

    }


}
