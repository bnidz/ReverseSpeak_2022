using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIMan : MonoBehaviour
{
    public Button skipButton;

    public TextMeshProUGUI lifesIndicator;
    public TextMeshProUGUI skipsIndicator;

    public TextMeshProUGUI skipsTimer;
    public TextMeshProUGUI lifesTimer;

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
            //lifesIndicator.gameObject.GetComponentInParent<Image>().sprite = noHearts;
            heart_center.enabled = false;
            //show empty heart
        }
        else
        {
            lifesIndicator.text = Components.c.settings.currentPlayer.current_Hearts.ToString() + "x";
            //lifesIndicator.gameObject.GetComponentInParent<Image>().sprite = yesHearts;
            //show full heart
            heart_center.enabled = true;
            

        }
        if( Components.c.settings.currentPlayer.current_Hearts == 1)
        {
            lifesIndicator.text = "";
            //lifesIndicator.gameObject.GetComponentInParent<Image>().sprite = yesHearts;
            //show full heart
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

        HeartIconUpdates();


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


    public Image heart_broken_piece_left;
    public Image heart_broken_piece_right;
    public Image heart_empty_bg;
    public Image heart_center;
    public Image heart_shatter_line;


    public void Hearts_Generating()
    {
        //CHANGE HOURGLASS TO HEART "CONTAINER" REGENERATING 

    }

    public void Heart_Lose_Life()
    {
        growheart = false;
        heart_center.transform.localScale = new Vector3(1,1,1);
        //DO SHATTERLINE IN CENTER
        
        heart_shatter_line.fillAmount = 0;
        heart_shatter_line.enabled = true;

        doShatter = true;

    }
    private bool doShatter = false;
    private bool growheart = false;
    private float scale;
    public void HeartIconUpdates()
    {
        if(doShatter)
        {
            Heart_Lose_Update();
        }
        if(growheart)
        {
            Heart_Grow_one_Update();
        }
    }

    //private Vector3 ogScale;
    private void Heart_Grow_one_Update()
    {
        if (growheart)
        {
            if(heart_center.fillAmount <= 1)
            {
                heart_shatter_line.fillAmount += 1.0f / 2 * Time.deltaTime;

            }
            if(heart_shatter_line.fillAmount >= 1)
            {
                growheart = false;
            }
        }
    }

    private void Heart_Lose_Update()
    {
        if(doShatter)
        {
            //fill vertical from up to down ---> 2s
            if(heart_shatter_line.fillAmount <= 1)
            {
                heart_shatter_line.fillAmount += 1.0f / 2 * Time.deltaTime;
            }
            if(heart_shatter_line.fillAmount >= 1)
            {
            //spawn two pieces
                DropPieces();                
                doShatter = false;
            }
        }

    }

    private void DropPieces()
    {
        if(Components.c.settings.currentPlayer.current_Hearts >= 1)
        {
            heart_center.fillAmount = 0;
            heart_center.enabled = true;
            growheart = true;
        }
        heart_broken_piece_left.enabled = true;
        heart_broken_piece_left.gameObject.GetComponent<heart_piece>().Drop(heart_broken_piece_left);
        heart_broken_piece_right.enabled = true;   
        heart_broken_piece_right.gameObject.GetComponent<heart_piece>().Drop(heart_broken_piece_right);
        heart_shatter_line.enabled = false;
        heart_center.enabled = false;
    }
}