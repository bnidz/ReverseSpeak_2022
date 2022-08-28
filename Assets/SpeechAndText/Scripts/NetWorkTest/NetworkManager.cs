// NetworkingManager.cs
using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

//using Newtonsoft.Json;

public class NetworkManager : MonoBehaviour
{
    public Image image;
    public AudioSource audioSource;
    private LoadWords lw;

    IEnumerator Start()
    {

        //replace with bool delegate? if prblems -- 
        yield return new WaitForSeconds(.1f);
        lw = FindObjectOfType<LoadWords>();

        GetPosts();

    }

    public void GetPosts()
    {

        StartCoroutine(GetRequest("http://192.168.8.116:8008/new_format_test.json", (UnityWebRequest req) =>
        {
            httpRequestDone = false;

            if (req.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log($"{req.error}: {req.downloadHandler.text}");
            }
            else
            {
                httpRequestDone = true;
                lw.new_GenerateGameWords(req.downloadHandler.text);

            }

        }));
    }

    public WordData wData_to_send;
    public void Upload()
    {
        //wData

        wData_to_send = lw.gameWordsList[1];

       // string jsonString = CreateFromJSON(string wData_to_send);

        string jsonString = JsonUtility.ToJson(lw.gameWordsList[1]);
        //StartCoroutine(UploadString(jsonString));

        StartCoroutine(Upload_string(jsonString));

    }

    public bool httpRequestDone = false;

    public IEnumerator Upload_string(string jstring)
    {
        httpRequestDone = false;
        using (UnityWebRequest www = UnityWebRequest.Post("http://192.168.8.116:8008/", jstring))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                httpRequestDone = true;
                Debug.Log("Server jSon updated!");
            }
        }
    }

    //IEnumerator UploadString(string uploadString)
    //{
    //    byte[] myData = System.Text.Encoding.UTF8.GetBytes(uploadString);
    //    UnityWebRequest www = UnityWebRequest.Post("http://localhost:8008/", myData.ToString());
    //    yield return www.SendWebRequest();

    //    if (www.result == UnityWebRequest.Result.ConnectionError)
    //    {
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        Debug.Log("Upload complete!");
    //    }
    //}

    IEnumerator GetRequest(string endpoint, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(endpoint))
        {
            // Send the request and wait for a response
            yield return request.SendWebRequest();

            callback(request);
        }
    }

    public void DownloadImage(string url)
    {
        StartCoroutine(ImageRequest(url, (UnityWebRequest req) =>
        {
            if (req.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log($"{req.error}: {req.downloadHandler.text}");
            }
            else
            {
                // Get the texture out using a helper downloadhandler
                Texture2D texture = DownloadHandlerTexture.GetContent(req);
                // Save it into the Image UI's sprite
                image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
        }));
    }

    IEnumerator ImageRequest(string url, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest req = UnityWebRequestTexture.GetTexture(url))
        {
            yield return req.SendWebRequest();
            callback(req);
        }
    }

    public void DownloadSound(string url)
    {
        StartCoroutine(SoundRequest(url, (UnityWebRequest req) =>
        {
            if (req.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log($"{req.error}: {req.downloadHandler.text}");
            }
            else
            {
                // Get the sound out using a helper downloadhandler
                AudioClip clip = DownloadHandlerAudioClip.GetContent(req);
                // Load the clip into our audio source and play
                audioSource.Stop();
                audioSource.clip = clip;
                audioSource.Play();
            }
        }));
    }

    IEnumerator SoundRequest(string url, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.OGGVORBIS))
        {
            yield return req.SendWebRequest();
            callback(req);
        }
    }
}

// Data Classes
public class Post
{
    public int userId;
    public int id;
    public string title;
    public string body;
}