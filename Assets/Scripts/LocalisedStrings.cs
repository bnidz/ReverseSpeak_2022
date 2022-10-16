using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocalisedStrings : MonoBehaviour
{
    public TextMeshProUGUI leaderBoardsTopPanel;
    public TextMeshProUGUI leaderBoards_score_text;
    public TextMeshProUGUI ChangeNameText;
    public TextMeshProUGUI ChangeLocaleText;
    public TextMeshProUGUI OK_text;
    public TextMeshProUGUI Cancel_text;
    public TextMeshProUGUI Settings_text;

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

    public void ChangeLanguage(string locale)
    {
        leaderBoardsTopPanel.text = ui_leaderboards;
        leaderBoards_score_text.text = ui_score;
        ChangeNameText.text = ui_changeName;
        ChangeLocaleText.text = ui_changeName;
        OK_text.text = ui_OK;
        Cancel_text.text = ui_cancel;
        Settings_text.text = ui_settings;
    }

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
        }

        leaderBoardsTopPanel.text = ui_leaderboards;
        leaderBoards_score_text.text = ui_score;
        ChangeNameText.text = ui_changeName;
        ChangeLocaleText.text = ui_changeName;
        OK_text.text = ui_OK;
        Cancel_text.text = ui_cancel;
        Settings_text.text = ui_settings;
    }
}
