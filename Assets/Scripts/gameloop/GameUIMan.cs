using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIMan : MonoBehaviour
{
    public Button skipButton;

    public Text lifesIndicator;
    public Text skipsIndicator;

    public Text skipsTimer;
    public Text lifesTimer;

    public void ActivateSkipButton()
    {
        skipButton.interactable = true;
    }
    public void DeactivateSkipButton()
    {
        skipButton.interactable = false;
    }
    public void ActivateGameButton()
    {
        //gameButton.interactable = true;
    }
    public void DeactivateGameButton()
    {
        //gameButton.interactable = false;
    }

    public void UpdateUIToConfigs()
    {
        UpdateLifesIndicator();
        UpdateSkipsIndicator();
    }

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

    public void UpdateMultiplier_UI(int value)
    {
        string x = "x";
        if(value >1)
        {
            multiplierText.text = value.ToString() + x;
        }
        else
        {
            multiplierText.text = "";
        }
    }
    public TextMeshProUGUI multiplierText;
    public TextMeshProUGUI[] circleTexts;
    //public ntw.CurvedTextMeshPro.TextProOnACircle r1;
    // public ntw.CurvedTextMeshPro.TextProOnACircle  r2;
    //public ntw.CurvedTextMeshPro.TextProOnACircle  b1;
    // public ntw.CurvedTextMeshPro.TextProOnACircle  b2;
    public bool startRotTexts = false;
    public void Update()
    {
        if (startRotTexts)
        {
            RotateCircularTexTs();
        }
    }
    private Vector3 rot;
    private Vector3 rot_minus;
    public Transform blue_;
    public Transform red_;


    public void SetCircularTexts(string text)
    {
        for (int i = 0; i < circleTexts.Length; i++)
        {
            circleTexts[i].text = "";

            // CHECK MAXLENTCHTG

            int maxTimesWord = 88 / (text.Length +1);
            for (int y = 0; y < maxTimesWord; y++)
            {
                circleTexts[i].text += text.ToString() + " ";
            }
        }
    }

    /// red colors from light to less alpha
    public Color r_color_1;
    public Color r_color_2;
    public Color r_color_3;
    public Color r_color_4;
    public Color r_color_5;

    /// blue colors from light to less alpha
    public Color b_color_1;
    public Color b_color_2;
    public Color b_color_3;
    public Color b_color_4;
    public Color b_color_5;

    public Image gameBTN_1;
    public Image gameBTN_2;
    public TextMeshProUGUI multiplierTEXT;
    public TextMeshProUGUI innerCircle_text;
    public TextMeshProUGUI outerCircle_text;

    public void GameButtonColorChange(bool p)
    {
        if(p)
        {
            /// reds
            multiplierTEXT.color =      r_color_1;
            gameBTN_1.color =           r_color_2;
            gameBTN_2.color =           r_color_3;
            innerCircle_text.color =    r_color_4;
            outerCircle_text.color =    r_color_5;

        }
        if(!p)
        {
            /// blös
            multiplierTEXT.color =     b_color_1;
            gameBTN_1.color =          b_color_2;
            gameBTN_2.color =          b_color_3;
            innerCircle_text.color =   b_color_4;
            outerCircle_text.color =   b_color_5;

        }
    }
    
    public void RotateCircularTexTs()
    {
        if(Components.c.filetotext.pointerDown)
        {
            rot +=   Vector3.forward*45*Time.deltaTime;
            rot_minus +=   Vector3.forward*-45*Time.deltaTime;
            blue_.rotation = Quaternion.Euler(rot_minus);
            red_.rotation = Quaternion.Euler(rot);
        }
        if(!Components.c.filetotext.pointerDown)
        {
            rot +=   Vector3.forward*22.5f*Time.deltaTime;
            rot_minus +=   Vector3.forward*-22.5f*Time.deltaTime;
            red_.rotation = Quaternion.Euler(rot);
            blue_.rotation = Quaternion.Euler(rot_minus);
        }
    }
}
