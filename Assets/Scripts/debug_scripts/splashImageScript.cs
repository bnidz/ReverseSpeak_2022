using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class splashImageScript : MonoBehaviour
{

  private bool hasPassed = false;
    void Update()
    {
      if(!hasPassed)
      {
        if (Input.GetMouseButtonDown(0))
        {

          Components.c.runorder.LoadComponents();
          hasPassed = true;
        }

      }
    }
}
