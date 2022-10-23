using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocalisedStrings : MonoBehaviour
{
    public TextMeshProUGUI leaderBoardsTopPanel;
    public TextMeshProUGUI leaderBoards_score_text;
    public TextMeshProUGUI[] ChangeNameText;
    public TextMeshProUGUI[] ChangeLocaleText;
    public TextMeshProUGUI OK_text;
    public TextMeshProUGUI Cancel_text;
    public TextMeshProUGUI Settings_text;
    
    //trans 2
    public TextMeshProUGUI dg_dailytask_done_title;
    public TextMeshProUGUI dg_dailytask_done_content;
    public TextMeshProUGUI dg_noConnection_title;
    public TextMeshProUGUI dg_noConnection_content;
    public string hud_newTask_text;
    public string hud_dailyDone_text;
    public string hud_streak_text;
    public TextMeshProUGUI hud_sessionScore_text;
    public TextMeshProUGUI hud_rank_text;
    public TextMeshProUGUI lb_week_text;
    public TextMeshProUGUI lb_month_text;
    public TextMeshProUGUI lb_year_text;
    public TextMeshProUGUI splash_start_text;
    public TextMeshProUGUI settings_start_text;

    public  string score_perfect = "Perfect";
    public  string score_good = "Good";
    public  string score_ok = "OK";
    public  string score_noScore = "Try again";
    public  string game_skip = "Skip";
    public  string game_newWord = "New Word is ";
    public string ui_leaderboards = "Rankings";
    public string ui_selectLanguage = "Select game language";
    public string ui_changeName = "Change Name";
    public string ui_score = "Score";
    public string ui_OK = "OK";
    public string ui_cancel = "Cancel";
    public string ui_settings = "Settings";
    public string rank_localised = "";

    public void ChangeLocale(List<UI_Localised> locList)
    {
        foreach(UI_Localised ui_l in locList)
        {
            if(ui_l.variable == "score_perfect")
            {
                score_perfect = ui_l.translation;
            }
            if(ui_l.variable == "score_good")
            {
                score_good = ui_l.translation;
            }
            if(ui_l.variable == "score_ok")
            {
                score_ok  = ui_l.translation;
            }
            if(ui_l.variable == "score_noScore")
            {
                score_noScore  = ui_l.translation;
            }
            if(ui_l.variable == "game_skip")
            {
                game_skip  = ui_l.translation;
            }
            if(ui_l.variable == "game_newWord")
            {
                game_newWord = ui_l.translation;
            }
            if(ui_l.variable == "ui_leaderboards")
            {
                ui_leaderboards = ui_l.translation;
            }
            if(ui_l.variable == "ui_selectLanguage")
            {
                ui_selectLanguage  = ui_l.translation;
            }
            if(ui_l.variable == "ui_changeName")
            {
               ui_changeName = ui_l.translation;
            }
            if(ui_l.variable == "ui_score")
            {
                ui_score  = ui_l.translation;
            }
            if(ui_l.variable == "ui_OK")
            {
                ui_OK  = ui_l.translation;
            }
            if(ui_l.variable == "ui_cancel")
            {
                ui_cancel  = ui_l.translation;
            }
            if(ui_l.variable == "ui_settings")
            {
                ui_settings = ui_l.translation;
            }

            // trans 2 texts 
            if(ui_l.variable == "dg_dailytask_done_title")
            {
                dg_dailytask_done_title.text = ui_l.translation;
            }
            if(ui_l.variable == "dg_dailytask_done_content")
            {
                dg_dailytask_done_content.text = ui_l.translation;
            }
            if(ui_l.variable == "dg_noConnection_title")
            {
                dg_noConnection_title.text = ui_l.translation;
            }
            if(ui_l.variable == "dg_noConnection_content")
            {
                dg_noConnection_content.text = ui_l.translation;
            }
            if(ui_l.variable == "hud_newTask_text")
            {
                hud_newTask_text = ui_l.translation;
            }
            if(ui_l.variable == "hud_dailyDone_text")
            {
                hud_dailyDone_text = ui_l.translation;
            }
            if(ui_l.variable == "hud_streak_text")
            {
                hud_streak_text = ui_l.translation;
            }
            if(ui_l.variable == "hud_sessionScore_text")
            {
                hud_sessionScore_text.text = ui_l.translation;
            }
            if(ui_l.variable == "hud_rank_text")
            {
                hud_rank_text.text = ui_l.translation;
                rank_localised = ui_l.translation;
            }
            if(ui_l.variable == "lb_week_text")
            {
                lb_week_text.text = ui_l.translation;
            }
            if(ui_l.variable == "lb_month_text")
            {
                lb_month_text.text = ui_l.translation;
            }
            if(ui_l.variable == "lb_year_text")
            {
                lb_year_text.text = ui_l.translation;
            }
            if(ui_l.variable == "splash_start_text")
            {
                splash_start_text.text = ui_l.translation;
                settings_start_text.text = ui_l.translation;
            }
        }

        leaderBoardsTopPanel.text = ui_leaderboards;
        leaderBoards_score_text.text = ui_score;
        for (int i = 0; i < ChangeNameText.Length; i++)
        {
            ChangeNameText[i].text = ui_changeName; 
        }
        for (int i = 0; i < ChangeLocaleText.Length; i++)
        {
            ChangeLocaleText[i].text = ui_selectLanguage; 
        }
        OK_text.text = ui_OK;
        Cancel_text.text = ui_cancel;
        Settings_text.text = ui_settings;
    }
}
