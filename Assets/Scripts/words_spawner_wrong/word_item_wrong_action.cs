using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class word_item_wrong_action : MonoBehaviour
{
    public TextMeshProUGUI thisWord;
    public float destructTime;
    private float timer;
    public Rigidbody2D rb;
    private Vector3 ogPos;
    public bool drop =false;
    // Start is called before the first frame update
    public void Drop(string word)
    {
        thisWord.text = word;
        ogPos = transform.position;
        timer = destructTime;
        rb.isKinematic = false;
        shatterForce = UnityEngine.Random.Range(-30f, 60f);
        drop = true;
        int y = Random.Range(0,1);
        if(y == 1)
        {
            y = -1;
        }else
        {
            y = 1;
        }
        rb.AddForce((Vector2.right * shatterForce) + Vector2.up * (System.Math.Abs(shatterForce)*4));
        rb.AddTorque(y * shatterForce/9, ForceMode2D.Impulse);
    }

    private float shatterForce;
    private float scale;
    private float reductionValue = 0.0525f;
    // Update is called once per frame
    void FixedUpdate()
    {
        if(drop)
        {

            timer -= Time.deltaTime;
            //float H,S,V;
            // Color.RGBToHSV(thisImage.color, out H, out S, out V);
            // V -= 0.0067f;
            // thisImage.color = Color.HSVToRGB(H,S,V);
           // scale += 0.0625f;

            // if(timer > 1.65f)
            // {
            //     //grow
            //     this.transform.localScale = new Vector3(scale,scale,1);
            // }
            // if(timer < 1.6 && timer > 0.1f)
            // {
            //     //shrink
            //     if(this.transform.localScale.x > 0.1f)
            //     {
            //         this.transform.localScale = new Vector3(this.transform.localScale.x - reductionValue,
            //             this.transform.localScale.y - reductionValue, 1);
            //     }
            // }
            if(timer <= 0)
            {
                rb.velocity = Vector3.zero;
                rb.rotation = 0;
                rb.isKinematic = true;
                //this.transform.localScale = new Vector3(1,1,1);
                this.transform.rotation = Quaternion.identity;
                this.transform.position = ogPos;
                // thisImage.enabled = false;
                // thisImage.color = origColor;
                thisWord.text = "";
                drop = false;
            } 

        }

    }
}