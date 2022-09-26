using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LaunchEventStartGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Components.c.runorder.m_StartGameEvent.Invoke();
    }


}
