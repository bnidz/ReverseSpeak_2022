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
    public void Init()
    {
        asource = gameObject.GetComponent<AudioSource>();
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
        ButtonUpdate();

        UpdateTimers();
    }

    public float skipCoolDown;
    public float heartCoolDown;
    private bool changeSkips = false;
    private bool changeLifes = false;
    public void UpdateTimers()
    {

        if(Components.c.settings.currentConfigs.current_Hearts < Components.c.settings.currentConfigs.max_Hearts)
        {
            DateTime startTime = DateTime.Now;
            DateTime endTime = DateTime.Now.AddSeconds(Math.Ceiling(heartCoolDown -= Time.deltaTime));
            TimeSpan span = endTime.Subtract ( startTime );

            String yourString = string.Format("{0} days, {1} hours, {2} minutes, {3} seconds",
                span.Days, span.Hours, span.Minutes, span.Seconds);

            Components.c.gameUIMan.lifesTimer.text = yourString;

            if(heartCoolDown <= 0)
            {
                changeLifes = true;
            }
        }else{
            Components.c.gameUIMan.lifesTimer.text = "";

        }
        if(Components.c.settings.currentConfigs.current_Skips < Components.c.settings.currentConfigs.max_Skip_Amount)
        {

            DateTime startTime = DateTime.Now;
            DateTime endTime = DateTime.Now.AddSeconds(Math.Ceiling(skipCoolDown -= Time.deltaTime));
            TimeSpan span = endTime.Subtract ( startTime );

            // Console.WriteLine( "Time Difference (seconds): " + span.Seconds );
            // Console.WriteLine( "Time Difference (minutes): " + span.Minutes );
            // Console.WriteLine( "Time Difference (hours): " + span.Hours );
            // Console.WriteLine( "Time Difference (days): " + span.Days );

            String yourString = string.Format("{0} days, {1} hours, {2} minutes, {3} seconds",
                span.Days, span.Hours, span.Minutes, span.Seconds);

            //visualise second changes
            Components.c.gameUIMan.skipsTimer.text = yourString;

            skipCoolDown -= Time.deltaTime;
            if(skipCoolDown <= 0)
            {
                changeSkips = true;
            }
        }else
        {
            Components.c.gameUIMan.lifesTimer.text = "";
        }

        if(changeLifes)
        {
            Components.c.settings.currentConfigs.current_Hearts++;
            heartCoolDown = Components.c.settings.currentConfigs.heart_CoolDown;
            Components.c.gameUIMan.UpdateUIToConfigs();
            changeLifes = false;
        }
        if(changeSkips)
        {
            Components.c.settings.currentConfigs.current_Skips++;
            skipCoolDown = Components.c.settings.currentConfigs.skip_CoolDown;
            Components.c.gameUIMan.UpdateUIToConfigs();
            changeSkips = false;
        }

    }
    public void ButtonUpdate()
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
        if(Components.c.settings.currentConfigs.current_Hearts >= 1)
        {
            sample.ClearResultList();
            //Figure out if this still eats memory.. 
            asource.clip = Microphone.Start(null, true, 5, 441000);

            effect.SetActive(true);
            scale = 1;
        }
    }

    public AudioClip clip;
    public void OnPointerUp(PointerEventData eventData)
    {

        //ADD EFFECT IF SHORTER THAN A SECOND

        if(Components.c.settings.currentConfigs.current_Hearts >= 1)
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