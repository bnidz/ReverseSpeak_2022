using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class splashImageScript : MonoBehaviour
{

    void Update()
    {
              if (Input.GetMouseButtonDown(0))
              {
                Components.c.gameManager.Init();
                Components.c.gameloop.Init();
                this.gameObject.SetActive(false);
              }
    }
}
