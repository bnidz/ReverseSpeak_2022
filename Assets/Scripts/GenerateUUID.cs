using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GenerateUUID
{
    public static byte[] UUID()
    {
        Guid myuuid = Guid.NewGuid();
     //   string myuuidAsString = myuuid.ToString();
        var bytes = myuuid.ToByteArray();
        //Debug.Log("Your UUID is: " + myuuidAsString);

        return bytes;
    }
}