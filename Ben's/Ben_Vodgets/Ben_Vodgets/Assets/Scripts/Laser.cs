using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

        Vector3 cVel = SteamVR_Controller.Input((int)controller.controllerIndex).velocity;
        Vector3 aVel = SteamVR_Controller.Input((int)controller.controllerIndex).angularVelocity;

        if (currFocus != null)
        {
            Interactive l = currFocus.GetComponent<Interactive>();
            if (l != null)
            {
                if (l.type == Interactive.InteractiveTypes.Grab)
                {
                    l.Release(cVel);
                }

                //if (l.type == Interactive.InteractiveTypes.Marker)
                //{
                //    AnotationManager canvs = currFocus.GetComponent<AnotationManager>();
                //    canvs.single.SetActive(false);
                //}

                if (l.type == Interactive.InteractiveTypes.Button)
                {
                    Button b = l.gameObject.GetComponent<Button>();
                    if(b != null)
                    {
                        b.onClick.Invoke();
                    }
                }
            }
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

            if (currFocus.gameObject.tag != "No Interaction")
                SteamVR_Controller.Input((int)controller.controllerIndex).TriggerHapticPulse(3999);

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
            Interactive l = currFocus.GetComponent<Interactive>();

            if (l != null)
            {
                if (l.type == Interactive.InteractiveTypes.Grab)
                {
                    l.Grab(this.transform);
                }

                if (l.type == Interactive.InteractiveTypes.Marker)
                {
                    AnotationManager canvs = currFocus.GetComponent<AnotationManager>();
                    canvs.single.SetActive(true);
                }

            }


        }
    }
}
