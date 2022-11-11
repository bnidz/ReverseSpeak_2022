using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

public class FireBaseConnectionCheck : MonoBehaviour
{
    // Start is called before the first frame update
public void Init () //removed some unrelated lines of code from this method
{
    PresenceCheck();
}

public void PresenceCheck() //listen for change
{

    DatabaseReference connectCheck = FirebaseDatabase.DefaultInstance.GetReference(".info/connected");
    connectCheck.ValueChanged += CheckForDisconnect;
}

public void CheckForDisconnect(object sender, ValueChangedEventArgs args)
{
    DataSnapshot checkBool = args.Snapshot;

    if (checkBool == null) //if null, do nothing for now
    {
        return;
    }
    else if ((bool)checkBool.GetValue(true)) //if connected, set online to "T" and on disconnect set online value to "F"
    {

    }
}
}
