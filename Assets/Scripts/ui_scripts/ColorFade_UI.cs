using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorFade_UI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public bool useSin;
    public Image img;
    public bool onlyPositiveValues;
    public float speed;
    public float magnitude;

    public float SineAmount()
    {
        if(onlyPositiveValues)
        {
            return Mathf.Abs(magnitude * Mathf.Sin(Time.time * speed));
        }else
        {
            return magnitude * Mathf.Sin(Time.time * speed);
        }
    }

    private float _h,_s,_v,_a;
    
    private void Awake() 
    {
        //float _h,_s,_v,_a;
        _a = img.color.a;
        Color.RGBToHSV(img.color, out _h, out _s, out _v);
    }

    public bool h;
    public bool s;
    public bool v;
    public bool a;
    // Update is called once per frame
    void FixedUpdate()
    {
            if(useSin)
            {


                if(h)
                {
                    //_h + SineAmount();
                    _h -= SineAmount();
                    //Debug.Log("h : " + _h);
                }
                if(s)
                {
                    _s -= SineAmount();
                    //Debug.Log("s : " + _s);
                }
                if(v)
                {
                    _v -= SineAmount();
                    //Debug.Log("v : " + _v);
                }
                if(a)
                {
                    _a -= SineAmount();
                    Color col = Color.HSVToRGB(_h,_s,_v);
                    col.a = _a;
                    img.color = col;

                }else
                {
                    img.color = Color.HSVToRGB(_h,_s,_v);
                    Debug.Log(SineAmount() +  "sineamount");

                }


                    Debug.Log(SineAmount() +  "sineamount");
                   // Image.color -= Quaternion.Euler(SineAmount() * rot);
            }
    }
}
