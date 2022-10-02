using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineTransform : MonoBehaviour
{

    [Header("Values Settings")]
    public float magnitude;
    public float speed;
    public bool useSin;
    public bool onlyPositiveValues;
    public bool affectLocal;

    [Header("Scale Settings")]
    public Vector3 scale;
    public bool useScale;
    private Vector3 og_loc_scale;
    [Header("Position Settings")]
    public Vector3 pos;
    private Vector3 og_pos;
    public bool usePos;
    [Header("Rotation Settings")]
    public Vector3 rot;
    public bool useRot;
    
    void Awake()
    {
        og_loc_scale = this.transform.localScale;
        og_pos = this.transform.position;
    }

    // Start is called before the first frame update
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
    // Update is called once per frame
    void FixedUpdate()
    {
        if(useRot)
        {
            if(useSin)
            {
                if(affectLocal)
                {
                    transform.localRotation *= Quaternion.Euler(SineAmount() * rot);
                }
                else
                {
                    transform.rotation *= Quaternion.Euler(SineAmount() * rot);
                }
            }
            else
            {
                if(affectLocal)
                {
                    transform.localRotation *= Quaternion.Euler(rot * speed * Time.deltaTime);
                }
                else
                {
                    transform.rotation *= Quaternion.Euler(rot * speed * Time.deltaTime);
                }    
            }
        }
        if(usePos)
        {
            if(useSin)
            {
                if(affectLocal)
                {
                    transform.localPosition = new Vector3(
                    og_pos.x + SineAmount() * pos.x,
                    og_pos.y + SineAmount() * pos.y,
                    og_pos.z + SineAmount() * pos.z
                    );
                }
                else
                {
                    transform.position = new Vector3(
                    og_pos.x + SineAmount() * pos.x,
                    og_pos.y + SineAmount() * pos.y,
                    og_pos.z + SineAmount() * pos.z
                    );
                }
            }else
            {
                if(affectLocal)
                {
                    transform.localPosition += (pos * speed * Time.deltaTime);
                }
                else
                {
                    transform.position += (pos * speed * Time.deltaTime);
                }
            }
        }

        if(useScale)
        {   
            if(useSin)
            {
                if(affectLocal)
                {
                    transform.localScale = og_loc_scale + SineAmount() * scale;
                }
                else
                {
                    transform.localScale = og_loc_scale + SineAmount() * scale;
                }
            }
            else
            {
                if(affectLocal)
                {
                    transform.localScale += scale * speed * Time.deltaTime;
                }
                else
                {
                    transform.localScale += scale * speed * Time.deltaTime;
                }
            }
        }
    }
}