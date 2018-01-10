using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_controller : Photon.MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if(photonView.isMine)
        {
            GameObject head = GameObject.FindGameObjectWithTag("Head");
            GameObject lefthand = GameObject.FindGameObjectWithTag("LeftH");
            GameObject righthand = GameObject.FindGameObjectWithTag("RightH");
            if (head)
            {
                Transform headtransform = transform.Find("Head");// GetComponentsInChildren();
                headtransform.position = head.transform.position;
                headtransform.rotation = head.transform.rotation;
            }
            if (lefthand)
            {
                Transform lefttransform = transform.Find("Hand left");// GetComponentsInChildren();
                lefttransform.position = lefthand.transform.position;
                lefttransform.rotation = lefthand.transform.rotation;
            }
            if (righthand)
            {
                Transform righttransform = transform.Find("Hand right");// GetComponentsInChildren();
                righttransform.position = righthand.transform.position;
                righttransform.rotation = righthand.transform.rotation;
            }
        }
    }
}
