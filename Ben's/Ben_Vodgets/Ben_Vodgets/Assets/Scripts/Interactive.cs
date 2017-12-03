using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    // Use this for initialization
    public void something()
    {
        print("Interacted with : " + this.gameObject.name );
    }
}
