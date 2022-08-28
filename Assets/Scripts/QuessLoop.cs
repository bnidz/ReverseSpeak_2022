using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TextSpeech;

public class QuessLoop : MonoBehaviour
{

    public Text WORD;
    public string currentWORD;
    public WordClass activeWord;
    private FileReader fr;
    private FileToText ftt;
    private LoadWords lw;
    //current word obu

    //networkmanager
    private NetworkManager nm;


    public void Init()
    {
        lw = FindObjectOfType<LoadWords>();
        fr = FindObjectOfType<FileReader>();
        ftt = FindObjectOfType<FileToText>();
        nm = FindObjectOfType<NetworkManager>();
        
        StartCoroutine(GetRandomWord());
    }

    // Start is called before the first frame update
    IEnumerator _Start()
    {
        yield return new WaitUntil(() => lw.loadWords == true);
        yield return new WaitForSeconds(1f);
        //TextToSpeech.instance.onDoneCallback = SetSpeechDone;
    }

    public IEnumerator GetRandomWord()
    {
        
        //have current wordClasses somewhere neatly
        //lets say its daily stack / current wordstack 
        // - get random word out of it

        activeWord  = Components.c.settings.gameWords[UnityEngine.Random.Range(0, Components.c.settings.gameWords.Count)]; //lw.gameWordsList.Count)];
        currentWORD = activeWord.word.ToUpper().ToString();
        WORD.text = currentWORD.ToString();

        Components.c.settings.activeWORD = activeWord.word;

        yield return new WaitForSeconds(interval);
        TextToSpeech.instance.StartSpeak("New Word is: " + currentWORD.ToString());
        yield return new WaitUntil(() => TextToSpeech.instance.Speaks() == false);
        yield return new WaitForSeconds(interval);
        TextToSpeech.instance.StopSpeak();

    }

    float timer = 1f;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(GetRandomWord());
        }

    }

    public bool _speechDone = false;
    public void SetSpeechDone()
    {
        _speechDone = true;
      //  TextToSpeech.instance.sDone = false;
    }

    public float interval = .25f;
    /// skip stuff can be implemented in a the record button functionality later
    public bool skip = false;
    public void setSkip()
    {
        if(skip)
        {
            skip = false;
        }
        if(!skip)
        {
            StartCoroutine(GetRandomWord());
            skip = true;
        }
    }

    public void ResetWordClassValues(WordClass activeWord)
    {

        //activeWord.word = fr.eng_words[i].ToString().ToUpper();
        activeWord.timesTried = 0;
        activeWord.timesSkipped = 0;
        activeWord.timesQuessed = 0;
        activeWord.totalScore = 0;
        activeWord.avgScore = 0;

    }


    private string wholeString;
    //  private string 

    public IEnumerator CompareResult(string quessWORD)
    {

        wholeString = quessWORD.ToUpper();

        string[] quess = quessWORD.Split('/');
        quessWORD = quess[0].ToString();

        if (quessWORD.ToUpper() == currentWORD.ToUpper() && quessWORD.ToUpper() != "skip".ToUpper())
        {
            activeWord.timesQuessed++;
            activeWord.totalScore += 1.5f;
            //upload new data to server jSon & reset local word data to zero

            yield return new WaitUntil(() => nm.httpRequestDone == true);
            StartCoroutine(nm.Upload_string(JsonUtility.ToJson(activeWord)));
            ResetWordClassValues(activeWord);

            if (!skip)
            {
                TextToSpeech.instance.StartSpeak("Congratulations!".ToString());
                yield return new WaitForSeconds(interval);
                yield return new WaitUntil(() => TextToSpeech.instance.Speaks() == false);
                TextToSpeech.instance.StopSpeak();
            }

            //if(!skip)
            //{
            //    yield return new WaitForSeconds(interval);
            //    TextToSpeech.instance.StartSpeak("Right answer is".ToString());
            //    yield return new WaitForSeconds(interval);
            //    yield return new WaitUntil(() => TextToSpeech.instance.Speaks() == false);
            //    TextToSpeech.instance.StopSpeak();
            //}
            //if (!skip)
            //{
            //    yield return new WaitForSeconds(interval);
            //    TextToSpeech.instance.StartSpeak(currentWORD);
            //    yield return new WaitForSeconds(interval);
            //    yield return new WaitUntil(() => TextToSpeech.instance.Speaks() == false);
            //    TextToSpeech.instance.StopSpeak();
            //}
            //if (!skip)
            //{             
            //    yield return new WaitForSeconds(interval);
            //    TextToSpeech.instance.StartSpeak("You got a point!".ToString());
            //    yield return new WaitForSeconds(interval);
            //    yield return new WaitUntil(() => TextToSpeech.instance.Speaks() == false);
            //    TextToSpeech.instance.StopSpeak();
            //}

            skip = false;
            StartCoroutine(GetRandomWord());
            yield break;

        }

        pointCategory = 0;
        comparing = true;
        StartCoroutine(CompareSubstrings(wholeString));
        yield return new WaitUntil(() => comparing == false);

        if(pointCategory == 1)
        {
            activeWord.timesQuessed++;
            activeWord.totalScore += 1.4f;
            //upload new data to server jSon & reset local word data to zero

            yield return new WaitUntil(() => nm.httpRequestDone == true);
            StartCoroutine(nm.Upload_string(JsonUtility.ToJson(activeWord)));
            yield return new WaitForSeconds(interval);
            ResetWordClassValues(activeWord);

            TextToSpeech.instance.StartSpeak("WOHOO POINT CATEGORY 1!".ToString());
            yield return new WaitForSeconds(interval);
            yield return new WaitUntil(() => TextToSpeech.instance.Speaks() == false);
            TextToSpeech.instance.StopSpeak();

            StartCoroutine(GetRandomWord());
            yield break;

        }
        if (pointCategory == 2)
        {
            activeWord.timesQuessed++;
            activeWord.totalScore += 1.2f;

            //upload new data to server jSon & reset local word data to zero

            yield return new WaitUntil(() => nm.httpRequestDone == true);
            StartCoroutine(nm.Upload_string(JsonUtility.ToJson(activeWord)));
            yield return new WaitForSeconds(interval);
            ResetWordClassValues(activeWord);

            TextToSpeech.instance.StartSpeak("OUJEA POINT CATEGORY 2!".ToString());
            yield return new WaitForSeconds(interval);
            yield return new WaitUntil(() => TextToSpeech.instance.Speaks() == false);
            TextToSpeech.instance.StopSpeak();

            StartCoroutine(GetRandomWord());
            yield break;
        }

        if (pointCategory == 3)
        {
            activeWord.timesQuessed++;
            activeWord.totalScore += 1f;

            //upload new data to server jSon & reset local word data to zero
            yield return new WaitUntil(() => nm.httpRequestDone == true);
            StartCoroutine(nm.Upload_string(JsonUtility.ToJson(activeWord)));
            yield return new WaitForSeconds(interval);
            ResetWordClassValues(activeWord);

            TextToSpeech.instance.StartSpeak("QUITE NICE POINT CATEGORY 3!".ToString());
            yield return new WaitForSeconds(interval);
            yield return new WaitUntil(() => TextToSpeech.instance.Speaks() == false);
            TextToSpeech.instance.StopSpeak();

            StartCoroutine(GetRandomWord());
            yield break;
        }

        if (quessWORD.ToUpper() != currentWORD.ToUpper())// && quessWORD.ToUpper()) // != "skip".ToUpper())
        {

            activeWord.timesTried++;
            //upload new data to server jSon & reset local word data to zero
            yield return new WaitUntil(() => nm.httpRequestDone == true);
            StartCoroutine(nm.Upload_string(JsonUtility.ToJson(activeWord)));
            yield return new WaitForSeconds(interval);
            ResetWordClassValues(activeWord);

            if (!skip)
            {
                TextToSpeech.instance.StartSpeak("WRONG! This was your quess!".ToString());
                yield return new WaitForSeconds(interval);
                yield return new WaitUntil(() => TextToSpeech.instance.Speaks() == false);
                TextToSpeech.instance.StopSpeak();
            }
            if (!skip)
            {
                yield return new WaitForSeconds(interval);
                //TextToSpeech.instance.StartSpeak(quessWORD);
                //recorded audio played back backwards
                ftt.PlayReversedReversed();
                //yield return new WaitUntil(() => ftt.asource.isPlaying == false);
                float waitforclip = FindObjectOfType<FileToText>().clip.length;
                yield return new WaitForSeconds(waitforclip);
                TextToSpeech.instance.StopSpeak();

            }
            if (!skip)
            {
                yield return new WaitForSeconds(interval);
                TextToSpeech.instance.StartSpeak("Right answer is".ToString());
                yield return new WaitForSeconds(interval);
                yield return new WaitUntil(() => TextToSpeech.instance.Speaks() == false);
                TextToSpeech.instance.StopSpeak();
            }
            if (!skip)
            {
                yield return new WaitForSeconds(interval);
                TextToSpeech.instance.StartSpeak(currentWORD);
                yield return new WaitForSeconds(interval);
                yield return new WaitUntil(() => TextToSpeech.instance.Speaks() == false);
                TextToSpeech.instance.StopSpeak();
            }

            //if (!skip)
            //{
            //    yield return new WaitForSeconds(interval);
            //    TextToSpeech.instance.StartSpeak("NO POINTS!".ToString());
            //    yield return new WaitForSeconds(interval);
            //    yield return new WaitUntil(() => TextToSpeech.instance.Speaks() == false);
            //    TextToSpeech.instance.StopSpeak();
            //}
            //if (!skip)
            //{
            //    yield return new WaitForSeconds(interval);
            //    TextToSpeech.instance.StartSpeak("Try again or say SKIP to get a new word".ToString());
            //    yield return new WaitForSeconds(interval);
            //    yield return new WaitUntil(() => TextToSpeech.instance.Speaks() == false);
            //    TextToSpeech.instance.StopSpeak();
            //}

            skip = false;
            yield break;

        }

        //if (quessWORD.ToUpper() == "skip".ToUpper())
        //{
        //    activeWord.timesSkipped++;
        //    //upload new data to server jSon & reset local word data to zero
        //    yield return new WaitUntil(() => nm.httpRequestDone == true);
        //    StartCoroutine(nm.Upload_string(JsonUtility.ToJson(activeWord)));
        //    ResetWordClassValues(activeWord);
        //    if (!skip)
        //    {
        //        TextToSpeech.instance.StartSpeak("Skipping a word! Good Luck".ToString());
        //        yield return new WaitForSeconds(interval);
        //        yield return new WaitUntil(() => TextToSpeech.instance.Speaks() == false);
        //        TextToSpeech.instance.StopSpeak();
        //    }
        //    skip = false;
        //    StartCoroutine(GetRandomWord());
        //    yield break;
        //}

    }

    public IEnumerator SkipWord()
    {
        activeWord.timesSkipped++;
        yield return new WaitUntil(() => nm.httpRequestDone == true);
        StartCoroutine(nm.Upload_string(JsonUtility.ToJson(activeWord)));
        yield return new WaitForSeconds(interval);
        ResetWordClassValues(activeWord);

        TextToSpeech.instance.StartSpeak("Skipping a word! Good Luck".ToString());
        yield return new WaitForSeconds(interval);
        yield return new WaitUntil(() => TextToSpeech.instance.Speaks() == false);
        TextToSpeech.instance.StopSpeak();

        skip = false;
        StartCoroutine(GetRandomWord());
        yield break;
    }

    //public string allDATA;
    public IEnumerator SendDebugString(string debug)
    {

        //allDATA = debug;
        yield return new WaitForSeconds(.01f);

        //parse here too
        //yield return new WaitUntil(() => nm.httpRequestDone == true);
        //StartCoroutine(CompareSubstrings(debug));
        // parse in python to get active plus substrings to score 
        //string activePlusDebug =  activeWord + "|" + debug;
        //yield return new WaitUntil(() => nm.httpRequestDone == true);
        //StartCoroutine(nm.Upload_string(debug));

    }

    private string formattedStringKey, substringKey, alternativeSubstringKey;
    public bool comparing = false;
    public int pointCategory = 0;

    public IEnumerator CompareSubstrings(string alldata)
    {

        //alldata = alldata.ToUpper();
        comparing = true;
        pointCategory = 0;
        formattedStringKey = "formattedString".ToUpper();
        substringKey = "substring".ToUpper();
        alternativeSubstringKey = "alternativeSubstrings".ToUpper();

        bool formattedStrinCheck = alldata.Contains(formattedStringKey);
        if (formattedStrinCheck)
        {
            int index = alldata.IndexOf(formattedStringKey);
            if (index >= 0)
            {
                for (int i = 0; i < AllIndexesOf(alldata, formattedStringKey, true).Length; i++)
                {
                  if(alldata.ToUpper().Substring(AllIndexesOf(alldata, formattedStringKey, true)[i] + formattedStringKey.Length, currentWORD.Length + 8).Contains(currentWORD.ToUpper()))
                    {
                        //BAZINGA
                        Debug.Log("FORMATTED STRING MATCHES THE WORD: BAZINGA!  point category 1");
                        pointCategory = 1;
                        comparing = false;
                        yield break;
                    }
                }
         
                //    string check = alldata.Substring(index + formattedStringKey.Length, currentWORD.Length +8);
                //    Debug.Log("FormattedString Check: " + check + " wonder if matches the current word");
                //    //most points of the extra check category
                //    if (check.ToUpper().Contains(currentWORD.ToUpper()))
                //    {
                //        //BAZINGA
                //        Debug.Log("FORMATTED STRING MATCHES THE WORD: BAZINGA!  point category 1");
                //        pointCategory = 1;
                //        comparing = false;
                //        yield break;
                //    }
             }
        }

        bool subStringCheck = alldata.Contains(substringKey);
        if(subStringCheck)
        {
            int index = alldata.IndexOf(substringKey);
            if(index >= 0)
            {

                for (int i = 0; i < AllIndexesOf(alldata, formattedStringKey, true).Length; i++)
                {
                    if (alldata.ToUpper().Substring(AllIndexesOf(alldata, substringKey, true)[i] + substringKey.Length, currentWORD.Length + 8).Contains(currentWORD.ToUpper()))
                    {
                        //BAZINGA
                        Debug.Log("FORMATTED STRING MATCHES THE WORD: BAZINGA!  point category 1");
                        pointCategory = 2;
                        comparing = false;
                        yield break;
                    }
                }

                //string check = alldata.Substring(index + substringKey.Length, currentWORD.Length +8);
                //Debug.Log("Substring Check: " + check + " wonder if matches the current word");
                //if (check.ToUpper().Contains(currentWORD.ToUpper()))
                //{
                //    //BAZINGA
                //    Debug.Log("SUBSTRING MATCHES THE WORD: BAZINGA! point category 2");
                //    pointCategory = 2;
                //    comparing = false;
                //    yield break;
                //}
            }
        }

        bool alternativeSubKeyCheck = alldata.Contains(alternativeSubstringKey);
        if(alternativeSubKeyCheck)
        {
            int index = alldata.IndexOf(alternativeSubstringKey);
            if(index >= 0)
            {
                for (int i = 0; i < AllIndexesOf(alldata, formattedStringKey, true).Length; i++)
                {
                    if (alldata.ToUpper().Substring(AllIndexesOf(alldata, alternativeSubstringKey, true)[i] + alternativeSubstringKey.Length).Contains(currentWORD.ToUpper()))
                    {
                        //BAZINGA
                        Debug.Log("FORMATTED STRING MATCHES THE WORD: BAZINGA!  point category 1");
                        pointCategory = 3;
                        comparing = false;
                        yield break;
                    }
                }

                //string check = alldata.Substring(index);
                //if (check.ToUpper().Contains(currentWORD.ToUpper()))
                //{
                //    Debug.Log("ALT SUBSTRING MATCHES THE WORD: BAZINGA! point category 3");
                //    //end of checks
                //    pointCategory = 3;
                //    comparing = false;
                //    yield break;
                //}
            }
        }

        pointCategory = 0;
        comparing = false;
    }

    public static int[] AllIndexesOf(string str, string substr, bool ignoreCase = false)
    {
        if (string.IsNullOrWhiteSpace(str) ||
            string.IsNullOrWhiteSpace(substr))
        {
            throw new ArgumentException("String or substring is not specified.");
        }

        var indexes = new List<int>();
        int index = 0;

        while ((index = str.IndexOf(substr, index, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)) != -1)
        {
            indexes.Add(index++);
        }

        Debug.Log("INDEXES FOUND: " + indexes.Count);
        return indexes.ToArray();

    }

}