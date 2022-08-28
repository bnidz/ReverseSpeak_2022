using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TextSpeech;

public class GameLoop : MonoBehaviour
{

    public Text WORD;
    public string currentWORD;
    public WordClass activeWord;

    //maybe change boolean to straight up disable button functionalities later
    public bool RS_buttonEnabled;

    public void Init()
    {
        TextToSpeech.instance.onReadyToSpeakCallback = onReadyToSpeakCallback;
    }
    
    public void NewRandomWORD()
    {
        nextWord = false;
        activeWord  = Components.c.settings.gameWords[UnityEngine.Random.Range(0, Components.c.settings.gameWords.Count)]; //lw.gameWordsList.Count)];
        currentWORD = activeWord.word.ToUpper().ToString();
        WORD.text = currentWORD.ToString();

        Components.c.settings.activeWORD = activeWord.word;

        StartCoroutine(Wait_and_Speak("New Word is: " + currentWORD.ToString()));
        /// ENABLE SPEECH BUTTON FOR SCORIGN
        RS_buttonEnabled = true;
    }


    public void SCORING(string results)
    {

        /// DISABLE SPEECH BUTTON FOR SCORIGN
        RS_buttonEnabled = false;

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

        for (int i = 0; i < chanches.Count; i++)
        {
            if(chanches[i].ToUpper().Contains(Components.c.settings.activeWORD.ToUpper()))
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

        if(match == false)
        {
            score = 0;
        }

        Debug.Log("score ; " + score + " / "+ chanches.Count );
        score *= 100;
        Debug.Log("score = " + score + "%");
        results_strings.Clear();

        // SCORE CURRENT WORD
        if(score > 0)
        {
            // FX - PERFECT
            if(score == 100)
            {
                //TextToSpeech.instance.StartSpeak("PERFECT!".ToString());
                StartCoroutine(Wait_and_Speak("PERFECT!".ToString()));
            }

            // FX - GOOD
            if(score < 100 && score > 50)
            {
                //TextToSpeech.instance.StartSpeak("GOOD!".ToString());
                StartCoroutine(Wait_and_Speak("GOOD!"));
            }

            // FX - ALRIGHT
            if(score <= 50)
            {
                //TextToSpeech.instance.StartSpeak("OK".ToString());
                StartCoroutine(Wait_and_Speak("OK"));
            }
            
            activeWord.timesTried++;
            activeWord.timesQuessed++;
            activeWord.totalScore += score;
            score = 0;
            //StartCoroutine(SpeechCheck());
            //NewRandomWORD();

        nextWord = true;
        }else
        {

            activeWord.timesTried++;
            activeWord.timesQuessed++;
            StartCoroutine(Wait_and_Speak("TOO BAD! TRY AGAIN"));
            //TextToSpeech.instance.StartSpeak("TOO BAD! TRY AGAIN".ToString());
            // FX - *TSSHHHHH* -
            // RETRY TRIE --- 
            // ACTIVATE SKIP -*SHINES IN*- *wrlimp*''~~

            nextWord = false;
        }

    // LATER WORDS - MANAGER 
    // MAKE ROLLING BUTTON OF THE ICON GFX
    // remove numerals from word datas

    }
    public void SkipWord()
    {
        activeWord.timesSkipped++;
        //yield return new WaitUntil(() => nm.httpRequestDone == true);
        //StartCoroutine(nm.Upload_string(JsonUtility.ToJson(activeWord)));
        //yield return new WaitForSeconds(interval);
        //ResetWordClassValues(activeWord);
        StartCoroutine(Wait_and_Speak("Skipping a word! Good Luck"));
        //TextToSpeech.instance.StartSpeak("Skipping a word! Good Luck".ToString());
        //yield return new WaitForSeconds(interval);
        //yield return new WaitUntil(() => TextToSpeech.instance.Speaks() == false);
        //TextToSpeech.instance.StopSpeak();

        //skip = false;
        //StartCoroutine(SpeechCheck());
        NewRandomWORD();
        //yield break;

    }

    public bool check;
    private string speakNext = "";
    public IEnumerator Wait_and_Speak(string speech)
    {
        yield return new WaitForSeconds(.6f);
        speakNext = speech;
        Debug.Log("in wait and speak");
        TextToSpeech.instance.CheckSpeak();
    }

    private bool nextWord = false;
    private void onReadyToSpeakCallback(string readyToSpeak)
    {
        if (readyToSpeak == "True")
        {
            Debug.Log("callback true");
            TextToSpeech.instance.StartSpeak(speakNext);

            if(nextWord)
            {
                NewRandomWORD();
            }
        }
        if (readyToSpeak == "False")
        {
            Debug.Log("callback false");
            StartCoroutine(Wait_and_Speak(speakNext));
        }
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