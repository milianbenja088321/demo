using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vodgets
{
    public class Selector : MonoBehaviour {

        public enum Button { Trigger, Menu, PadClicked, PadTouched, Gripped }

        // The Cursor property provides vodgets with the emulated controller world position and rotation.
        protected Srt cursor = new Srt();
        public Srt Cursor
        {
            get { return cursor; }
        }

        protected virtual void SetCursor()
        {
            // Developer should override in all inherited classes.
        }

        public void CopyFocusObjs(List<GameObject> objs)
        {
            // Save focus objects to grabbed objects persistent during grab.
            foreach (GameObject f in focus_objs)
                if (f != null)
                    objs.Add(f);
        }

        class ButtonHandler
        {
            protected Selector selector = null;
            SteamVR_TrackedController controller;
            Button button;
            protected List<GameObject> objs = new List<GameObject>();

            public ButtonHandler(Selector s, SteamVR_TrackedController c, Button b)
            {
                selector = s;
                controller = c;
                button = b;
                switch (button)
                {
                    case Button.Trigger:
                        controller.TriggerClicked += OnButtonDown;
                        controller.TriggerUnclicked += OnButtonUp;
                        break;
                    case Button.Menu:
                        controller.MenuButtonClicked += OnButtonDown;
                        controller.MenuButtonUnclicked += OnButtonUp;
                        break;
                    case Button.PadClicked:
                        controller.PadClicked += OnButtonDown;
                        controller.PadUnclicked += OnButtonUp;
                        break;
                    case Button.PadTouched:
                        controller.PadTouched += OnButtonDown;
                        controller.PadUntouched += OnButtonUp;
                        break;
                    case Button.Gripped:
                        controller.Gripped += OnButtonDown;
                        controller.Ungripped += OnButtonUp;
                        break;
                }
            }

            ~ButtonHandler()
            {
                switch (button)
                {
                    case Button.Trigger:
                        controller.TriggerClicked -= OnButtonDown;
                        controller.TriggerUnclicked -= OnButtonUp;
                        break;
                    case Button.Menu:
                        controller.MenuButtonClicked -= OnButtonDown;
                        controller.MenuButtonUnclicked -= OnButtonUp;
                        break;
                    case Button.PadClicked:
                        controller.PadClicked -= OnButtonDown;
                        controller.PadUnclicked -= OnButtonUp;
                        break;
                    case Button.PadTouched:
                        controller.PadTouched -= OnButtonDown;
                        controller.PadUntouched -= OnButtonUp;
                        break;
                    case Button.Gripped:
                        controller.Gripped -= OnButtonDown;
                        controller.Ungripped -= OnButtonUp;
                        break;
                }
            }

            protected virtual void OnButtonDown(object sender, ClickedEventArgs e)
            {
                // Save focus objects to grabbed objects persistent during grab.
                selector.CopyFocusObjs(objs);
                selector.SetCursor();

                // Notify grab on all grabbed objects.
                foreach (GameObject f in objs)
                {
                    Vodget[] vlist = f.GetComponents<Vodget>();
                    foreach (Vodget v in vlist)
                        v.DoButton(selector, button, true);
                }
            }

            protected virtual void OnButtonUp(object sender, ClickedEventArgs e)
            {
                selector.SetCursor();

                // Notify grab release on all grabbed objects
                foreach (GameObject f in objs)
                {
                    if (f != null)
                    {
                        Vodget[] vlist = f.GetComponents<Vodget>();
                        foreach (Vodget v in vlist)
                            v.DoButton(selector, button, false);
                    }
                }

                // Clear all grabbing state
                ClearObjs();
            }

            protected virtual void ClearObjs()
            {
                objs.Clear();
            }

        }

        // All Selectors with Controllers use the trigger button to control grabbing.
        // Note: Selectors without controllers must implement the DoGrab methods directly.
        class GrabButtonHandler : ButtonHandler
        {
            public GrabButtonHandler(Selector s, SteamVR_TrackedController c) : base(s, c, Button.Trigger)
            {
            }

            protected override void OnButtonDown(object sender, ClickedEventArgs e)
            {
                // Send generic button event
                base.OnButtonDown(sender, e);

                if (objs.Count > 0)
                {
                    selector.isGrabbing = true;
                    foreach (GameObject f in objs)
                    {
                        Vodget[] vlist = f.GetComponents<Vodget>();
                        foreach (Vodget v in vlist)
                            v.DoGrab(selector, true);
                    }
                }
            }

            protected override void ClearObjs()
            {
                if (selector.isGrabbing)
                {
                    selector.isGrabbing = false;
                    if (objs.Count > 0)
                    {
                        foreach (GameObject f in objs)
                        {
                            Vodget[] vlist = f.GetComponents<Vodget>();
                            foreach (Vodget v in vlist)
                                v.DoGrab(selector, false);
                        }
                    }
 
                    // Notify de-focus on grabbed objects no longer in focus objects.
                    foreach (GameObject f in objs)
                    {
                        if (f != null)
                        {
                            if (!selector.focus_objs.Contains(f))
                            {
                                Vodget[] vlist = f.GetComponents<Vodget>();
                                foreach (Vodget v in vlist)
                                    v.DoFocus(selector, false);
                            }
                        }
                    }

                    // Notify focus on focus objects not found in grabbed objects
                    foreach (GameObject f in selector.focus_objs)
                    {
                        if (f != null)
                        {
                            if (!objs.Contains(f))
                            {
                                Vodget[] vlist = f.GetComponents<Vodget>();
                                foreach (Vodget v in vlist)
                                    v.DoFocus(selector, true);
                            }
                        }
                    }
                }
                base.ClearObjs();
            }

            public void DoGrabbedUpdate()
            {
                if (objs.Count > 0)
                {
                    selector.SetCursor();

                    foreach (GameObject f in objs)
                    {
                        // Object can be deleted while grabbing.
                        if (f != null)
                        {
                            Vodget[] vlist = f.GetComponents<Vodget>();
                            foreach (Vodget v in vlist)
                                v.DoUpdate(selector);
                        }
                    }
                }

            }
        }

        // The FocusObj is the current focus object with vodgets.
        List<GameObject> focus_objs = new List<GameObject>();
        protected bool isGrabbing = false;

        // The ColliderObj is the object that either direct or ray collision occured on for vodget selection.
        GameObject collider_obj = null;
        public GameObject ColliderObj
        {
            get { return collider_obj; }
        }

        // The Controller property is added to allow vodgets to access other controller state and events.
        // Note: Vodgets should be aware that some selectors like the head ray may return null.
        protected SteamVR_TrackedController controller = null;
        //public SteamVR_TrackedController Controller {
        //    get { return controller; }
        //}

        GrabButtonHandler trigger_handler = null;
        ButtonHandler menu_handler = null;
        ButtonHandler padClicked_handler = null;
        ButtonHandler padTouched_handler = null;
        ButtonHandler gripped_handler = null;

        SteamVR_Controller.Device device = null;

        // Called by Selectors that require SteamVR_TrackedControllers 
        protected void SetupController()
        {
            controller = GetComponent<SteamVR_TrackedController>();
            if (controller != null)
            {
                trigger_handler = new GrabButtonHandler(this, controller);
                menu_handler = new ButtonHandler(this, controller, Button.Menu);
                padClicked_handler = new ButtonHandler(this, controller, Button.PadClicked);
                padTouched_handler = new ButtonHandler(this, controller, Button.PadTouched);
                gripped_handler = new ButtonHandler(this, controller, Button.Gripped);
            }
        }

        protected void ReleaseController()
        {
            if (controller != null)
            {
                trigger_handler = null;
                menu_handler = null;
                padClicked_handler = null;
                padTouched_handler = null;
                gripped_handler = null;
            }
        }

        public Vector3 GetVelocity()
        {
            if ( device == null )
            {
                SteamVR_TrackedObject trackedObj = GetComponent<SteamVR_TrackedObject>();
                device = SteamVR_Controller.Input((int)trackedObj.index);
            }
            if (device != null)
                return transform.parent.TransformVector( device.velocity );
            return Vector3.zero;
        }

        public Vector3 GetAngularVelocity()
        {
            if (device == null)
            {
                SteamVR_TrackedObject trackedObj = GetComponent<SteamVR_TrackedObject>();
                device = SteamVR_Controller.Input((int)trackedObj.index);
            }
            if (device != null)
                return transform.parent.TransformDirection(device.angularVelocity.normalized) * device.angularVelocity.magnitude;

            return Vector3.zero;
        }

        public void Rumble( ushort microsecs )
        {
            SteamVR_Controller.Input((int)controller.controllerIndex).TriggerHapticPulse(microsecs);
        }

        // All grabbing Selectors update their emulated cursor before notifying the vodgets of their new world location.
        protected void DoGrabbedUpdate()
        {
            trigger_handler.DoGrabbedUpdate();
        }

        protected GameObject GetVodgetObj( GameObject obj )
        {
            if ( obj == null )
            {
                //Debug.Log("GetVodgetObj: NULL");
                return null;
            }
            Transform t = obj.transform;
            while (t != null )
            {
                Vodget[] vlist = t.gameObject.GetComponents<Vodget>();
                if (vlist.Length > 0)
                    return t.gameObject;
                t = t.parent;
            }
            return null;
        }

        // Used by DirectSelector instead of DetectVodget
        // Direct selectors can focus on multiple gameobjs and must add/remove them separately.
        protected void AddFocus(GameObject c_obj)
        {
            GameObject found_focus = GetVodgetObj(c_obj);

            if (found_focus == null)
                return;

            if ( !focus_objs.Contains( found_focus ) )
            {
                focus_objs.Add(found_focus);

                if (!isGrabbing)
                {
                    Vodget[] vlist = found_focus.GetComponents<Vodget>();
                    foreach (Vodget v in vlist)
                        v.DoFocus(this, true);
                }
            }
        }

        // Used by DirectSelector instead of DetectVodget
        // Direct selectors can focus on multiple gameobjs and must add/remove them separately.
        protected void RemoveFocus(GameObject c_obj)
        {
            GameObject found_focus = GetVodgetObj(c_obj);

            if (found_focus == null)
                return;

            if (focus_objs.Remove(found_focus))
            {
                if (!isGrabbing)
                {
                    Vodget[] vlist = found_focus.GetComponents<Vodget>();
                    foreach (Vodget v in vlist)
                        v.DoFocus(this, false);
                }
            }
        }

        // Ray selectors call DetectVodget while attempting to find new vodgets.
        // Ray selectors only focus on a single gameobj at a time.
        protected void DetectVodget(GameObject obj)
        {
            GameObject found_focus = GetVodgetObj(obj);

            // The search for vodgets from any collider object always returns the same gameobject.
            if (found_focus == null || ! focus_objs.Contains(found_focus) )
            {
                // Send defocus immediately when not grabbing. 
                if (!isGrabbing)
                {
                    SetCursor();

                    foreach (GameObject f in focus_objs)
                    {
                        if (f != null)
                        {
                            Vodget[] vlist = f.GetComponents<Vodget>();
                            foreach (Vodget v in vlist)
                                v.DoFocus(this, false);
                        }
                    }
                }
                focus_objs.Clear();

                // Search up the tree to find current vodgets.
                collider_obj = obj; // This should be deprecated in favor of curr_vodgets.

                if (found_focus != null)
                {
                    focus_objs.Add(found_focus);

                    if ( ! isGrabbing )
                    {
                        // When the collider changes while not grabbing update the cursor for any events.
                        SetCursor();

                        Vodget[] vlist = found_focus.GetComponents<Vodget>();
                        foreach (Vodget v in vlist)
                            v.DoFocus(this, true);
                    }
                }

            }
        }
    }

}