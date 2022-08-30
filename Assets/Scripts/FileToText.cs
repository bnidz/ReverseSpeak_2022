using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;
using System.IO;
using UnityEngine.iOS;

[RequireComponent(typeof(AudioSource))]
public class FileToText : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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
    public AudioSource asource;
    private QuessLoop qL;

    public void Init()
    {
        asource = gameObject.GetComponent<AudioSource>();
        qL = FindObjectOfType<QuessLoop>();
        StartCoroutine(_Start());
    }

    IEnumerator _Start()
    {

        yield return new WaitForSeconds(.2f);
        yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
        if (Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            Debug.Log("Microphone found");
        }
        else
        {
            Debug.Log("Microphone not found");
        }
        __Start();
    }

    void __Start()
    {

        effect.SetActive(false);
        speed = speedEffect;

        //MIC STARTUP STUFF
        //Check if there is at least one microphone connected
        if (Microphone.devices.Length <= 0)
        {
            Debug.LogWarning("Microphone not connected!");
        }
        else //At least one microphone is present
        {
            micConnected = true;
            //Get the default microphone recording capabilities

            Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);
            if (minFreq == 0 && maxFreq == 0)
            {
                //...meaning 44100 Hz can be used as the recording sampling rate  
                maxFreq = 44100;
            }
        }

        if (micConnected == false)
        {
            StartCoroutine(_Start());
        }
        effect.SetActive(false);
        speed = speedEffect;

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

    public bool audioSourceIsPlaying = false;
    public string filename;

    public void OnPointerDown(PointerEventData eventData)
    {
        sample.ClearResultList();

        //Figure out if this still eats memory.. 
        asource.clip = Microphone.Start(null, true, 5, 441000);

        effect.SetActive(true);
        scale = 1;
    }


    public AudioClip clip;
    public void OnPointerUp(PointerEventData eventData)
    {
        if(Microphone.GetPosition(null) > 441000)
        {

            effect.SetActive(false);
            effect.SetActive(false);
            string filename = "quess.wav";
            gameObject.GetComponent<FileToText>().filename = filename;

            float[] samples = new float[Microphone.GetPosition(null)];
            asource.clip.GetData(samples, 0);

            clip = AudioClip.Create("tagClip", samples.Length, 1, 441000, false);
            if (isReversed)
            {
                Array.Reverse(samples);
            }
            clip.SetData(samples, 0);
            SaveWav.Save(filename, clip);
            Microphone.End(null);
            string URL = Application.persistentDataPath + "/" + filename.ToString();
            sample.RecognizeFile(URL);

        }
    }

    public bool isReversed = false;
    public void PlayReversedReversed()
    {
        if(isReversed)
        {
            asource.clip = clip;
        }
        asource.Play();
        asource.loop = false;
    }
}