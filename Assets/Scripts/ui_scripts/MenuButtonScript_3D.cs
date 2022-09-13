using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonScript_3D : MonoBehaviour
{

    public Vector3 rotDir;
    public float speed;

    public bool isLocRot;

    private float _rot;
    // Update is called once per frame
    void Update()
    {
        if(isLocRot)
        {
            _rot += Time.deltaTime *  speed;
            rotDir = new Vector3(_rot*rotDir.x, _rot*rotDir.y, _rot*rotDir.z);
            //Vector3 rotation = new Vector3(rot * rotDir);

            //rotDir *= _rot;
            this.transform.localRotation = Quaternion.Euler(rotDir);

        }else
        {
            transform.Rotate(rotDir * speed * Time.deltaTime);

        }
    }
}
