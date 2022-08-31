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


}
