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
    public void UpdateLifesIndicator()
    {
        lifesIndicator.text = Components.c.settings.currentConfigs.current_Hearts.ToString();

    }

    //update lifes
    public void UpdateSkipsIndicator()
    {

        skipsIndicator.text = Components.c.settings.currentConfigs.current_Skips.ToString();

    }


}
