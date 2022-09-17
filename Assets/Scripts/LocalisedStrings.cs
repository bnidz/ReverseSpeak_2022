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

    // spoken strings
    public string[] Perfect_Score = {"Perfect!","Täydet pisteet", "parfaite!","Perfekt!"};
    public string[] Good_Score = {"Good!","Hyvä", "bien!","Gut!"};
    public string[] OK_Score = {"OK","OK", "d'accord","OK"};
    public string[] No_Score = {"Too bad, try again","Yritä uudelleen", "dommage réessayer","Schade, versuchen Sie es erneut"};
    public string[] SKIP = {"Skip","Ohita", "Sauter","Überspringen"};

    // menu etch strings

    public string[] string_LeaderBoards = {"Leaderboards","Tilastotaulukot", "Classements","Bestenlisten"};
    public string[] string_SelectLanguage = {"Select game language","Valitse pelikieli", "changer la langue du jeu","Spielsprache auswählen"};
    public string[] string_ChangeName = {"Change Name","Vaihda pelaajanimi", "changez votre nom","Namen ändern"};
    public string[] string_Score = {"Score","Pisteet", "Score","Punktzahl"};
    public string[] string_OK = {"OK","OK", "D'accord","OK"};
    public string[] string_Cancel = {"Cancel","Peruuta", "Annuler","Absagen"};
    public string[] string_Settings = {"Settings","Asetukset", "Réglages","Einstellungen"};

    public void ChangeLanguage(int localeIndex)
    {

        // 1 = en-US
        // 2 = fi-FI
        // 3 = fr-FR
        // 4 = de-DE

        leaderBoardsTopPanel.text = string_LeaderBoards[localeIndex];
        leaderBoards_score_text.text = string_Score[localeIndex];
        ChangeNameText.text = string_ChangeName[localeIndex];
        ChangeLocaleText.text = string_SelectLanguage[localeIndex];
        OK_text.text = string_OK[localeIndex];
        Cancel_text.text = string_Cancel[localeIndex];
        
    }
}
