using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TextSpeech;
using Apple.GameKit;
using System;
using TMPro;
using System.Text;
using System.Linq;
using System.IO;

public class GameLoop : MonoBehaviour
{
    public TextMeshProUGUI WORD;
    public TextMeshProUGUI inverted_WORD;
    public string currentWORD;
    public WordClass activeWord;

    private float delaytimer;

    public void Init()
    {
        TextToSpeech.instance.onReadyToSpeakCallback = onReadyToSpeakCallback;
        nextWord = false;
        Components.c.gameUIMan.UpdateMultiplier_UI(Components.c.settings.thisPlayer.multiplier);
        nextWord = true;
        Components.c.gameUIMan.startRotTexts = true;

        delaytimer = delay;
    }
    private void DebugValuesToPlayer()
    {
        Components.c.settings.thisPlayer.playerMaxMultiplier = 5;
        Debug.Log("warning debug values in useeeee!!!!!!!");
    }
    public void NewRandomWORD()
    {
        //if(!checkingWords)
        //{
            nextWord = false;
            activeWord  = Components.c.settings.gameWords[UnityEngine.Random.Range(0, Components.c.settings.gameWords.Count)];
            //string[] task_words = activeWord.word.ToLower().Split(' ');
            currentWORD = activeWord.word; //task_words[UnityEngine.Random.Range(0, task_words.Length)]; //lw.gameWordsList.Count)];
            WORD.text = currentWORD.ToUpper().ToString();
            inverted_WORD.text = WORD.text;
            Components.c.gameUIMan.SetCircularTexts(currentWORD);
            Components.c.settings.activeWORD = activeWord.word;
            
            StartCoroutine(Wait_and_Speak(Components.c.localisedStrings.game_newWord + currentWORD.ToString()));
            /// ENABLE SPEECH BUTTON FOR SCORIGN
            Components.c.fireStore_Manager.Get_Rank();
        //}
    }
    public void SpeakWordAgain()
    {
        StartCoroutine(Wait_and_Speak(currentWORD));   
    }
    //public Button skipButton;
    public void SkipWord()
    {
        if(Components.c.settings.thisPlayer.current_Skips > 0)
        {
            activeWord = new WordClass();
            activeWord.word = currentWORD;
            activeWord.times_skipped++;
            StartCoroutine(_wait_Update_WordData(activeWord));
            Components.c.fireStore_Manager.Update_WordData(activeWord);

            //StartCoroutine(Wait_and_Speak("Skipping a word! Good Luck"));
            Components.c.settings.thisPlayer.timesSkipped++;
            NewRandomWORD();
            Components.c.settings.thisPlayer.current_Skips--;
        }
        if(Components.c.settings.thisPlayer.current_Skips == 0)
        {
            Components.c.gameUIMan.DeactivateSkipButton();
        }
        SaveALL();
        Components.c.gameUIMan.UpdateSkipsIndicator();
    }
        private string results;
        private bool judgingDone_ActivateButton = true;
        public IEnumerator _wait_Update_WordData(WordClass w)
        {
            Components.c.dadabaseManager.waiting_ = true;
            Components.c.dadabaseManager.Update_WordData(w);
            while (Components.c.dadabaseManager.waiting_) yield return null;
        }
        public string DecodeFromUtf8(string utf8String)
        {
            byte[] utf8Bytes = new byte[utf8String.Length];
            for (int i=0;i<utf8String.Length;++i) {
                utf8Bytes[i] = (byte)utf8String[i];
            }

            return Encoding.UTF8.GetString(utf8Bytes,0,utf8Bytes.Length);
        }
    public void CheatScore()
    {
        
        int score = 100;
        StartCoroutine(Wait_and_Speak(Components.c.localisedStrings.score_perfect));
        if (Components.c.settings.thisPlayer.multiplier < Components.c.settings.thisPlayer.playerMaxMultiplier)
        {
            Components.c.settings.thisPlayer.multiplier++;
        }
        Components.c.settings.thisPlayer.totalScore += Convert.ToInt32((score * Components.c.settings.thisPlayer.multiplier));
        Components.c.settings.thisPlayer.timesQuessed++;
        Components.c.settings.thisPlayer.totalTries++;

        Components.c.settings.sessionScore += Convert.ToInt32((score * Components.c.settings.thisPlayer.multiplier));
        Components.c.settings.lastScore = Convert.ToInt32((score * Components.c.settings.thisPlayer.multiplier));

        Components.c.settings.localeScore += Convert.ToInt32((score * Components.c.settings.thisPlayer.multiplier));
        Components.c.gameUIMan.SpawnWordsScoreText(Convert.ToInt32((score * Components.c.settings.thisPlayer.multiplier)));
        Components.c.settings.DailyTaskWordComplete();
        Components.c.settings.CheckStreak();

        Components.c.settings.SavePlayerdDataToFile();
        Components.c.shieldButton.CheckStatusTo_GFX();
        score = 0;
        Components.c.gameUIMan.UpdateRankText();
        nextWord = true;          
    }
    public void SCORING(string results)
    {
        Components.c.filetotext.canPushButton = false;
        Debug.Log("-------------------------------------------------------");
        List<string> results_strings = ExtractFromBody(results, "substring","phoneSequence");
        Debug.Log(results_strings.Count);

        //SCORING
        //results divided by space
        float score = 1;
        string all = "";
        for (int i = 0; i < results.Length; i++)
        {
            all += results[i];
        }
        List<string> chanches = ExtractFromBody(all, "substring=",",");
        bool match = false;
        results = "";
        for (int i = 0; i < chanches.Count; i++)
        {
            string toCHECK = System.Text.RegularExpressions.Regex.Unescape(chanches[i].ToLower());
            results += "\n" + toCHECK.ToString();
            results += " " + i + " / " + chanches.Count;
            if(toCHECK.Contains((currentWORD.ToLower())))
            {
                if(i == 0)
                {
                    score = 1;
                    Debug.Log(chanches[i].ToUpper());
                    match = true;
                    break;
                }else
                {
                    score = score / i;
                    Debug.Log(chanches[i].ToUpper());
                    match = true;
                    break;
                }
            }
        }

        Debug.Log(results.ToString());
        Components.c.sampleSpeechToText.resultListText.text = results;
        if(match == false)
        {
            score = 0;
            // make wrong choises spawn
            string[] wrongWords = chanches.ToArray();
            Components.c.wrongSpawner.SpawnWrongAnswers(wrongWords);
        }

        Debug.Log("score ; " + score + " / " + chanches.Count );
        score *= 100;
        Debug.Log("score = " + score + "%");

        results_strings.Clear();
        // SCORE CURRENT WORD
        if(score > 0)
        {
            Components.c.gameUIMan.GetTimeBonusMultiplier();
            // FX - PERFECT
            if(score == 100)
            {
                StartCoroutine(Wait_and_Speak(Components.c.localisedStrings.score_perfect));
                if (Components.c.settings.thisPlayer.multiplier < Components.c.settings.thisPlayer.playerMaxMultiplier)
                {
                    Components.c.settings.thisPlayer.multiplier++;
                }
            }
            // FX - GOOD
            if(score >= 50 && score != 100)
            {
                StartCoroutine(Wait_and_Speak(Components.c.localisedStrings.score_good));
            }
            // FX - ALRIGHT
            if(score < 50)
            {
                StartCoroutine(Wait_and_Speak(Components.c.localisedStrings.score_ok));
                if (Components.c.settings.thisPlayer.multiplier > 1 && !Components.c.settings.isActiveShield)
                {
                    Components.c.settings.thisPlayer.multiplier--;
                }
                if (Components.c.settings.thisPlayer.multiplier > 1 && Components.c.settings.isActiveShield)
                {
                    //Components.c.settings.thisPlayer.shield_count--;
                    Components.c.shieldButton.DeActivateShield();
                }
            }
            //FRES WORD VALUES SINCE RIGHT - SO UPDATE DATABASE WORD VALUES ---
            activeWord = new WordClass();
            activeWord.times_tried++;
            activeWord.times_right++;
            activeWord.word = currentWORD;
            activeWord.total_score += (score);

            Components.c.fireStore_Manager.Update_WordData(activeWord);
            //StartCoroutine(_wait_Update_WordData(activeWord));
            Components.c.dadabaseManager.waiting_ = false;
            //Components.c.dadabaseManager._ = false;;
            Components.c.settings.thisPlayer.totalScore += Convert.ToInt32((score * Components.c.settings.thisPlayer.multiplier)  * (Components.c.settings.thisPlayer.dailyTaskStreak +1));
            Components.c.settings.thisPlayer.timesQuessed++;
            Components.c.settings.thisPlayer.totalTries++;

            Components.c.settings.sessionScore += Convert.ToInt32((score * Components.c.settings.thisPlayer.multiplier) * (Components.c.settings.thisPlayer.dailyTaskStreak +1));
            Components.c.settings.lastScore = Convert.ToInt32((score * Components.c.settings.thisPlayer.multiplier) * (Components.c.settings.thisPlayer.dailyTaskStreak +1));
            Components.c.settings.DailyTaskWordComplete();
            Components.c.settings.CheckStreak();

            if(Components.c.settings.thisPlayer.dailyTaskStreak > 1)
            {
                Components.c.gameUIMan.HighlightText_DailyStreak();
            }



            Components.c.settings.localeScore += Convert.ToInt32((score * Components.c.settings.thisPlayer.multiplier) * (Components.c.settings.thisPlayer.dailyTaskStreak +1));
            Components.c.gameUIMan.SpawnWordsScoreText(Convert.ToInt32((score * Components.c.settings.thisPlayer.multiplier) * (Components.c.settings.thisPlayer.dailyTaskStreak +1)));
            Components.c.settings.SavePlayerdDataToFile();
            Components.c.shieldButton.CheckStatusTo_GFX();
            score = 0;
            Components.c.gameUIMan.UpdateRankText();
            Resources.UnloadUnusedAssets();
            nextWord = true;

        }
        else
        {
        if (Components.c.settings.thisPlayer.multiplier > 1 && !Components.c.settings.isActiveShield)
        {
            Components.c.settings.thisPlayer.multiplier = 1;
        }
        if (Components.c.settings.thisPlayer.multiplier > 1 && Components.c.settings.isActiveShield)
        {
            Components.c.shieldButton.DeActivateShield();
        }
            //REDUCE LIFE
            activeWord = new WordClass();
            activeWord.times_tried++;
            activeWord.word = currentWORD;
            Components.c.fireStore_Manager.Update_WordData(activeWord);
            Components.c.settings.thisPlayer.totalTries++;
            StartCoroutine(Wait_and_Speak(Components.c.localisedStrings.score_noScore));
            Components.c.gameUIMan.UpdateLifesIndicator();
            judgingDone_ActivateButton = true;
            if(Components.c.settings.thisPlayer.current_Hearts >= 1)
            {
                Components.c.gameUIMan.Heart_Lose_Life();
                Components.c.settings.thisPlayer.current_Hearts--;
                Components.c.gameUIMan.UpdateLifesIndicator();
            }
            
            if(Components.c.settings.thisPlayer.current_Hearts < 1)
            {
                Components.c.settings.thisPlayer.current_Hearts = 0;
                Components.c.gameUIMan.UpdateLifesIndicator();
            }
            Components.c.gameUIMan.UpdateMultiplier_UI(Components.c.settings.thisPlayer.multiplier);
            Components.c.shieldButton.CheckStatusTo_GFX();
            SaveALL();
            nextWord = false;
        }
        /// SAFETY FOR NEGATIVE MULTIPLIERS OR zeros
        if (Components.c.settings.thisPlayer.multiplier < 1)
        {
            Components.c.settings.thisPlayer.multiplier = 1;
        }
        Components.c.gameUIMan.UpdateMultiplier_UI(Components.c.settings.thisPlayer.multiplier);
    }

    private bool checkingWords = false;
    public void SaveALL()
    {
        Components.c.settings.SavePlayerdDataToFile();
    }
    public bool check;
    private string speakNext = "";
    public IEnumerator Wait_and_Speak(string speech)
    {
        if(checkingWords)
        {
            yield return new WaitForSeconds(.1f);
            speakNext = speech;
            TextToSpeech.instance.CheckSpeak();
        }
        else
        {
            yield return new WaitForSeconds(.6f);
            speakNext = speech;
            TextToSpeech.instance.CheckSpeak();
        }
    }
    private bool nextWord = false;
    private IEnumerator newWordDelayForButton()
    {
        yield return new WaitForSeconds(1.35f);
        changeButtonBooleans();
        if(Components.c.settings.thisPlayer.multiplier > 1)
        {
            float sliderLenght = 2 + (MathF.Floor(currentWORD.Length/2));
            Components.c.gameUIMan.StartTimeBonusSlider(sliderLenght * 1.0f);
        }
    }
    private void changeButtonBooleans()
    {
        Components.c.filetotext.canPushButton = true;
        judgingDone_ActivateButton = false;
    }
    public List<string> ExtractFromBody(string body, string start, string end)
    {
        List<string> matched = new List<string>();
        int indexStart = 0;
        int indexEnd = 0;
        bool exit = false;
        while (!exit)
        {
            indexStart = body.IndexOf(start);
            if (indexStart != -1)
            {
                indexEnd = indexStart + body.Substring(indexStart).IndexOf(end);
                matched.Add(body.Substring(indexStart + start.Length, indexEnd - indexStart - start.Length));
                body = body.Substring(indexEnd + end.Length);
            }
            else
            {
                exit = true;
            }
        }
        return matched;
    }
    //// CHECKING STUFFF
    public int checkIndex = 0;
    public List<string> taskWordsList = new List<string>();
    private bool bonk = false;
    public void _check_NewRandomWORD()
    {   
        nextWord = false;
        //activeWord  = Components.c.settings.gameWords[UnityEngine.Random.Range(0, Components.c.settings.gameWords.Count)];
        activeWord  = Components.c.settings.gameWords[Components.c.settings.thisPlayer.dailyTaskStreak +(Components.c.settings.thisPlayer.skillLevel * 10)];
        WORD.text = activeWord.word.ToUpper().ToString();
        inverted_WORD.text = WORD.text;
        //start record ---
        //Components.c.filetotext.StartRecordForCheck();
        Components.c.settings.activeWORD = activeWord.word;
        currentWORD = activeWord.word;
        //StartCoroutine(wait_());
        Components.c.settings.checkedWords_list.Add(activeWord.word);
        Components.c.settings.SaveCheckedWords();
        
        delaytimer = delay;
        check_tts= true;
    }
    private float delay = 0.5f;
    private bool check_tts = false;
    
    private void Update() 
    {
        if(checkingWords)
        {
            if(check_tts)
            {
                delaytimer -= Time.deltaTime;
                if(delaytimer <= 0)
                {
                    if(!recording)
                    {
                        TextToSpeech.instance.CheckSpeak();
                        check_tts = false; 
                    }
                    else
                    {
                        TextToSpeech.instance.CheckSpeak();
                        check_tts = false;
                    }
                }
            }
            if(backup)
            {
                backuptimer -= Time.deltaTime;
                if(backuptimer <= 0)
                {
                    StartCoroutine(wait_());
                    backup = false;
                    //backuptimer = 10;
                    //// blablabla
                }
            }
        }
    }

    private float backuptimer = 10;
    private bool backup = true;
    private bool recording = false;
    private void onReadyToSpeakCallback(string readyToSpeak)
    {
        if(checkingWords)
        {
            if (readyToSpeak == "True" && recording)
            {
                Components.c.filetotext._DoTheClip();
                check_tts = false;
                delaytimer = delay;
                //backup = true;
                //backuptimer = 10;
                recording = false;
                return;
            }
            if (readyToSpeak == "True" && !recording)
            {
                Components.c.filetotext.StartRecordForCheck();
                TextToSpeech.instance.StartSpeak(activeWord.word.ToString());
                delaytimer = delay * 4;
                check_tts = true;
                recording = true;
                return;
            }
            if (readyToSpeak == "False")
            {
                delaytimer = delay;
                check_tts = true;
            }
            return;
        }
        ///normal sequence
        if (readyToSpeak == "True")
        {
            TextToSpeech.instance.StartSpeak(speakNext.ToLower());
            if(judgingDone_ActivateButton && Components.c.filetotext.canPushButton == false)
            {
                StartCoroutine(newWordDelayForButton());
            }
            if(nextWord)
            {
                NewRandomWORD();
                judgingDone_ActivateButton = true;
            }
        }
        if (readyToSpeak == "False")
        {
            StartCoroutine(Wait_and_Speak(speakNext));
        }
    }

        private bool waiting = false;
    public IEnumerator wait_()
    {   
        yield return new WaitForSeconds(.25f);
        //_check_NewRandomWORD();
        //saveSuffledWords();

    }
    private static System.Random rng = new System.Random();  

public void Shuffle<T>(List<T> list)  
{  
    int n = list.Count;  
    while (n > 1) {  
        n--;  
        int k = rng.Next(n + 1);  
        T value = list[k];  
        list[k] = list[n];  
        list[n] = value;  
    }

}
// IList<T> ToIList<T>(List<T> t) {  
//     return t;  
// } 
    [System.Serializable]
    public class WrappingClass
    {
        public List<WordClass> Allwords;
    }
    public void saveSuffledWords()
    {
        for (int i = 0; i < 26; i++)
        {
           var wordsIlist = Components.c.settings.gameWords;
           Shuffle(wordsIlist);
            WrappingClass allwordsClass = new WrappingClass(); 
            allwordsClass.Allwords = wordsIlist;
            //save it locally
            File.WriteAllText(Application.persistentDataPath +"/"+ Components.c.settings.locale +"_WordsJson.json", JsonUtility.ToJson(allwordsClass)); 
            Debug.Log(Components.c.settings.locale + "done suffled");
                Components.c.settings.selection++;
                Components.c.settings.ExecuteLocaleChange();
        }
    }

        public void _SCORING(string results)
        {
            Components.c.filetotext.canPushButton = false;
            Debug.Log("-------------------------------------------------------");
            List<string> results_strings = ExtractFromBody(results, "substring","phoneSequence");
            Debug.Log(results_strings.Count);
            float score = 0;
            string all = "";
            for (int i = 0; i < results.Length; i++)
            {
                all += results[i];
            }
            List<string> chanches = ExtractFromBody(all, "substring=",",");
            bool match = false;

            results = "";
            for (int i = 0; i < chanches.Count; i++)
            {
                results += "\n" + chanches[i].ToString();
                results += " " + i + " / " + chanches.Count;
                string toCHECK = System.Text.RegularExpressions.Regex.Unescape(chanches[i].ToLower());
                    if(toCHECK.ToLower().Contains(currentWORD.ToLower()))
                    {
                        if(i == 0)
                        {
                            score = 100;
                            Debug.Log(chanches[i].ToUpper());
                            match = true;
                            break;
                            
                        }else
                        {
                            score = (100 / i);
                            Debug.Log(chanches[i].ToUpper());
                            match = true;
                            break;
                        }
                    }
            }
            Components.c.sampleSpeechToText.resultListText.text = results;
            if(match == false)
            {
                score = 0;
            }
            Debug.Log("score ; " + score + " / " + chanches.Count );
            Debug.Log("score = " + score + "%");
            results_strings.Clear();

            if(score > 0)
            {
                Components.c.fireStore_Manager.SanityCheck_Upload_WordData_passed(currentWORD, Components.c.settings.thisPlayer.playerLocale, score);
                Resources.UnloadUnusedAssets();
                Components.c.settings.thisPlayer.dailyTaskStreak++;
            }
            if(score == 0)
            {
                Components.c.fireStore_Manager.SanityCheck_Upload_WordData_rejected(currentWORD, Components.c.settings.thisPlayer.playerLocale);
                Resources.UnloadUnusedAssets();
                Components.c.settings.thisPlayer.dailyTaskStreak++;
            }

            if(Components.c.settings.thisPlayer.dailyTaskStreak < 10)
            {
                Resources.UnloadUnusedAssets();
                Debug.Log(checkIndex.ToString() +  "   WORDS CHECKED!!!!");
                StartCoroutine(wait_());

            }else
            {
                Resources.UnloadUnusedAssets();
                //next locale -- reset counter --- 
                if(Components.c.settings.selection < 24)
                {
                    Components.c.settings.selection++;
                }else
                {
                    Components.c.settings.selection = 0;
                    Components.c.settings.thisPlayer.skillLevel++;
                }
                Components.c.settings.ExecuteLocaleChange();
                Components.c.settings.thisPlayer.dailyTaskStreak = 0;
                Components.c.fireStore_Manager.sstat.passed = 0;
                Components.c.fireStore_Manager.sstat.rejected = 0;
                Components.c.fireStore_Manager.sstat.total = 0;
                Components.c.settings.SavePlayerdDataToFile();
                StartCoroutine(wait_());
            }
            Components.c.settings.SavePlayerdDataToFile();

        }


}