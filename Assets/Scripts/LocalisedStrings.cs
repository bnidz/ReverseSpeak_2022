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
    [HideInInspector]
    public string[] Perfect_Score = 
    {"Perfect!",
    "Täydet pisteet",
     "parfaite!",
     "Perfekt!"};
    [HideInInspector]
    public string[] Good_Score = 
    {"Good!",
    "Hyvä",
     "bien!",
     "Gut!"};
    [HideInInspector]
    public string[] OK_Score = 
    {"OK",
    "OK",
     "d'accord",
     "OK"};
    [HideInInspector]
    public string[] No_Score = 
    {"Too bad, try again",
    "Yritä uudelleen",
     "dommage réessayer",
     "Schade, versuchen Sie es erneut"};
    [HideInInspector]
    public string[] SKIP = 
    {"Skip",
    "Ohita",
     "Sauter",
     "Überspringen"};
    [HideInInspector]
    public string[] NewWordIS = 
    {"New Word is",
    "Uusi sana on",
     "Le nouveau mot est",
      "Neues Wort ist"};

    // menu etch strings

    [HideInInspector]
    private string[] string_LeaderBoards = 
    {"Leaderboards",
    "Tilastotaulukot",
     "Classements",
     "Bestenlisten"};
    [HideInInspector]
    private string[] string_SelectLanguage = 
    {"Select game language",
    "Valitse pelikieli",
     "changer la langue du jeu",
     "Spielsprache auswählen"};
    [HideInInspector]
    private string[] string_ChangeName = 
    {"Change Name",
    "Vaihda pelaajanimi",
     "changez votre nom",
     "Namen ändern"};
    [HideInInspector]
    private string[] string_Score = 
    {"Score",
    "Pisteet",
     "Score",
     "Punktzahl"};
    [HideInInspector]
    private string[] string_OK = 
    {"OK",
    "OK",
     "D'accord",
     "OK"};
    [HideInInspector]
    private string[] string_Cancel = 
    {"Cancel",
    "Peruuta",
     "Annuler",
     "Absagen"};
    [HideInInspector]
    private string[] string_Settings = 
    {"Settings",
    "Asetukset",
     "Réglages",
     "Einstellungen"};

    

    [HideInInspector]
    public int currentLocale = 1;
    public void ChangeLanguage(int localeIndex)
    {
        currentLocale = localeIndex;
        // 1 = en-US
        // 2 = fi-FI
        // 3 = fr-FR
        // 4 = de-DE

        leaderBoardsTopPanel.text = string_LeaderBoards[localeIndex];
        Debug.Log("1");
        leaderBoards_score_text.text = string_Score[localeIndex];
        Debug.Log("2");
        ChangeNameText.text = string_ChangeName[localeIndex];
        Debug.Log("3");
        ChangeLocaleText.text = string_SelectLanguage[localeIndex];
        Debug.Log("4");
        OK_text.text = string_OK[localeIndex];
        Debug.Log("5");
        Cancel_text.text = string_Cancel[localeIndex];
        Debug.Log("6");
        Settings_text.text = string_Settings[localeIndex];
        Debug.Log("7");
        
    }
}
