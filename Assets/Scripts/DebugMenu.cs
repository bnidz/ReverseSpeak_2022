using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DebugMenu : MonoBehaviour
{
    public Toggle debugToggle;
    public GameObject debugPanel;
    private bool toggle = false; 

    public void ToggleDebug()
    {
        toggle = !toggle;
        DebugToggel(toggle);
    }

    public void DebugToggel (bool toggle)
    {
        if(toggle)
        {
            debugPanel.SetActive(true);
        }else
        {
            debugPanel.SetActive(false);
        }
    }
}