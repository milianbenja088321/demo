using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : SteamVR_TrackedController
{

    LineRenderer line;
    SteamVR_TrackedController controller = null;
    GameObject currFocus = null;

    public float angle = 30;
    public int lengthOfRay = 7;
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


        if (currFocus != null)
        {
            // let go of what is being grabbed
            currFocus.transform.SetParent(null);
        }
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();



        line.SetPosition(0, this.transform.position);
        Quaternion q = Quaternion.AngleAxis(angle, transform.right);
        Vector3 v = q * (this.transform.forward * lengthOfRay);
        Vector3 targetPos = this.transform.position + v;
        line.SetPosition(1, targetPos);

        // hold info for what ever the ray hits
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, v, out hit, lengthOfRay))
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
            if (currFocus.gameObject.tag == "CanGrab")
            {
                // grab what you pointing to
                currFocus.transform.SetParent(this.transform, true);
            }
            Interactive l = currFocus.GetComponent<Interactive>();
            if (l != null)
            {
                l.something();
            }
        }
    }
}
