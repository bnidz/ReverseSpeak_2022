using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class heart_piece : MonoBehaviour
{
    public float destructTime;
    private float timer;
    public Rigidbody2D rb;
    public bool drop =false;
    // Update is called once per frame
    private Image thisImage;
    private Vector3 ogPos;
    public void Drop(Image img)
    {
        ogPos = transform.position;
        timer = destructTime;
        rb.isKinematic = false;
        doDrop = true;
    }
    private bool doDrop = false;
    void FixedUpdate()
    {
        if(doDrop)
        {
            timer -= Time.deltaTime;
            if(destructTime <= 0)
            {
                this.transform.position = ogPos;
                this.transform.rotation = Quaternion.identity;
                rb.isKinematic = true;
                thisImage.enabled = false;
                doDrop = false;
            } 
        }
    }
}
