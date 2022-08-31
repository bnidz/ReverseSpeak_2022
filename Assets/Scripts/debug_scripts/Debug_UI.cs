using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_UI : MonoBehaviour
{
   
    public void ADD_SKIP()
    {
        Components.c.filetotext.changeSkips = true;
    }
    public void ADD_HEART()
    {
        Components.c.filetotext.changeLifes = true;
    }
    public void MINUS_SKIP()
    {
        Components.c.settings.currentConfigs.current_Skips -= 2;
        Components.c.filetotext.changeSkips = true;
    }
    public void MINUS_HEART()
    {
        Components.c.settings.currentConfigs.current_Hearts -= 2;
        Components.c.filetotext.changeLifes = true;
    }


}
