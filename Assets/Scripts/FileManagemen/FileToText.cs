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

        //min press time value
        howLongToPress = 0.25f;

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

    public Image hourglass_HUD_heart;
    public Image hourglass_HUD_skips;

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
        //speed = speedEffect;
        //buffer = new float[maxFreq * 5];

        iPhoneSpeaker.ForceToSpeaker();
    }
    

    private float[] buffer;
    private bool first  = true;

    void Update()
    {
        if(startUpdates)
        {
            UpdateTimers();
            // if(pressInAdvance)
            // {
            //     WaitTilcanPress();
            // }
        }
    }
    private bool pressInAdvance = false;
    public void EndAndReturnMic()
    {

      //  Microphone.End(null);
        asource.clip = Microphone.Start(null, false, 5, maxFreq);
    }

    private void LateUpdate() {

        if(startUpdates)
        {
            ButtonUpdate();
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
        if(Components.c.settings.thisPlayer.current_Hearts < Components.c.settings.thisConfigs.max_Hearts)
        {
            if(hourglass_HUD_heart.enabled == false)
            {
                hourglass_HUD_heart.enabled = true;
            }
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
        }else
        {
            hourglass_HUD_heart.enabled = false;
            Components.c.gameUIMan.lifesTimer.text = "";
        }

        if(Components.c.settings.thisPlayer.current_Skips < Components.c.settings.thisConfigs.max_Skip_Amount)
        {

            if(hourglass_HUD_skips.enabled == false)
            {
                hourglass_HUD_skips.enabled = true;
            }

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
        }else
        {
                hourglass_HUD_skips.enabled = false;
                Components.c.gameUIMan.skipsTimer.text = "";
        }



        if(changeLifes)
        {    
            if(Components.c.settings.thisPlayer.current_Hearts == 0)
            {
                Components.c.gameUIMan.EmptyToOneHeart();
            }

            Components.c.settings.thisPlayer.current_Hearts++;
            heartCoolDown = Components.c.settings.thisConfigs.heart_CoolDown;
            Components.c.gameUIMan.UpdateUIToConfigs();
            Components.c.settings.SavePlayerConfigs();
            changeLifes = false;
        }
        if(changeSkips)
        {
            Components.c.gameUIMan.ActivateSkipButton();
            Components.c.settings.thisPlayer.current_Skips++;
            skipCoolDown = Components.c.settings.thisConfigs.skip_CoolDown;
            Components.c.gameUIMan.UpdateUIToConfigs();
            Components.c.settings.SavePlayerConfigs();
            changeSkips = false;

        }
        if (Components.c.settings.thisPlayer.current_Hearts == Components.c.settings.thisConfigs.max_Hearts)
        {
            Components.c.gameUIMan.lifesTimer.text = "";
        }
        if (Components.c.settings.thisPlayer.current_Skips == Components.c.settings.thisConfigs.max_Skip_Amount)
        {
            Components.c.gameUIMan.skipsTimer.text = "";
        }
    }

    private Vector3 rot;

    private bool innerChanged = false;
    private bool outerChanged = false;
    private bool buttonChanged = false;
    public void ButtonUpdate()
    {
        if (_pointerDown && canPushButton)
        {
           // pressTime += Time.deltaTime;
            if(Components.c.settings.thisPlayer.current_Hearts > 0)
            {
                // scale += Time.deltaTime * speed;
                rot += Vector3.forward*-120*Time.deltaTime; //increment 30 degrees every second
                
                Components.c.gameUIMan.r1.m_angularOffset += 120*Time.deltaTime;
                Components.c.gameUIMan.r2.m_angularOffset += 120*Time.deltaTime;
                Components.c.gameUIMan.b1.m_angularOffset += 120*Time.deltaTime;
                Components.c.gameUIMan.b2.m_angularOffset += 120*Time.deltaTime;

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
        }
        if(!_pointerDown)
        {
            Components.c.gameUIMan.r1.m_angularOffset += 120*Time.deltaTime;
            Components.c.gameUIMan.r2.m_angularOffset += 120*Time.deltaTime;
            Components.c.gameUIMan.b1.m_angularOffset += 120*Time.deltaTime;
            Components.c.gameUIMan.b2.m_angularOffset += 120*Time.deltaTime;
            // scale += Time.deltaTime * speed;
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
    public float howLongToPress = 0.25f;
    public AudioClip clip;
    public bool _pointerDown = false;

    private float pressTime;
    public void OnPointerDown(PointerEventData eventData)
    {
        _pointerDown = true;
        if(!canPushButton)
        {
            Components.c.sfxmanager.PlaySFX("null_rec_fx");
            //pressInAdvance = true;
            return;
        }
        if(Components.c.settings.thisPlayer.current_Hearts < 1)
        {
            Components.c.sfxmanager.PlaySFX("null_rec_fx");
            //pressInAdvance = true;
            return;
        }
        else
        {
            StartDoingTheClipRecord();
            Components.c.gameUIMan.CircularTexts_ChangeColor_RtoB();
        }
    }
    private float theClipLength;
    public void OnPointerUp(PointerEventData eventData)
    {
         pressTriggered = false;
        _pointerDown = false;
        {
            Debug.Log("maxfreq " + maxFreq);
            Debug.Log("you pushed the button for  " + theClipLength);
            DoTheClip();
        }
    }

    public void changeRingColors(bool c)
    {
        if(c)
        {
            Components.c.gameUIMan.gameBTN_1.color = Components.c.gameUIMan.r_color_2;
            Components.c.gameUIMan.gameBTN_2.color = Components.c.gameUIMan.r_color_3;
        }else
        {
            Components.c.gameUIMan.gameBTN_1.color = Components.c.gameUIMan.b_color_2;
            Components.c.gameUIMan.gameBTN_2.color = Components.c.gameUIMan.b_color_3;
        }
    }

    private bool pressTriggered = false;
    public void WaitTilcanPress()
    {
        if(canPushButton && !pressTriggered)
        {
            StartDoingTheClipRecord();
            pressTriggered = true;
        }
    }
    public GameObject iphoneSpeaker;
    private void StartDoingTheClipRecord()
    {
        if(Components.c.settings.thisPlayer.current_Hearts >= 1 && canPushButton)  
        {
            Components.c.sampleSpeechToText.ClearResultList();
            //Figure out if this still eats memory.. 
            asource.clip = Microphone.Start(null, true, 5, maxFreq);
            iPhoneSpeaker.ForceToSpeaker();
            //effect.SetActive(true);
            scale = 1;
       }
    }

    private void DoTheClip()
    {
        // if(pressTime > 9)
        // {
            
        //     pressTime = 0;
        //     return;
        // }
        if(Components.c.settings.thisPlayer.current_Hearts >= 1)
        {
            if(Microphone.GetPosition(null) > (maxFreq * howLongToPress))
            {

                if(Microphone.GetPosition(null) < (maxFreq * 1))
                {
                    //effect.SetActive(false);
                    string _filename = "quess.wav";
                    gameObject.GetComponent<FileToText>().filename = _filename;

                    float[] _samples = new float[Microphone.GetPosition(null)];
                    asource.clip.GetData(_samples, 0);
                    isReversed = true;
                    clip = AudioClip.Create("tagClip", maxFreq * 1, 1, maxFreq, false);
                    if (isReversed)
                    {
                        Array.Reverse(_samples);
                    }

                    float[] emptySec = new float[maxFreq];
                    for (int i = 0; i < _samples.Length; i++)
                    {
                        emptySec[i] = _samples[i];
                    }
                    for (int i = _samples.Length; i < maxFreq; i++)
                    {
                        emptySec[i] = 0.001f;
                    }
Debug.Log(" :D :D SUPER DATA CLIP :D");
                    clip.SetData(emptySec, 0);
                    //clip.SetData(_samples, 0);
                    SaveWav.Save(_filename, clip);
                    Microphone.End(null);
                    string _URL = Application.persistentDataPath + "/" + _filename.ToString();
                    Components.c.sampleSpeechToText.RecognizeFile(_URL);

                StartCoroutine(_PlayReversedReversed());
                return;
                }

                //effect.SetActive(false);
                string filename = "quess.wav";
                gameObject.GetComponent<FileToText>().filename = filename;

                float[] samples = new float[Microphone.GetPosition(null)];
                asource.clip.GetData(samples, 0);
                isReversed = true;
                clip = AudioClip.Create("tagClip", samples.Length, 1, maxFreq, false);
                if (isReversed)
                {
                    Array.Reverse(samples);
                }
                clip.SetData(samples, 0);
                SaveWav.Save(filename, clip);
                Microphone.End(null);
                string URL = Application.persistentDataPath + "/" + filename.ToString();
                Components.c.sampleSpeechToText.RecognizeFile(URL);

            StartCoroutine(_PlayReversedReversed());
            //PlayReversedReversed();
            }else
            {
                Components.c.sfxmanager.PlaySFX("null_rec_fx");
                Components.c.gameUIMan.CircularTexts_ChangeColor_BtoR();
            }
       }
    }

    public void StartRecordForCheck()
    {
        asource.clip = Microphone.Start(null, true, 5, maxFreq);
        //StartCoroutine(waitClip());
    }
    private IEnumerator waitClip()
    {

        yield return new WaitForSeconds(4);
        //_DoTheClip();
    }   

    public void _DoTheClip()
    {
        string filename = "quess.wav";
        gameObject.GetComponent<FileToText>().filename = filename;

        float[] samples = new float[Microphone.GetPosition(null)];
        asource.clip.GetData(samples, 0);
        isReversed = true;
        clip = AudioClip.Create("tagClip", samples.Length, 1, maxFreq, false);
        // if (isReversed)
        // {
        //     Array.Reverse(samples);
        // }
        clip.SetData(samples, 0);
        SaveWav.Save(filename, clip);
        Microphone.End(null);
        string URL = Application.persistentDataPath + "/" + filename.ToString();
        Components.c.sampleSpeechToText.RecognizeFile(URL);
    }

    public bool isReversed;// = true;
    public void PlayReversedReversed()
    {
        if(isReversed)
        {
            asource.clip = clip;
        }
        asource.Play();
        asource.loop = false;
    }

    public bool isPlayingReversed = false;
    public IEnumerator _PlayReversedReversed()
    {
        isPlayingReversed = true;
        yield return null;
        if(isReversed)
        {
            asource.clip = clip;
        }
        asource.Play();
        asource.loop = false;
        yield return new WaitForSeconds (Components.c.filetotext.clip.length);
        isPlayingReversed = false;

    }
}