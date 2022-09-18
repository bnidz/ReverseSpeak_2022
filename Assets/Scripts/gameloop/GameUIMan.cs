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
            //heart_center.enabled = false;
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

    public ntw.CurvedTextMeshPro.TextProOnACircle  b1;
    public ntw.CurvedTextMeshPro.TextProOnACircle  b2;
    public ntw.CurvedTextMeshPro.TextProOnACircle r1;
    public ntw.CurvedTextMeshPro.TextProOnACircle r2;

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
    public TextMeshProUGUI innerCircle_text_blue;
    public TextMeshProUGUI innerCircle_text_red;
    public TextMeshProUGUI outerCircle_text_blue;
    public TextMeshProUGUI outerCircle_text_red;

    // public void GameButtonColorChange(bool p)
    // {
    //     if(p)
    //     {

    //         /// reds
    //         multiplierTEXT.color =      r_color_1;
    //         gameBTN_1.color =           r_color_2;
    //         gameBTN_2.color =           r_color_3;
    //         innerCircle_text_blue.gameObject.SetActive(false);      
    //         innerCircle_text_blue.gameObject.SetActive(false);      
    //         outerCircle_text_red.gameObject.SetActive(true);       
    //         outerCircle_text_red.gameObject.SetActive(true);       

    //     }
    //     if(!p)
    //     {

    //         /// blös
    //         multiplierTEXT.color =     b_color_1;
    //         gameBTN_1.color =          b_color_2;
    //         gameBTN_2.color =          b_color_3;

    //         innerCircle_text_blue.gameObject.SetActive(true);      
    //         outerCircle_text_red.gameObject.SetActive(false);       
    //         outerCircle_text_red.gameObject.SetActive(false);       
    //         innerCircle_text_blue.gameObject.SetActive(true);      

    //     }
    // }

    public void ChangeOuterRingColor(bool r)
    {
        if(r)
        {
            outerCircle_text_blue.enabled = false; //SetActive(false);       
            outerCircle_text_red.enabled = true; //SetActive(true);       
        }
        else
        {
            outerCircle_text_red.enabled = false;       
            outerCircle_text_blue.enabled = true;       
        }
    }

    public void ChangeInnerRingColor(bool r)
    {
        if(r)
        {
            innerCircle_text_blue.enabled = false;      
            innerCircle_text_red.enabled = true;      
        }
        else
        {
            innerCircle_text_red.enabled = false;      
            innerCircle_text_blue.enabled = true;     
        }
    }
    public void ChangeGameButtonColor(bool c)
    {
        if(c)
        {
            gameBTN_1.color =           r_color_2;
            gameBTN_2.color =           r_color_3;   
        }
        else
        {
            gameBTN_1.color =           b_color_2;
            gameBTN_2.color =           b_color_3;
        }
    }
    
    public void RotateCircularTexTs()
    {
        // if(Components.c.filetotext._pointerDown)
        // {
        //     rot +=   Vector3.forward*45*Time.deltaTime;
        //     rot_minus +=   Vector3.forward*-45*Time.deltaTime;
        //     blue_.rotation = Quaternion.Euler(rot_minus);
        //     red_.rotation = Quaternion.Euler(rot);
        // }
        // if(!Components.c.filetotext._pointerDown)
        // {
        //     rot +=   Vector3.forward*22.5f*Time.deltaTime;
        //     rot_minus +=   Vector3.forward*-22.5f*Time.deltaTime;
        //     red_.rotation = Quaternion.Euler(rot);
        //     blue_.rotation = Quaternion.Euler(rot_minus);
        // }
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
        heart_center.enabled = true;
        heart_shatter_line.enabled = true;
        heart_shatter_line.fillAmount = 0;
        a_timer = anticipationTime;
        heart_center.transform.localScale = new Vector3(1,1,1);
        growheart = false;
        //DO SHATTERLINE IN CENTER
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

    public void EmptyToOneHeart()
    {
        heart_center.transform.localScale = new Vector3(0,0,1);
        heart_center.enabled = true;
        scale = 0;
        growheart = true;
    }

    //private Vector3 ogScale;
    private void Heart_Grow_one_Update()
    {
        if (growheart)
        {
            if(scale <= 1)
            {
                a_timer -= Time.deltaTime;
                if(a_timer <= 0)
                {
                    scale += 0.055f; // * Time.deltaTime;
                    heart_center.transform.localScale = new Vector3(scale, scale, 0);
                }
            }
            if(scale >= 1)
            {
                heart_center.transform.localScale = new Vector3(1, 1, 1);
                growheart = false;
            }
        }
    }

    private float anticipationTime = .27f;
    private float a_timer;
    private void Heart_Lose_Update()
    {
        if(doShatter)
        {
            //fill vertical from up to down ---> 2s
            if(heart_shatter_line.fillAmount <= 1)
            {
                heart_shatter_line.fillAmount += 1.0f / .7f * Time.deltaTime;
            }
            if(heart_shatter_line.fillAmount >= 1)
            {
            //spawn two pieces
                a_timer -= Time.deltaTime;
                if(a_timer <= 0)
                {
                    heart_shatter_line.enabled = false;
                    DropPieces();               
                    doShatter = false;
                }
            }
        }
    }

    private void DropPieces()
    {
        scale = 0;
        if(Components.c.settings.currentPlayer.current_Hearts > 1)
        {
            //heart_center.fillAmount = 0;
            heart_shatter_line.enabled = false;
            heart_center.enabled = true;
            heart_center.transform.localScale = new Vector3(0,0,1);
            a_timer = anticipationTime;
            growheart = true;
        }
        if(Components.c.settings.currentPlayer.current_Hearts <= 1)
        {
            heart_center.enabled = false;
            growheart = false;
        }
        
        heart_shatter_line.enabled = false;
        heart_broken_piece_left.enabled = true;
        heart_broken_piece_left.gameObject.GetComponent<heart_piece>().Drop(heart_broken_piece_left);
        heart_broken_piece_right.enabled = true;   
        heart_broken_piece_right.gameObject.GetComponent<heart_piece>().Drop(heart_broken_piece_right);
        //heart_center.enabled = false;
    }

    public GameObject namePrompt;
    public void HideLogin()
    {
        namePrompt.SetActive(false);
    }

    public GameObject settingsMenu;
    public GameObject LB_button;
    public GameObject settings_button;
    private GameObject lastMenu;
    private bool firstTime = true;
    public void ShowMenu()
    {
        if(firstTime)
        {
            lastMenu = leaderboards;
            firstTime = false;
        }
        if(lastMenu.activeInHierarchy)
        {
            if(lastMenu == leaderboards || lastMenu == settingsMenu)
            {
                HideAllMenus();
                return;
            }
        }else
        {
            lastMenu.SetActive(true);
            if(lastMenu == leaderboards)
            {
                LB_button.SetActive(false);
                settings_button.SetActive(true);

            }else
            {
                LB_button.SetActive(true);
                settings_button.SetActive(false);
            }
        }
    }
    public GameObject leaderboards;
    public GameObject SpeakAgain_button;
    public void ShowLeaderboards()
    {
            LB_button.SetActive(false);
            settings_button.SetActive(true);
            lastMenu = leaderboards;
            leaderboards.SetActive(true);
            settingsMenu.SetActive(false);
            SpeakAgain_button.SetActive(false);
    }
    public GameObject nameChange;
    public void HideAllMenus()
    {
        settingsMenu.SetActive(false);
        leaderboards.SetActive(false);
        LB_button.SetActive(false);
        settings_button.SetActive(false);
        SpeakAgain_button.SetActive(true);
    }
    public void ShowSettingsMenu()
    {
            LB_button.SetActive(true);
            settings_button.SetActive(false);
            lastMenu = settingsMenu;
            leaderboards.SetActive(false);
            settingsMenu.SetActive(true);
            SpeakAgain_button.SetActive(false);
    }
    public TextMeshProUGUI inputfieldText;
    public void ShowNameChange()
    {
        //nameChange.GetComponentInChildren<TMP_InputField>().gameObject.GetComponentInChildren<TextMeshProUGUI>().text
        //inputfieldText.text = Components.c.settings.currentPlayer.playerName;
        nameChange.SetActive(true); 
        Cancel_to_nameChange_button.SetActive(true);
        Submit_to_nameChange_button.SetActive(false);
    }

    public GameObject Cancel_to_nameChange_button;
    public GameObject Submit_to_nameChange_button;
    public void ChangeNameChangeButton()
    {
        Cancel_to_nameChange_button.SetActive(false);
        Submit_to_nameChange_button.SetActive(true);
    }

    public void ButtonNanmeCancel()
    {
        nameChange.SetActive(false);
    }
}