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
    private Image thisImage;
    private Vector3 ogPos;
    private Color origColor;
    bool variation = false;
    public void Drop(Image img)
    {
        scale = 1;
        origColor = img.color;
        variation = !variation;
        if(img.name.Contains("right"))
        {
            isRight = true;
        }
        else
        {
            isRight = false;
        }
        thisImage = img;
        ogPos = transform.position;
        timer = destructTime;
        rb.isKinematic = false;
        doDrop = true;

        shatterForce = UnityEngine.Random.Range(30f, 90f);
        if(isRight)
        {
            if(variation)
            {
                rb.AddForce((Vector2.right * shatterForce) + Vector2.up * (shatterForce*4));
                rb.AddTorque(-shatterForce/5, ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce((-Vector2.right * shatterForce) + Vector2.up * (shatterForce*4));
                rb.AddTorque(shatterForce/5, ForceMode2D.Impulse);
            }
        }
        else
        {   
            if(variation)
            {
                rb.AddForce((-Vector2.right * shatterForce) + Vector2.up * (shatterForce*4));
                rb.AddTorque(shatterForce/5, ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce((Vector2.right * shatterForce) + Vector2.up * (shatterForce*4));
                rb.AddTorque(-shatterForce/5, ForceMode2D.Impulse);
            }
        }
    }

    private bool doDrop = false;
    private bool isRight;
    private float shatterForce;
    private float scale;
    private float reductionValue = 0.0525f;
    void FixedUpdate()
    {
        if(doDrop)
        {

            timer -= Time.deltaTime;
            float H,S,V;
            Color.RGBToHSV(thisImage.color, out H, out S, out V);
            V -= 0.0067f;
            thisImage.color = Color.HSVToRGB(H,S,V);
            scale += 0.0625f;

            if(timer > 1.65f)
            {
                //grow
                thisImage.transform.localScale = new Vector3(scale,scale,1);
            }
            if(timer < 1.6 && timer > 0.1f)
            {
                //shrink
                if(thisImage.transform.localScale.x > 0.1f)
                {
                    thisImage.transform.localScale = new Vector3(thisImage.transform.localScale.x - reductionValue,
                        thisImage.transform.localScale.y - reductionValue, 1);
                }
            }
            if(timer <= 0)
            {
                rb.velocity = Vector3.zero;
                rb.rotation = 0;
                rb.isKinematic = true;
                thisImage.transform.localScale = new Vector3(1,1,1);
                this.transform.rotation = Quaternion.identity;
                this.transform.position = ogPos;
                thisImage.enabled = false;
                thisImage.color = origColor;
                doDrop = false;
            } 
        }
    }
}
