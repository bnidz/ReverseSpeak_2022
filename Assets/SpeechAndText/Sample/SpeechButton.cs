using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;
using UnityEngine.iOS;

[RequireComponent(typeof(AudioSource))]
public class SpeechButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public SampleSpeechToText sample;
    public GameObject effect;
    public float speedEffect = 1;
    public float scaleEffect = 1.2f;
    float speed;
    float scale = 1;
    private int minFreq;
    private int maxFreq;
    private bool micConnected = false;
    //private SavWav sawwav;
    private AudioSource asource;


    void Start()
    {
        //effect.SetActive(false);
        //speed = speedEffect;

        ////MIC STARTUP STUFF

        ////Check if there is at least one microphone connected
        //if (Microphone.devices.Length <= 0)
        //{
        //    //MIC NOT CONNECTED - BUTTON STATE TO THAT
        //    //Throw a warning message at the console if there isn't
        //    Debug.LogWarning("Microphone not connected!");
        //}
        //else //At least one microphone is present
        //{
        //    //Set 'micConnected' to true
        //    micConnected = true;
        //    //Get the default microphone recording capabilities

        Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);
        if (minFreq == 0 && maxFreq == 0)
        {
            //...meaning 44100 Hz can be used as the recording sampling rate  
            maxFreq = 44100;
        }
        Debug.Log("DEVICE MIN FREQ: " + minFreq);

        //maxFreq = maxFreq;
        //automatic start recording - for now
        //ButtonPress();
        //pholdz.ResetProgress();
        //}
        //if (micConnected == false)
        //{
        //    //InvokeRepeating("Start", 0, 0.5f);
        //    StartCoroutine(_Start());
        //}

    }

    void Update()
    {
        if (effect.activeSelf)
        {
            scale += Time.deltaTime * speed;
            if (scale > scaleEffect)
            {
                speed = -speedEffect;
            }
            if (scale < scaleEffect - 0.1f)
            {
                speed = speedEffect;
            }
            effect.transform.localScale = new Vector3(scale, scale, 1);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //effect.SetActive(true);
        scale = 1;

//        asource.clip = Microphone.Start(null, true, 10, 441000);

        sample.StartRecording();
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        //effect.SetActive(false);
        //string filename = "quess.wav";
        //gameObject.GetComponent<FileToText>().filename = filename;
       // SaveWav.Save(filename, asource.clip);
        //Microphone.End(null);
        sample.StopRecording();

    }
}