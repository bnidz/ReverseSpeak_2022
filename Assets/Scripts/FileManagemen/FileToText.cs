using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;
using System.IO;
using UnityEngine.iOS;
using UnityEngine.UI;
[RequireComponent(typeof(AudioSource))]
public class FileToText : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject effect;
    public float speedEffect = 1;
    public float scaleEffect = 1.2f;
    float speed;
    float scale = 1;

    private int minFreq;
    private int maxFreq;

    public bool micConnected = false;
    //private SavWav sawwav;
    public AudioSource asource;

    public bool startUpdates = false;
    public void Init()
    {
        asource = gameObject.GetComponent<AudioSource>();
        StartCoroutine(_Start());
    }
    
    IEnumerator _Start()
    {
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
        //effect.SetActive(false);
        speed = speedEffect;

        //MIC STARTUP STUFF
        //Check if there is at least one microphone connected
        if (Microphone.devices.Length <= 0)
        {
            Debug.LogWarning("Microphone not connected!");
            micConnected = false;
        }
        else //At least one microphone is present
        {
            micConnected = true;
            //Get the default microphone recording capabilities

            Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);
            if (minFreq == 0 && maxFreq == 0)
            {
                //...meaning 44100 Hz can be used as the recording sampling rate  
                maxFreq = 441000;
            }
        }
        if (micConnected == false)
        {
            StartCoroutine(_Start());
        }
        //effect.SetActive(false);
        speed = speedEffect;

    }
    void Update()
    {
        if(startUpdates)
        {
            //MicBuffer();
            ButtonUpdate();
            UpdateTimers();
        }
    }

    public float skipCoolDown;
    public float heartCoolDown;
    public bool changeSkips = false;
    public bool changeLifes = false;

    private DateTime startTime;
    public void UpdateTimers()
    {
        startTime = DateTime.Now;
        if(Components.c.settings.currentPlayer.current_Hearts < Components.c.settings.currentConfigs.max_Hearts)
        {
            heartCoolDown -= Time.deltaTime;
            //DateTime startTime = DateTime.Now;
            DateTime endTime = DateTime.Now.AddSeconds(heartCoolDown);
            TimeSpan span = endTime.Subtract ( startTime );
            //String yourString = string.Format("{0} days, {1} hours, {2} minutes, {3} seconds",
            if(span.Hours < 1 )
            {
                String yourString = string.Format("{0}min, {1}s",
                    span.Minutes, span.Seconds);
                Components.c.gameUIMan.lifesTimer.text = yourString;
            }
            if(span.Minutes < 1 )
            {
                String yourString = string.Format("{0}s",
                    span.Seconds);
                Components.c.gameUIMan.lifesTimer.text = yourString;
            }
            if(heartCoolDown <= 0)
            {
                changeLifes = true;
            }
        }

        if(Components.c.settings.currentPlayer.current_Skips < Components.c.settings.currentConfigs.max_Skip_Amount)
        {

            skipCoolDown -= Time.deltaTime;
            //DateTime startTime = DateTime.Now;
            DateTime endTime = DateTime.Now.AddSeconds(skipCoolDown);
            TimeSpan span = endTime.Subtract ( startTime );
            if(span.Hours < 1 )
            {
                String yourString = string.Format("{0}min, {1}s",
                    span.Minutes, span.Seconds);
                Components.c.gameUIMan.skipsTimer.text = yourString;
            }
            if(span.Minutes < 1 )
            {
                String yourString = string.Format("{0}s",
                    span.Seconds);
                Components.c.gameUIMan.skipsTimer.text = yourString;
            }

            //skipCoolDown -= Time.deltaTime;
            if(skipCoolDown <= 0)
            {
                changeSkips = true;
            }
        }
        if(changeLifes)
        {
            Components.c.settings.currentPlayer.current_Hearts++;
            heartCoolDown = Components.c.settings.currentConfigs.heart_CoolDown;
            Components.c.gameUIMan.UpdateUIToConfigs();
            Components.c.settings.SavePlayerConfigs();
            changeLifes = false;
        }
        if(changeSkips)
        {
            Components.c.gameUIMan.ActivateSkipButton();
            Components.c.settings.currentPlayer.current_Skips++;
            skipCoolDown = Components.c.settings.currentConfigs.skip_CoolDown;
            Components.c.gameUIMan.UpdateUIToConfigs();
            Components.c.settings.SavePlayerConfigs();
            changeSkips = false;

        }
        if (Components.c.settings.currentPlayer.current_Hearts == Components.c.settings.currentConfigs.max_Hearts)
        {
            Components.c.gameUIMan.lifesTimer.text = "";
        }
        if (Components.c.settings.currentPlayer.current_Skips == Components.c.settings.currentConfigs.max_Skip_Amount)
        {
            Components.c.gameUIMan.skipsTimer.text = "";
        }
    }

    private Vector3 rot;
    public void ButtonUpdate()
    {
        if (_pointerDown)
        {   

            scale += Time.deltaTime * speed;
            rot += Vector3.forward*-120*Time.deltaTime; //increment 30 degrees every second
            this.transform.rotation = Quaternion.Euler(rot);
            //buttonVisual.value = Microphone.GetPosition(null); //howLongToPress;
            if (scale > scaleEffect)
            {
                speed = -speedEffect;
            }
            if (scale < scaleEffect - 0.01f)
            {
                speed = speedEffect;
            }
           // effect.transform.localScale = new Vector3(scale, scale, 1);
        }
        if(!_pointerDown)
        {
            scale += Time.deltaTime * speed;

            if(transform.rotation.z < 0)
            {
                rot += Vector3.forward*40*Time.deltaTime; //increment 30 degrees every second
                this.transform.rotation = Quaternion.Euler(rot);
            }
        }
        if(!canPushButton)
        {
            Components.c.gameUIMan.skipButton.interactable = false;

        }else
        {
            Components.c.gameUIMan.skipButton.interactable = true;
        }
    }
    private bool rotdone = false;
    public bool audioSourceIsPlaying = false;
    public string filename;
    public bool canPushButton = true;
    public bool pointerDown = false;


    ///// GAME BUTTON STUFF --------------------

    public Slider buttonVisual;
    public float howLongToPress = 0.75f;
    public AudioClip clip;
    private bool _pointerDown = false;
    public void OnPointerDown(PointerEventData eventData)
    {
        // START THE CLIP STUFF
        _pointerDown = true;
        StartDoingTheClipRecord();
        // FX STUFF
        Components.c.gameUIMan.GameButtonColorChange(true);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _pointerDown = false;
        // DO THE CLIP STUFF
        DoTheClip();
        // FX STUFF 
        Components.c.gameUIMan.GameButtonColorChange(false);

    }

    private void StartDoingTheClipRecord()
    {
        if(Components.c.settings.currentPlayer.current_Hearts >= 1 && canPushButton)  
        {
            Components.c.sampleSpeechToText.ClearResultList();
            //Figure out if this still eats memory.. 
            asource.clip = Microphone.Start(null, true, 5, 441000);
            //effect.SetActive(true);
            scale = 1;
       }
    }

    private void DoTheClip()
    {
        if(Components.c.settings.currentPlayer.current_Hearts >= 1)
        {
            if(Microphone.GetPosition(null) > (441000 * howLongToPress ))
            {
                //effect.SetActive(false);
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
                Components.c.sampleSpeechToText.RecognizeFile(URL);
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