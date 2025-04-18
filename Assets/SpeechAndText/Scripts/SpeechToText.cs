﻿using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System;

namespace TextSpeech
{
    public class SpeechToText : MonoBehaviour
    {

        // #region Init
        // static SpeechToText _instance;
        // public static SpeechToText instance
        // {
        //     get
        //     {
        //         if (_instance == null)
        //         {
        //             Init();
        //         }
        //         return _instance;
        //     }
        // }
        // public static void Init()
        // {
        //     if (_instance != null) return;
        //     GameObject obj = new GameObject();
        //     obj.name = "TextToSpeech";
        //     _instance = obj.AddComponent<SpeechToText>();
        // }
        // void Awake()
        // {
        //     _instance = this;
        // }
        // #endregion
        public Action<string> onResultCallback;
        public Action<string> onResultsArrayCallback;

        public void Setting(string _language)
        {
#if UNITY_EDITOR
#elif UNITY_IPHONE
        _TAG_SettingSpeech(_language);
#endif
        }
        public void StartRecording(string _message = "")
        {
#if UNITY_EDITOR
#elif UNITY_IPHONE
        _TAG_startRecording();
#endif
        }
        public void StopRecording()
        {
#if UNITY_EDITOR
#elif UNITY_IPHONE
        _TAG_stopRecording();
#endif
        }
        public void RecognizeFile(string URL)
        {
#if UNITY_EDITOR
#elif UNITY_IPHONE
        _TAG_RecognizeFile(URL);
//#elif UNITY_ANDROID
//        if (isShowPopupAndroid == false)
//        {
//            AndroidJavaClass javaUnityClass = new AndroidJavaClass("com.starseed.speechtotext.Bridge");
//            javaUnityClass.CallStatic("StopRecording");
//        }
#endif
        }

#if UNITY_IPHONE
        [DllImport("__Internal")]
        private static extern void _TAG_startRecording();

        [DllImport("__Internal")]
        private static extern void _TAG_stopRecording();

        [DllImport("__Internal")]
        private static extern void _TAG_RecognizeFile(string URL);

        [DllImport("__Internal")]
        private static extern void _TAG_SettingSpeech(string _language);
#endif

        public void onMessage(string _message)
        {
        }
        public void onErrorMessage(string _message)
        {
            Debug.Log(_message);
        }
        /** Called when recognition results are ready. */
        public void onResults(string _results)
        {
            // if (onResultCallback != null)
            //Components.c.fireStore_Manager.SanityCheck_Upload_WordData_rejected(Components.c.gameloop.currentWORD, Components.c.settings.thisPlayer.playerLocale);
            //Components.c.settings.thisPlayer.dailyTaskStreak++;
            //     onResultCallback(_results);
            //Components.c.gameloop._check_NewRandomWORD();
            Debug.Log("ERRORREOREOROEROEROOEROREOEROEROEOROEROERO ---- --  REC NULLL ");
            Debug.Log("ERRORREOREOROEROEROOEROREOEROEROEOROEROERO ---- --  REC NULLL ");
            Components.c.sfxmanager.PlaySFX("null_rec_fx");
            Components.c.gameUIMan.CircularTexts_ChangeColor_BtoR();
            Debug.Log("TRY AGAIN - TIDYY");
            //play error sound
        }
        private IEnumerator waitAudio(string _results)
        {
            //Components.c.filetotext.PlayReversedReversed();  
            //have it check if reversed has played and then continue with scoring --->           
            while (Components.c.filetotext.isPlayingReversed) yield return null;//new WaitForSeconds (Components.c.filetotext.clip.length);
            print ("end of sound");
            Components.c.gameUIMan.CircularTexts_ChangeColor_BtoR();
            if (onResultsArrayCallback != null)
            {
                onResultsArrayCallback(_results);
            }
        }
        public void onResultsArray(string _results)
        {
            StartCoroutine(waitAudio(_results));
        }
        #region Android STT custom
#if UNITY_ANDROID
        #region Error Code
        /** Network operation timed out. */
        public const int ERROR_NETWORK_TIMEOUT = 1;
        /** Other network related errors. */
        public const int ERROR_NETWORK = 2;
        /** Audio recording error. */
        public const int ERROR_AUDIO = 3;
        /** Server sends error status. */
        public const int ERROR_SERVER = 4;
        /** Other client side errors. */
        public const int ERROR_CLIENT = 5;
        /** No speech input */
        public const int ERROR_SPEECH_TIMEOUT = 6;
        /** No recognition result matched. */
        public const int ERROR_NO_MATCH = 7;
        /** RecognitionService busy. */
        public const int ERROR_RECOGNIZER_BUSY = 8;
        /** Insufficient permissions */
        public const int ERROR_INSUFFICIENT_PERMISSIONS = 9;
        /////////////////////
        String getErrorText(int errorCode)
        {
            String message;
            switch (errorCode)
            {
                case ERROR_AUDIO:
                    message = "Audio recording error";
                    break;
                case ERROR_CLIENT:
                    message = "Client side error";
                    break;
                case ERROR_INSUFFICIENT_PERMISSIONS:
                    message = "Insufficient permissions";
                    break;
                case ERROR_NETWORK:
                    message = "Network error";
                    break;
                case ERROR_NETWORK_TIMEOUT:
                    message = "Network timeout";
                    break;
                case ERROR_NO_MATCH:
                    message = "No match";
                    break;
                case ERROR_RECOGNIZER_BUSY:
                    message = "RecognitionService busy";
                    break;
                case ERROR_SERVER:
                    message = "error from server";
                    break;
                case ERROR_SPEECH_TIMEOUT:
                    message = "No speech input";
                    break;
                default:
                    message = "Didn't understand, please try again.";
                    break;
            }
            return message;
        }
        #endregion
        public bool isShowPopupAndroid = true;
        public Action<string> onReadyForSpeechCallback;
        public Action onEndOfSpeechCallback;
        public Action<float> onRmsChangedCallback;
        public Action onBeginningOfSpeechCallback;
        public Action<string> onErrorCallback;
        public Action<string> onPartialResultsCallback;
        /** Called when the endpointer is ready for the user to start speaking. */
        public void onReadyForSpeech(string _params)
        {
            if (onReadyForSpeechCallback != null)
                onReadyForSpeechCallback(_params);
        }
        /** Called after the user stops speaking. */
        public void onEndOfSpeech(string _paramsNull)
        {
            if (onEndOfSpeechCallback != null)
                onEndOfSpeechCallback();
        }
        /** The sound level in the audio stream has changed. */
        public void onRmsChanged(string _value)
        {
            float _rms = float.Parse(_value);
            if (onRmsChangedCallback != null)
                onRmsChangedCallback(_rms);
        }

        /** The user has started to speak. */
        public void onBeginningOfSpeech(string _paramsNull)
        {
            if (onBeginningOfSpeechCallback != null)
                onBeginningOfSpeechCallback();
        }
        /** A network or recognition error occurred. */
        public void onError(string _value)
        {
            int _error = int.Parse(_value);
            string _message = getErrorText(_error);
            Debug.Log(_message);

            if (onErrorCallback != null)
                onErrorCallback(_message);
        }
        /** Called when partial recognition results are available. */
        public void onPartialResults(string _params)
        {
            if (onPartialResultsCallback != null)
                onPartialResultsCallback(_params);
        }
#endif
        #endregion
    }
}