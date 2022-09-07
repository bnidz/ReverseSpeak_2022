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

    public void Init()
    {
        Setting("en-US");
        Components.c.speechToText.onResultsArrayCallback = onResultsArrayCallback;
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

    public void StopRecording()
    {
#if UNITY_EDITOR
   Components.c.speechToText.StopRecording();
#endif
#if UNITY_IOS
        loading.SetActive(true);
#endif
    }
    public void OnClickSpeak()
    {
        TextToSpeech.instance.StartSpeak(QuessWord.text);
    }
    public void  OnClickStopSpeak()
    {
        TextToSpeech.instance.StopSpeak();
    }
    public void Setting(string code)
    {
        TextToSpeech.instance.Setting(code, pitch, rate);
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