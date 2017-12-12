using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkScript : MonoBehaviour 
{

    float timer;
    float waitTime = 0.5f;
    float resetPoint;
    public bool isOn;
    public Material mTerial;



    void Start()
    {
        mTerial = GetComponent<Renderer>().material;
        mTerial.color = Color.blue;
		resetPoint = waitTime * 2;
    }
    
    // Update is called once per frame
    void Update () 
    {
        timer += Time.deltaTime;

        if (timer < waitTime) { mTerial.color = Color.white; }

        if (timer > waitTime) { mTerial.color = Color.red; }

        if (timer > resetPoint) { timer = 0; }
	}
}
