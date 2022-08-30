using UnityEngine;
using UnityEngine.UI;
using TextSpeech;
using System;
using System.Collections.Generic;

public class SampleSpeechToText : MonoBehaviour
{
    public GameObject loading;
    public InputField inputLocale;
    public InputField inputText;
    public float pitch;
    public float rate;

    public Text txtLocale;
    public Text txtPitch;
    public Text txtRate;

    //Game text to speech
    public Text QuessWord;
    public Text resultListText;
    private NetworkManager nm;
    public bool DebugServerActive = false;

    public List <String> resultVariants;

    void Start()
    {

        nm = FindObjectOfType<NetworkManager>();
        Setting("en-US");
        loading.SetActive(false);

        //SpeechToText.instance.onDoneCallback = Continue();
        //SpeechToText.instance.onResultCallback = OnResultSpeech;
        Components.c.speechToText.onResultsArrayCallback = onResultsArrayCallback;
        //Components.c.textToSpeech.isSpeaking = !isSpeaking;
    }

    private bool isSpeaking;

//////////
    public Text debugText;
    public List<string> results_strings = new List<string>();
    private int checkInt;

    private void onResultsArrayCallback(string results)
    {
        resultListText.text = "";
        //resultListText.text = results;
        Components.c.gameloop.SCORING(results);

    }

    public void ClearResultList()
    {
        resultVariants.Clear();
    }

///////
    public void StartRecording()
    {
#if UNITY_EDITOR
#else
        Components.c.speechToText.StartRecording("Speak any");
#endif
    }

    public void RecognizeFile(string URL)
    {
#if UNITY_EDITOR
#else
        Components.c.speechToText.RecognizeFile(URL);
#endif
    }

// private bool Continue()
// {


// }

/////////
    public void StopRecording()
    {
#if UNITY_EDITOR
//        // OnResultSpeech("Not support in editor.");
// #else
   Components.c.speechToText.StopRecording();
#endif
#if UNITY_IOS
        loading.SetActive(true);
#endif
    }


////////
//     void OnResultSpeech(string _data)
//     {

//         if (DebugServerActive)
//         {
//             StartCoroutine(nm.Upload_string(_data));

//             Debug.Log(_data);
//             return;
//         }else
//         {
//             string[] quess = _data.Split('/');
//             // quessWORD = quess[0].ToUpper().ToString();
//             inputText.text = quess[0].ToUpper().ToString();
//         }
// #if UNITY_IOS
//         loading.SetActive(false);
// #endif
//     }

/////////
    public void OnClickSpeak()
    {
        //Components.c.textToSpeech.StartSpeak(inputText.text);
        //for the word to quess
        Components.c.textToSpeech.StartSpeak(QuessWord.text);
    }
    public void  OnClickStopSpeak()
    {
        Components.c.textToSpeech.StopSpeak();
    }
    public void Setting(string code)
    {
        Components.c.textToSpeech.Setting(code, pitch, rate);
        Components.c.speechToText.Setting(code);
        txtLocale.text = "Locale: " + code;
        txtPitch.text = "Pitch: " + pitch;
        txtRate.text = "Rate: " + rate;
    }
    public void OnClickApply()
    {
        Setting(inputLocale.text);
    }
}