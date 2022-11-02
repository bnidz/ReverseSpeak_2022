using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sfxManager : MonoBehaviour
{
    public AudioSource asource;
    public AudioClip[] clips;
    public void PlaySFX(string cname)
    {
        asource.Play();
        for (int i = 0; i < clips.Length; i++)
        {
            if(clips[i].name == cname)
            {
                asource.clip = clips[i];
                asource.Play();
                return;
            }
        }
    }
}
