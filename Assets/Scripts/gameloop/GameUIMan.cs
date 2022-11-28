using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameUIMan : MonoBehaviour
{
    public Button skipButton;
    public TextMeshProUGUI lifesIndicator;
    public TextMeshProUGUI skipsIndicator;
    public TextMeshProUGUI shieldsIndicator;
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
    public void UpdateUIToConfigs()
    {
        UpdateLifesIndicator();
        UpdateSkipsIndicator();
        UpdateShieldsIndicator();
    }

    public Sprite noHearts, yesHearts;
    public void UpdateLifesIndicator()
    {
        if(Components.c.settings.thisPlayer.current_Hearts == 0)
        {
            lifesIndicator.text = Components.c.settings.thisPlayer.current_Hearts.ToString() + 
            " / " + Components.c.settings.thisConfigs.max_Hearts; //+ "x";
        }
        else
        {

            lifesIndicator.text = Components.c.settings.thisPlayer.current_Hearts.ToString() + 
            " / " + Components.c.settings.thisConfigs.max_Hearts; //+ "x";
            heart_center.enabled = true;
        }
        if( Components.c.settings.thisPlayer.current_Hearts == 1)
        {
            lifesIndicator.text = Components.c.settings.thisPlayer.current_Hearts.ToString() + 
            " / " + Components.c.settings.thisConfigs.max_Hearts; //+ "x";
        }
    }
    public void UpdateSkipsIndicator()
    {
        if(Components.c.settings.thisPlayer.current_Skips == 0 || Components.c.settings.thisPlayer.current_Skips == 1 )
        {
            skipsIndicator.text = Components.c.settings.thisPlayer.current_Skips.ToString() + 
            " / " + Components.c.settings.thisConfigs.max_Skip_Amount; 

        }else
        {
            skipsIndicator.text = Components.c.settings.thisPlayer.current_Skips.ToString() + 
            " / " + Components.c.settings.thisConfigs.max_Skip_Amount; 
        }
    }
    public void UpdateShieldsIndicator()
    {
        //sheilds
        shieldsIndicator.text = Components.c.settings.thisPlayer.shield_count.ToString();
        Components.c.shieldButton.CheckStatusTo_GFX();
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
        HeartIconUpdates();
    }
    public float firstLBitem_yValue;
    public void ScrollLock()
    {
        // if(leaderboards.activeInHierarchy)
        // {
        //     if(Components.c.displayHighScores.lbGO_list.Count > 0)
        //     {
        //         if(Components.c.displayHighScores.lbGO_list[0].transform.position.y < firstLBitem_yValue)
        //         {
        //             Reset_lb_ScrollRectPos();
        //         }
        //     }
        // }
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
    public TMP_FontAsset japan_primary;
    public TMP_FontAsset arabic_primary;
    public TMP_FontAsset korean_primary;
    public TMP_FontAsset thai_primary;
    public TMP_FontAsset notosans_all;
    public void SetCircularTexts_FONT(string locale)
    {
        if(locale == "ar-AE")
        {
            for (int i = 0; i < circleTexts.Length; i++)
            {
                circleTexts[i].font = arabic_primary;
            }
            return;             
        }
        if(locale == "ja-JP")
        {
            for (int i = 0; i < circleTexts.Length; i++)
            {
                circleTexts[i].font = japan_primary;
            }
            return;   
        }
        if(locale == "ko-KR")
        {
            for (int i = 0; i < circleTexts.Length; i++)
            {
                circleTexts[i].font = korean_primary;
            }
            return;   
        }
        if(locale == "th-TH")
        {
            for (int i = 0; i < circleTexts.Length; i++)
            {
                circleTexts[i].font = thai_primary;
            }
            return;  
        }
        // have it normal in any other case ---- 
        for (int i = 0; i < circleTexts.Length; i++)
        {
            circleTexts[i].font = notosans_all;
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

    public Animation inner;
    //public Animation inner_BtR;
    public Animation outer;
    public Animation button;
    
    //public Animation outer_BtR;
    public void CircularTexts_ChangeColor_BtoR()
    {
        inner.Play("outer_red_to_blue");
        outer.Play("outer_red_to_blue");

        button.Play("button_r_to_b");
    }
    public void CircularTexts_ChangeColor_RtoB()
    {
        inner.Play("outer_blue_to_red");
        outer.Play("outer_blue_to_red");

        Debug.Log("");button.Play("button_b_to_r");
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
        if(Components.c.settings.thisPlayer.current_Hearts > 1)
        {
            //heart_center.fillAmount = 0;
            heart_shatter_line.enabled = false;
            heart_center.enabled = true;
            heart_center.transform.localScale = new Vector3(0,0,1);
            a_timer = anticipationTime;
            growheart = true;
        }
        if(Components.c.settings.thisPlayer.current_Hearts <= 1)
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
        //    namePrompt.SetActive(false);
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
            if(leaderboards.activeInHierarchy)
            {
                updateButtonText_tab_to_smallest_font();
            }
            if(lastMenu == leaderboards)
            {
                LB_button.SetActive(false);
                settings_button.SetActive(true);
                SpeakAgain_button.SetActive(false);
                //Components.c.fireStore_Manager.Get_LB_local_top10();
                LB_WEEK_BUTTON();

            }else
            {
                LB_button.SetActive(true);
                settings_button.SetActive(false);
                SpeakAgain_button.SetActive(false);
            }
        }
    }
    public GameObject debugMenu;
    public void ShowDebug()
    {
        if(debugMenu.activeInHierarchy)
        {
            debugMenu.SetActive(false);
            return;
        }
        if(!debugMenu.activeInHierarchy)
        {
            debugMenu.SetActive(true);
            return;
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
        updateButtonText_tab_to_smallest_font();
        //Components.c.fireStore_Manager.Get_LB_local_top10();
        infoButton.SetActive(false);
        LB_WEEK_BUTTON();
    }
    public GameObject nameChange;
    public void HideAllMenus()
    {
        settingsMenu.SetActive(false);
        leaderboards.SetActive(false);
        LB_button.SetActive(false);
        settings_button.SetActive(false);
        SpeakAgain_button.SetActive(true);
        infoButton.SetActive(false);

    }
    public void ShowSettingsMenu()
    {
        LB_button.SetActive(true);
        settings_button.SetActive(false);
        lastMenu = settingsMenu;
        leaderboards.SetActive(false);
        settingsMenu.SetActive(true);
        SpeakAgain_button.SetActive(false);
        infoButton.SetActive(true);
    }
    public GameObject infoButton;
    public TextMeshProUGUI inputfieldText;
    public void ShowNameChange()
    {
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
    public TextMeshProUGUI totalScore;
    public TextMeshProUGUI sessionScore;
    public TextMeshProUGUI playerName_score;
    public void UpdateScoreTo_UI()
    {
        totalScore.text = Components.c.settings.localeScore.ToString();
        sessionScore.text = Components.c.settings.sessionScore.ToString();
        playerName_score.text = Components.c.settings.thisPlayer.playerName.ToString();
    }

    public Slider timeBonusSlider;
    public TextMeshProUGUI timeBonusText;
    private bool timebonus = false;
    public void StartTimeBonusSlider(float lenght)
    {        
        timeBonusSlider.gameObject.SetActive(true);
        timeBonusSlider.maxValue = lenght;
        timeBonusSlider.value = lenght;
        timebonus = true;
        // 1.5x --- 3x ---- 5x --
        // 25 --- 25---- 25----
    }
    private void FixedUpdate()
    {
        if(timebonus)
        {
            timeBonusSlider.value -= Time.deltaTime;
        }
        if(timeBonusSlider.value <= 0)
        {
            timeBonusSlider.gameObject.SetActive(false);
            //Debug.Log("No timebonus!");
            timebonus = false;
        }

        Update_dailyTask_timeLeft();
    }
    public int GetTimeBonusMultiplier()
    {
        timebonus = false;
        float _tb = timeBonusSlider.value / timeBonusSlider.maxValue;
        if(_tb >= 0.5f)
        {
            timeBonusText.transform.gameObject.SetActive(true);
            timeBonusText.text = "5x";
            StartCoroutine(gfx_delay());
            //max bonus
            return 5;
        }
        if(_tb >= 0.25f)
        {
            timeBonusText.transform.gameObject.SetActive(true);
            timeBonusText.text = "3x";
            //mid bonus
            StartCoroutine(gfx_delay());
            return 3;
        }
        if(_tb <= 0.25f)
        {
            timeBonusText.transform.gameObject.SetActive(true);
            timeBonusText.text = "2x";
            //low bonus
            StartCoroutine(gfx_delay());
            return 2;
        }
        return 0;
    }
    public IEnumerator gfx_delay()
    {
        yield return new WaitForSeconds(1.5f);
        timeBonusText.text = "";
        timeBonusText.transform.gameObject.SetActive(false);
        timeBonusSlider.gameObject.SetActive(false);
    }
    public Animator wordScoreAnimator;
    public TextMeshProUGUI wordScoreText;
    public AnimationClip spawn;

    public void SpawnWordsScoreText(int score)
    {
        wordScoreAnimator.gameObject.SetActive(true);
        wordScoreText.text = score.ToString();
        wordScoreAnimator.Play("spawn_score");
        StartCoroutine(Wait_Seconds(2f));
    }

    public IEnumerator Wait_Seconds(float wait_duration)
    {
        yield return new WaitForSeconds(wait_duration);
        DisableWordsScore();
    }

    public void DisableWordsScore()
    {
        wordScoreAnimator.gameObject.SetActive(false);
    }

    public TextMeshProUGUI ranktext;
    public TextMeshProUGUI[] tab_texts;
    // public TextMeshProUGUI tab_month_text;
    // public TextMeshProUGUI tab_year_text;
    // public TextMeshProUGUI tab_all_time_text;

    private float smallestFont = 999;
    public void updateButtonText_tab_to_smallest_font()
    {
        for (int i = 0; i < tab_texts.Length; i++)
        {
            if(tab_texts[i].fontSize < smallestFont)
            {
                
                smallestFont = tab_texts[i].fontSize;
            }
        }
        for (int i = 0; i < tab_texts.Length; i++)
        {
            tab_texts[i].fontSizeMax = smallestFont;
        }

        leaderboards.gameObject.SetActive(false);
        leaderboards.gameObject.SetActive(true);
    }
    public int rank;
    public void UpdateRankText()
    {
        rank = 1;
        for (int i = 0; i < Components.c.settings.lb_wrap.rank_scores.Count; i++)
        {
            if(Components.c.settings.lb_wrap.rank_scores[i] > Components.c.settings.localeScore)
            {
                rank++;
            }
        }
        ranktext.text = Components.c.localisedStrings.lb_month_text.text + " " + Components.c.localisedStrings.rank_localised +"\n"+ "#" + rank;
        // + "\n " + Components.c.localisedStrings.lb_month_text.text + ": # monthlylyrank"
        // + "\n " + Components.c.localisedStrings.lb_year_text.text  + ": # yealyrank";
    }

    public TextMeshProUGUI leaderboardsTITLE_text;
    public void LB_WEEK_BUTTON()
    {
        string type = "week";
        StartCoroutine(Components.c.fireStore_Manager.get_LB_ButtonPress(type));
        leaderboardsTITLE_text.text = Components.c.fireStore_Manager.week_lb_title;
    }
    public void LB_MONTH_BUTTON()
    {
        string type = "month";
        StartCoroutine(Components.c.fireStore_Manager.get_LB_ButtonPress(type));
        leaderboardsTITLE_text.text = Components.c.fireStore_Manager.month_lb_title;
    }
    public void LB_YEAR_BUTTON()
    {
        string type = "year";
        StartCoroutine(Components.c.fireStore_Manager.get_LB_ButtonPress(type));
        leaderboardsTITLE_text.text = Components.c.fireStore_Manager.year_lb_title;
        // Reset_lb_ScrollRectPos();
    }
    public void LB_ALLTIME_BUTTON()
    {
        string type = "alltime";
        StartCoroutine(Components.c.fireStore_Manager.get_LB_ButtonPress(type));
        leaderboardsTITLE_text.text = Components.c.fireStore_Manager.alltime_lb_title;
        //  Reset_lb_ScrollRectPos();
    }

    public TextMeshProUGUI monthInfoText;
    public TextMeshProUGUI streakText;
    public TextMeshProUGUI toClear_numberText;
    public GameObject DailyQuest_OG_parent;
    public GameObject DailyQuest_splash_parent;
    public GameObject DailyQuestHolder;

    public void Update_dailyTask_timeLeft()
    {
        //var tomorrow = new DateTime(DateTime.UtcNow.AddDays(1).Year,DateTime.UtcNow.AddDays(1).Month, DateTime.UtcNow.AddDays(1).Day);
        ui_TimeLeft.text = Components.c.localisedStrings.hud_newTask_text + ":\n" + (Components.c.settings.tomorrow - DateTime.UtcNow).Hours.ToString() + "h "+(Components.c.settings.tomorrow - DateTime.UtcNow).Minutes.ToString() + "min " + (Components.c.settings.tomorrow- DateTime.UtcNow).Seconds.ToString() + "s";
        if(Components.c.settings.tomorrow.Day == DateTime.UtcNow.Day && Components.c.gameManager.startSplashInfo)
        {
            Components.c.settings.CheckStreak();
        }
    }
    public GameObject DG_DailyDone;
    public void SpawnCongratz()
    {
        if(!DG_DailyDone.activeInHierarchy)
        {
            ui_toClear_numberText.text = Components.c.localisedStrings.hud_dailyDone_text;
            DG_DailyDone.SetActive(true);
        }
    }
    //public bool dailyDone = false;
    public TextMeshProUGUI ui_monthInfoText;
    public TextMeshProUGUI ui_streakText;
    public TextMeshProUGUI ui_toClear_numberText;
    public TextMeshProUGUI ui_TimeLeft;
    
    public void Update_UI_DailyStreak()
    {
        var today = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);
        int daysInMonth = DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month);
        int curDay = DateTime.UtcNow.Day;
        ui_monthInfoText.text = curDay.ToString() + " / " + daysInMonth.ToString();

        if((today - DateTime.Parse(Components.c.settings.thisPlayer.DailyTasksDoneDate)).Days == 0)
        {
            if(Components.c.settings.thisPlayer.dailyTaskStreak == 0)
            {
                Debug.Log("streak 0.... ");
            }else
            {
                ui_monthInfoText.text = curDay.ToString() + " / " + daysInMonth.ToString();
                ui_streakText.text = Components.c.localisedStrings.hud_streak_text + " : " + Components.c.settings.thisPlayer.dailyTaskStreak.ToString();
                ui_toClear_numberText.text = Components.c.localisedStrings.hud_dailyDone_text;
                return;
            }
        }
        if(Components.c.settings.thisPlayer.dailyTaskStreak > 0)
        {
            ui_streakText.text = Components.c.localisedStrings.hud_streak_text + " : " + Components.c.settings.thisPlayer.dailyTaskStreak.ToString();
        }else
        {
            ui_streakText.text = "";
        }
        int toClear = Components.c.settings.thisConfigs.dailyTask_baseValue + (Components.c.settings.thisPlayer.dailyTaskStreak * Components.c.settings.thisConfigs.dailyTask_increment);
        ui_toClear_numberText.text = Components.c.localisedStrings.hud_completed + "\n" + Components.c.settings.thisPlayer.dailyTaskWordsComplete.ToString() + " / " + toClear.ToString();
    }
    public Animation text_sizeHighlight;
    public void HighlightText_DailyStreak()
    {
        text_sizeHighlight.Play();
    }

public GameObject DG_noInternet;
public ScrollRect lb_ScrollRect;
public void Reset_lb_ScrollRectPos()
{
    StartCoroutine(UpdateScrollRect());
}
    public IEnumerator UpdateScrollRect()
    {
        yield return new WaitForEndOfFrame();
        lb_ScrollRect.verticalNormalizedPosition = 1f;
    }
}