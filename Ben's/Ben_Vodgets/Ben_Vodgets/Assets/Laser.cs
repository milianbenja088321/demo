using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : SteamVR_TrackedController {

    LineRenderer line;
    SteamVR_TrackedController controller = null;
    public float angle = 30;
    GameObject currFocus = null;

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<SteamVR_TrackedController>();

        controller.TriggerClicked += OnClicked;
        controller.TriggerUnclicked += OnUnClicked;

        line = this.GetComponent<LineRenderer>();
    }

    void OnClicked(object sender, ClickedEventArgs e)
    {
        line.enabled = true;
    }

    void OnUnClicked(object sender, ClickedEventArgs e)
    {
        line.enabled = false;
    }


    // Update is called once per frame
    protected override void Update ()
    {
        base.Update();



        line.SetPosition(0, this.transform.position);
        Quaternion q = Quaternion.AngleAxis(angle, transform.right);
        Vector3 v = q * (this.transform.forward * 10);
        Vector3 targetPos = this.transform.position + v;
        line.SetPosition(1, targetPos);

        // hold info for what ever the ray hits
        RaycastHit hit;
        if(Physics.Raycast(this.transform.position, v, out hit, 10))
        {
            currFocus = hit.collider.gameObject;
            //print(hit.collider.gameObject.name);
        }
        else
        {
            currFocus = null;
        }

    }

    public override void OnTriggerClicked(ClickedEventArgs e)
    {
        base.OnTriggerClicked(e);

        if (currFocus != null)
        {
            // grab what you pointing to
            currFocus.transform.SetParent(this.transform, true);

            Interactive l = currFocus.GetComponent<Interactive>();
            if (l != null)
            {
                l.something();
            }
        }
    }

    public override void OnTriggerUnclicked(ClickedEventArgs e)
    {
        base.OnTriggerUnclicked(e);

        if (currFocus != null)
        {
            // let go of what is being grabbed
            currFocus.transform.SetParent(null);
        }
    }
}
