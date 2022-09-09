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
        if(img.name.Contains("right"))
        {
            isRight = true;

        }
        else
        {
            isRight = false;
        }
        ogPos = transform.position;
        timer = destructTime;
        rb.isKinematic = false;
        doDrop = true;
    }
    private bool doDrop = false;

    private bool isRight;
    private float shatterForce;
    void FixedUpdate()
    {
        if(doDrop)
        {
            shatterForce = UnityEngine.Random.Range(0.1f, 0.8f);
            if(isRight)
            {
                rb.AddForce((Vector2.right * shatterForce) + Vector2.up * shatterForce);
            }else
            {
                rb.AddForce((-Vector2.right * shatterForce) + Vector2.up * shatterForce);
            }
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                rb.velocity = Vector3.zero;

                rb.isKinematic = true;
                this.transform.position = ogPos;//transform.parent.position;
                this.transform.rotation = Quaternion.identity;
                thisImage.enabled = false;
                doDrop = false;
            } 
        }
    }
}
