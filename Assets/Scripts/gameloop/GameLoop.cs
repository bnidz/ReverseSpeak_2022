using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TextSpeech;
using Apple.GameKit;
using System;
using TMPro;
using System.Text;

public class GameLoop : MonoBehaviour
{
    public TextMeshProUGUI WORD;
    public TextMeshProUGUI inverted_WORD;
    public string currentWORD;
    public WordClass activeWord;

    public void Init()
    {
        TextToSpeech.instance.onReadyToSpeakCallback = onReadyToSpeakCallback;
        nextWord = false;
        Components.c.gameUIMan.UpdateMultiplier_UI(Components.c.settings.thisPlayer.multiplier);
        nextWord = true;
        Components.c.gameUIMan.startRotTexts = true;
    }
    private void DebugValuesToPlayer()
    {
        Components.c.settings.thisPlayer.playerMaxMultiplier = 5;
        Debug.Log("warning debug values in useeeee!!!!!!!");
    }
    public void NewRandomWORD()
    {
        nextWord = false;
        activeWord  = Components.c.settings.gameWords[UnityEngine.Random.Range(0, Components.c.settings.gameWords.Count)];
        string[] task_words = activeWord.word.ToLower().Split(' ');
        currentWORD = task_words[UnityEngine.Random.Range(0, task_words.Length)]; //lw.gameWordsList.Count)];
        WORD.text = currentWORD.ToUpper().ToString();
        inverted_WORD.text = WORD.text;
        Components.c.gameUIMan.SetCircularTexts(currentWORD);
        Components.c.settings.activeWORD = activeWord.word;
        
        StartCoroutine(Wait_and_Speak(Components.c.localisedStrings.game_newWord + currentWORD.ToString()));
        /// ENABLE SPEECH BUTTON FOR SCORIGN
        Components.c.fireStore_Manager.Get_Rank();
    }
    public void LOAD_LAST_LOCALE_WORD()
    {
        nextWord = false;
        activeWord  = Components.c.settings.lastLocaleWord();
        // Components.c.settings.gameWords[UnityEngine.Random.Range(0, Components.c.settings.gameWords.Count)]; //lw.gameWordsList.Count)];
        currentWORD = activeWord.word.ToUpper().ToString();
        WORD.text = currentWORD.ToString();
        inverted_WORD.text = WORD.text;
        Components.c.gameUIMan.SetCircularTexts(currentWORD);
        Components.c.settings.activeWORD = activeWord.word;
        StartCoroutine(Wait_and_Speak(Components.c.localisedStrings.game_newWord + currentWORD.ToString()));
        /// ENABLE SPEECH BUTTON FOR SCORIGN
    }
    
    public int checkIndex = 0;
    public void _check_NewRandomWORD()
    {
        nextWord = false;
        activeWord  = Components.c.settings.gameWords[Components.c.settings.thisPlayer.totalScore];
        currentWORD = activeWord.word.ToUpper().ToString();
        WORD.text = currentWORD.ToString();
        inverted_WORD.text = WORD.text;
        //start record ---
        Components.c.filetotext.StartRecordForCheck();
        //Components.c.gameUIMan.SetCircularTexts(currentWORD);
        Components.c.settings.activeWORD = activeWord.word;
        StartCoroutine(wait_());
        nakki = 4;
    }
    private bool waiting = false;
    public IEnumerator wait_()
    {   
        waiting = true;
        yield return new WaitForSeconds(.3f);
        StartCoroutine(Wait_and_Speak( currentWORD.ToString()));
        //START TIMEBONUS SLIDER IF MULTIPLIER IN ACTION
        waiting = false;
    }
    public void CheckWordsAutom()
    {
        // checkIndex = DadabaseManager
        for (int i = 0; i < 500; i++)
        {
            
        }
        Components.c.settings.thisPlayer.totalScore = checkIndex;
    }
    float timertocheck = 10;
    float nakki = 10000;
    void Update()
    {
        nakki -= Time.deltaTime;
        if(nakki <= 0)
        {
            Components.c.dadabaseManager.Rejected_WordData(activeWord);
            Components.c.settings.thisPlayer.totalScore++;
            _check_NewRandomWORD();
            nakki = 4;
        }
    }
        public void _SCORING(string results)
        {
            Components.c.filetotext.canPushButton = false;
            //Debug.Log(results);
            Debug.Log("-------------------------------------------------------");
            List<string> results_strings = ExtractFromBody(results, "substring","phoneSequence");
            Debug.Log(results_strings.Count);
            //SCORING
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
                results += "\n" + chanches[i].ToString();
                results += " " + i + " / " + chanches.Count;
                string toCHECK = System.Text.RegularExpressions.Regex.Unescape(chanches[i].ToLower());

                if(toCHECK.ToLower().Contains(currentWORD.ToLower()))
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
            Components.c.sampleSpeechToText.resultListText.text = results;
            if(match == false)
            {
                score = 0;
            }
            Debug.Log("score ; " + score + " / " + chanches.Count );
            score *= 100;
            Debug.Log("score = " + score + "%");
            results_strings.Clear();

            // SCORE CURRENT WORD
            if(score > 0)
            {
                //activeWord
                // upload passed word to DB
                Components.c.dadabaseManager.Passed_WordData(activeWord);
            }else
            {
                Components.c.dadabaseManager.Rejected_WordData(activeWord);
                //upload not passed word to db
            }
            Components.c.settings.thisPlayer.totalScore++;
            Components.c.settings.SavePlayerdDataToFile();

            if(Components.c.settings.thisPlayer.totalScore < 3000)
            {
                _check_NewRandomWORD();
                Debug.Log(checkIndex.ToString() +  "   WORDS CHECKED!!!!");
            }

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
                StartCoroutine(Wait_and_Speak("Skipping a word! Good Luck"));
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
            // copy the string as UTF-8 bytes.
            byte[] utf8Bytes = new byte[utf8String.Length];
            for (int i=0;i<utf8String.Length;++i) {
                //Debug.Assert( 0 <= utf8String[i] && utf8String[i] <= 255, "the char must be in byte's range");
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
            activeWord.total_score += (score * Components.c.settings.thisPlayer.multiplier);
            StartCoroutine(_wait_Update_WordData(activeWord));
            Components.c.dadabaseManager.waiting_ = false;
            //Components.c.dadabaseManager._ = false;;
            Components.c.settings.thisPlayer.totalScore += Convert.ToInt32((score * Components.c.settings.thisPlayer.multiplier));
            Components.c.settings.thisPlayer.timesQuessed++;
            Components.c.settings.thisPlayer.totalTries++;

            Components.c.settings.sessionScore += Convert.ToInt32((score * Components.c.settings.thisPlayer.multiplier));
            Components.c.settings.lastScore = Convert.ToInt32((score * Components.c.settings.thisPlayer.multiplier));
            Components.c.settings.DailyTaskWordComplete();
            Components.c.settings.CheckStreak();

            Components.c.settings.localeScore += Convert.ToInt32((score * Components.c.settings.thisPlayer.multiplier));
            Components.c.gameUIMan.SpawnWordsScoreText(Convert.ToInt32((score * Components.c.settings.thisPlayer.multiplier)));
            Components.c.settings.SavePlayerdDataToFile();
            Components.c.shieldButton.CheckStatusTo_GFX();
            score = 0;
            Components.c.gameUIMan.UpdateRankText();
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
            StartCoroutine(_wait_Update_WordData(activeWord));
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
    public void SaveALL()
    {
        Components.c.settings.SavePlayerdDataToFile();
    }
    public bool check;
    private string speakNext = "";
    public IEnumerator Wait_and_Speak(string speech)
    {
        yield return new WaitForSeconds(.6f);
        speakNext = speech;
        TextToSpeech.instance.CheckSpeak();
    }
    private bool nextWord = false;
    private void onReadyToSpeakCallback(string readyToSpeak)
    {
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
}