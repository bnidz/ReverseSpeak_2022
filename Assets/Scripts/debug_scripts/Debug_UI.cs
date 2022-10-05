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
        Components.c.settings.thisPlayer.current_Skips -= 2;
        Components.c.filetotext.changeSkips = true;
    }
    public void MINUS_HEART()
    {
        Components.c.settings.thisPlayer.current_Hearts -= 2;

        Components.c.gameUIMan.Heart_Lose_Life();
        Components.c.filetotext.changeLifes = true;
    }
}
