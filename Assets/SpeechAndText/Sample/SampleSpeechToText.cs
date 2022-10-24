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

//    LANG(en, en-us); LANG(es, es-es); LANG(fr, fr-fr); LANG(de, de-de);
//    LANG(ja, ja-jp); LANG(nl, nl-nl); LANG(it, it-it); LANG(pt, pt-br);
//    LANG(da, da-dk); LANG(fi, fi-fi); LANG(nb, nb-no); LANG(sv, sv-se);
//    LANG(ko, ko-kr); LANG(ru, ru-ru); LANG(pl, pl-pl); LANG(tr, tr-tr);
//    LANG(uk, uk-ua); LANG(hr, hr-hr); LANG(cs, cs-cz); LANG(el, el-gr);
//    LANG(he, he-il); LANG(ro, ro-ro); LANG(sk, sk-sk); LANG(th, th-th);
//    LANG(ca, ca-es); LANG(hu, hu-hu); LANG(vi, vi-vn);
//    LANG(zh-Hans, zh-cn); LANG(pt-PT, pt-pt); LANG(id, id); LANG(ms, ms);
//    LANG(zh-Hant, zh-tw); LANG(en-GB, en-gb); LANG(ar, ar);
// Setting("en-US");


        Setting("fr-FR");
        TextToSpeech.instance.Setting("fr-FR", .60f, .75f);
        Components.c.speechToText.onResultsArrayCallback = onResultsArrayCallback;
    }

    public void SetSettings(string locale, float pitch, float rate)
    {
        TextToSpeech.instance.Setting(locale, pitch, rate);
    }

    private bool isSpeaking;

//////////
    public Text debugText;
    public List<string> results_strings = new List<string>();
    private int checkInt;

    private void onResultsArrayCallback(string results)
    {

        if(results == null)
        {

            Components.c.fireStore_Manager.SanityCheck_Upload_WordData_rejected(Components.c.gameloop.currentWORD, Components.c.settings.thisPlayer.playerLocale);
            StartCoroutine(Components.c.gameloop.wait_());

        }
        resultListText.text = "";
        //resultListText.text = results;
        //Components.c.gameloop.SCORING(results);
        Components.c.gameloop._SCORING(results);

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