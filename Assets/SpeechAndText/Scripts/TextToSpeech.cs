using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System;

namespace TextSpeech
{
    public class TextToSpeech : MonoBehaviour
    {
        public static TextToSpeech instance;
        public void Init()
        {
            instance = this;
            Components.c.textToSpeech = instance;
        }

        public Action onStartCallBack;
        public Action onDoneCallback;
        public Action<string> onSpeakRangeCallback;
        public Action<string> onReadyToSpeakCallback;
        [System.NonSerialized]
        public bool isSpeaking;        
        [Range(0.1f, 2)]
        public float pitch;// = .3f; //[0.5 - 2] Default 1
        [Range(0.1f, 2)]
        public float rate;// = 1f; //[min - max] android:[0.5 - 2] iOS:[0 - 1]
        public void Setting(string language, float _pitch, float _rate)
        {
            pitch = _pitch;
            rate = _rate;
#if UNITY_EDITOR
#elif UNITY_IPHONE
        _TAG_SettingSpeak(language, pitch, rate / 2);

#endif
        }
        public void StartSpeak(string message)
        {

#if UNITY_EDITOR
#elif UNITY_IPHONE
        _TAG_StartSpeak(message);
#endif
        }
        public void StopSpeak()
        {
#if UNITY_EDITOR
#elif UNITY_IPHONE
        _TAG_StopSpeak();
#endif
        }
        public void onSpeechRange(string message)
        {
            if (onSpeakRangeCallback != null && message != null)
            {
                onSpeakRangeCallback(message);
            }
        }
        public void onStart(string message)
        {
            if (onStartCallBack != null)
            {
                onStartCallBack();
            }
        }

        public void onDone(string message)
        {
            if(message == "False")
            {
                isSpeaking = false;
            }
            if(message == "True")
            {
                isSpeaking = true;
            }
        }

        public bool Speaks()
        {
            return isSpeaking;
        }

        public void onError(string message)
        {
        }
        public void onMessage(string message)
        {

        }

        /** Denotes the language is available for the language by the locale, but not the country and variant. */
        public const int LANG_AVAILABLE = 0;
        /** Denotes the language data is missing. */
        public const int LANG_MISSING_DATA = -1;
        /** Denotes the language is not supported. */
        public const int LANG_NOT_SUPPORTED = -2;
        public void onSettingResult(string _params)
        {
            int _error = int.Parse(_params);
            string message = "";
            if (_error == LANG_MISSING_DATA || _error == LANG_NOT_SUPPORTED)
            {
                message = "This Language is not supported";
            }
            else
            {
                message = "This Language valid";
            }
            Debug.Log(message);
        }
        //public bool ready = false;
        public void isSpeakingResults(string message)
        {


            if (onReadyToSpeakCallback != null && message != null)
            {

                Debug.Log("from c script --- " + message);
                onReadyToSpeakCallback(message);
            }


        //     if(message == "False")
        //     {
        //         Components.c.gameloop.check = false;
        //         //isSpeaking = false; continue
        //     }
        //     if(message == "True")
        //    {
        //         Components.c.gameloop.check = true;
        //         //wait
        //     }
        }

    public void CheckSpeak()
    {

        Debug.Log("checkSpeak lähti c skriptiin");
#if UNITY_IPHONE
        _TAG_CheckSpeak();
#endif
    }

#if UNITY_IPHONE
        [DllImport("__Internal")]
        private static extern void _TAG_StartSpeak(string message);

        [DllImport("__Internal")]
        private static extern void _TAG_SettingSpeak(string _language, float _pitch, float _rate);

        [DllImport("__Internal")]
        private static extern void _TAG_StopSpeak();

        [DllImport("__Internal")]
        private static extern void _TAG_CheckSpeak();
#endif
    }
}