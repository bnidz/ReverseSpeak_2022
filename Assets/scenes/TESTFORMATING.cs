using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TextSpeech;
using Apple.GameKit;
using System;
using TMPro;
using System.Text;
using System.Text.RegularExpressions;

public class TESTFORMATING : MonoBehaviour
{
    // Start is called before the first frame update
    public Text iostext;
    public Text normaltext;


    void Awake()
    {
        trynaFormat();
        Debug.Log("done");
    }
    public void trynaFormat()

    {
        Debug.Log(iostext.text);
string unicodestring = "Sch\u00f6nen";
Debug.Log(@unicodestring);

        //byte[] shit = Encoding.Convert(Encoding.UTF8, Encoding.Default, Encoding.UTF8.GetBytes(iostext.text.ToLower()));
       iostext.text = System.Text.RegularExpressions.Regex.Unescape(iostext.text.ToLower());
        //iostext.text = Encoding.UTF8.GetString(shit);
        Debug.Log("");
        Debug.Log(iostext.text.ToLower());
    }
}
