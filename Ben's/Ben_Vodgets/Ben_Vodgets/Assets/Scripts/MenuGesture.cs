using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(SteamVR_TrackedController))] 

public class MenuGesture : MonoBehaviour {

    SteamVR_TrackedController controller;

    LineRenderer line;
    bool drawline = false;

    public float controllerRot;
    public float angle = 30;

    // Use this for initialization
    void Start () {
        controller = GetComponent<SteamVR_TrackedController>();

        controller.TriggerClicked += OnClicked;
        controller.TriggerUnclicked += OnUnClicked;


        line = this.GetComponent<LineRenderer>();
    }

    void OnClicked(object sender, ClickedEventArgs e)
    {
        Debug.Log(controllerRot);
        line.enabled = true;
    }

    void OnUnClicked(object sender, ClickedEventArgs e)
    {
        line.enabled = false;
    }

    // Update is called once per frame
    void Update () {
        line.SetPosition(0, this.transform.position);
        Quaternion q = Quaternion.AngleAxis(angle, transform.right);
        Vector3 v = q * (this.transform.forward * 10);
        Vector3 targetPos = this.transform.position + v;
        line.SetPosition(1, targetPos);

        controllerRot = this.transform.localEulerAngles.z;
        //print(this.transform.localPosition);    
    }
}
