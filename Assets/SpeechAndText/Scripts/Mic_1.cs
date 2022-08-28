using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.iOS;
[RequireComponent(typeof(AudioSource))]
public class Mic_1 : MonoBehaviour
{
	//A boolean that flags whether there's a connected microphone
	public bool micConnected = false;

	//The maximum and minimum available recording frequencies
	private int minFreq;
	public int maxFreq;


	//clip length values & buffer
	public int bufferInSec = 1200;


	//private ProgressHolder pholdz;

	//torecord
	bool isRecording = true;
	public int timeTag;
	public bool isRecorded = false;
	public int lenght;

	//A handle to the attached AudioSource
	public AudioSource temp_audiosource;
	public AudioSource bufferAudioSource;

	//sends for receiver
	public GameObject Receiver;
	//public ReceiverSourceScript rc;

	//clip duration buttons to spawn
	public int captureLenght;
	public GameObject[] durationButtons;
	//REC visualization
	//private visualisationScript vizScript;

	//BUTTON STUFF
	public GameObject StarButton;
	public Animator starAnimator;
	public ParticleSystem starParticles;
	private Vector2 starButton_defaultPos;

	//stareffect
	//public StarEffect pulse;

	//tut stuff
	//private TutorialManager tutman;


	public List<float[]> BufferClips = new List<float[]>();
	private void Awake()
	{
		//tutman = FindObjectOfType<TutorialManager>();
		starButton_defaultPos = StarButton.transform.position;
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
		Start();
	}


	void Start()
	{

		//Check if there is at least one microphone connected
		if (Microphone.devices.Length <= 0)
		{
			//MIC NOT CONNECTED - BUTTON STATE TO THAT
			//Throw a warning message at the console if there isn't
			Debug.LogWarning("Microphone not connected!");
		}
		else //At least one microphone is present
		{
			//Set 'micConnected' to true
			micConnected = true;
			//Get the default microphone recording capabilities
			Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);
			//pulse.pulse = true;

			if (minFreq == 0 && maxFreq == 0)
			{
				//...meaning 44100 Hz can be used as the recording sampling rate  
				maxFreq = 44100;
			}
			Debug.Log("DEVICE MIN FREQ: " + minFreq);
			//maxFreq = maxFreq;

			//automatic start recording - for now
		   	ButtonPress();
			//pholdz.ResetProgress();

		}
		if (micConnected == false)
		{
			//InvokeRepeating("Start", 0, 0.5f);
			StartCoroutine(_Start());
		}

		ButtonPress();
	}
	float SecElapsed = 0;
	int intSECS = 0;

	int saveSecond;
	//public List<int> secs = new List<int>();

	void Update()
	{
		if (Microphone.devices.Length >= 1)
		{
			SecElapsed += Time.deltaTime;
			intSECS = (int)Math.Ceiling(SecElapsed);

			if (readyToSwipe)
			{
				SwipeFunctionality();
			}
			if (returnToCenter)
			{
				ReturnToCenter();
			}
			if (Microphone.GetPosition(null) >= bufferInSec * maxFreq - maxFreq / 20) // -1s 
			{

				if (saveSecond != intSECS)
				{

					float[] samples = new float[Microphone.GetPosition(null)];
					temp_audiosource.clip.GetData(samples, 0);

					if (BufferClips.Count > 1)
					{
						BufferClips.RemoveAt(0);

					}
					BufferClips.Add(samples);
					turnCount++;
					Debug.Log("bufferclip count " + BufferClips.Count.ToString());
					Debug.Log("turncount" + turnCount);
					saveSecond = intSECS;

				}
			}
		}
	}

	public bool turn = false;
	public bool buttonPRess = false;

	//STAR BUTTON PRESS FUNCTIONALITY

	bool firstGo = false;
	bool secondGo = false;

	public List<float> buffar = new List<float>();

	public int turnCount = 0;

	public void ButtonPress()
	{
		//If there is a microphone
		if (micConnected)
		{

			if (!firstGo)
			{
				temp_audiosource.clip = Microphone.Start(null, true, bufferInSec, maxFreq);

				firstGo = true;
				return;
			}
		}
		else // No microphone
		{

			//Print a red "Microphone not connected!" message at the center of the screen
			GUI.contentColor = Color.red;
			GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 200, 50), "Microphone not connected!");
			//pulse.pulse = false;

		}
	}
	//SWIPE START
	public void OnButtonDown()
	{
		if (Microphone.IsRecording(null))
		{
			//	Debug.Log("BUTTON DOWN");
			readyToSwipe = true;
			SwipeFunctionality();
		}
	}
	public void ButtonUP()
	{
		if (Microphone.IsRecording(null))
		{
			returnToCenter = true;
		}
	}

	//SWIPE STUFF
	public GameObject SwipeAsset;
	public bool readyToSwipe = false;
	public bool REC_pressed = false;
	public bool swipeStarted = false;
	//private bool swipeDone;
	private float swipeLength;
	private float mouseMoveLenght;
	public Animator swipeAsset_animator;
	public GameObject durBubble;
	public Vector3 bubbleOffset;
	private bool returnToCenter, starReverse = false;
	Vector2 mouseDirection;
	public TMP_Text durText;
	private float swipeTime = 0;
	private int swipeValue, clipLength;
	private int curMaxLenght;

	public void SwipeFunctionality()
	{
		//fix
		if (Microphone.GetPosition(null) < bufferInSec * maxFreq && turnCount < 1)
		{
			timeTag = Microphone.GetPosition(null);
		}
		else
		{
			timeTag = bufferInSec * maxFreq;
		}
		if (!returnToCenter)
		{

			// DEBUG MOVE CODE WITH MOUSE
			Vector2 mouseStartpos = StarButton.transform.position;//new Vector2(0, 0);
			Vector2 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			starAnimator.SetBool("outline", true);
			mouseDirection = p - mouseStartpos;
			mouseMoveLenght = mouseDirection.magnitude;
			SwipeAsset.transform.position = new Vector2(p.x, p.y);
			SwipeAsset.SetActive(true);
			durBubble.transform.position = SwipeAsset.transform.position + bubbleOffset;

			//Jos Swipe lenghti yli 2

			if (mouseMoveLenght > 2)
			{
				starReverse = false;
				durBubble.GetComponent<SpriteRenderer>().color = Color.green;
				//timeTag = Microphone.GetPosition(null);// + bufferAudioSource.clip.samples;
				swipeTime += Time.deltaTime;
				durBubble.SetActive(true);
				swipeValue = Mathf.RoundToInt(mouseMoveLenght * 2 + swipeTime);

				if (timeTag > swipeValue * maxFreq)
				{
					durText.text = swipeValue.ToString();
					clipLength = swipeValue;
				}
				else
				{
					//durBubble.SetActive(false);
					curMaxLenght = Mathf.RoundToInt(timeTag / maxFreq);
					durText.text = curMaxLenght.ToString();
					durBubble.GetComponent<SpriteRenderer>().color = Color.red;
					clipLength = curMaxLenght;
				}
			}
			else
			{
				durBubble.GetComponent<SpriteRenderer>().color = Color.yellow;
				swipeTime -= Time.deltaTime;
				//	durBubble.SetActive(true);
				starReverse = true;
				swipeValue = Mathf.RoundToInt(mouseMoveLenght * 2 + swipeTime);
				if (timeTag > swipeValue * maxFreq && swipeValue * maxFreq >= 2)
				{
					durText.text = swipeValue.ToString();
					clipLength = swipeValue;
				}
			}
			if (!starReverse)
			{
				swipeAsset_animator.SetFloat("rotspeed", 1 + mouseMoveLenght);
			}
			else
			{
				swipeAsset_animator.SetFloat("rotspeed", -1 + -mouseMoveLenght);
			}
		}
	}

	//public IEnumerator TutDelay()
	//{
	//	//yield return new WaitForSeconds(.8f);
	//	//if (tutman.swipetut)
	//	//{
	//	//	tutman.ShowTutorial("release");
	//	//	tutman.HideTutorial("swipe");
	//	//	tutman.StopTalking();
	//	//}
	//}

	private void SwipeToClip(int lenght)
	{
		//reset the swipe length values
		clipLength = 0;
		swipeTime = 0;

		captureLenght = (maxFreq * lenght); //hmm..
		float[] newClip = new float[captureLenght];

		int lastPos = Microphone.GetPosition(null);
		int timestamp = Convert.ToInt32(SecElapsed);

		if (BufferClips.Count > 0)
		{
			float[] secondCLIP = new float[Microphone.GetPosition(null)];
			temp_audiosource.clip.GetData(secondCLIP, 0);
			float[] tempClip = new float[BufferClips[BufferClips.Count - 1].Length + secondCLIP.Length];
			BufferClips[BufferClips.Count - 1].CopyTo(tempClip, 0);
			secondCLIP.CopyTo(tempClip, BufferClips[BufferClips.Count - 1].Length);

			for (int i = 0; i < newClip.Length; i++)
			{
				newClip[i] = tempClip[tempClip.Length - newClip.Length + i];
			}
		}
		else
		{
			temp_audiosource.clip.GetData(newClip, Microphone.GetPosition(null) - captureLenght);
		}

		//send captured data to clip
		//rc.ReceiveClip(newClip);
	}

	public float snapSpeed;
	private void ReturnToCenter()
	{
		durBubble.transform.position = SwipeAsset.transform.position + bubbleOffset;
		durBubble.SetActive(false);
		float step = snapSpeed * Time.deltaTime;
		SwipeAsset.transform.position = Vector3.MoveTowards(SwipeAsset.transform.position, StarButton.transform.position, step);
		Vector2 backdir = new Vector2(SwipeAsset.transform.position.x, SwipeAsset.transform.position.y) - new Vector2(StarButton.transform.position.x, StarButton.transform.position.y);
		float snapLenght = backdir.magnitude;

		swipeAsset_animator.SetFloat("rotspeed", 1 + snapLenght);
		if (snapLenght < 1)
		{
			SetButtonBack();
			returnToCenter = false;
		}
	}

	private void SetButtonBack()
	{
		SwipeAsset.transform.position = StarButton.transform.position;
		SwipeAsset.SetActive(false);
		//StarButton.GetComponent<Image>().enabled = true;
		starAnimator.SetBool("outline", false);
		starAnimator.Play("return_twirl_anim");
		readyToSwipe = false;
		StartCoroutine(DoTheClip());
	}

	private IEnumerator DoTheClip()
	{
		yield return new WaitForSeconds(.2f);
		SwipeToClip(clipLength);
	}
}