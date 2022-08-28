using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipButtonScript : MonoBehaviour
{

    private QuessLoop qL;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<QuessLoop>();
    }

    // Update is called once per frame
    public void ButtonPress()
    {
        qL.skip = true;
    }
}
