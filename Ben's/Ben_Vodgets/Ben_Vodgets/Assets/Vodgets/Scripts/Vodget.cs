using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if USING_HIGHLIGHTING
using HighlightingSystem;
#endif

namespace Vodgets
{
    public class Vodget : MonoBehaviour
    {
        //[System.Serializable]
        public enum NetworkMode { Standalone, NetworkOnGrab, NetworkOnFocus };

        public NetworkMode networkMode = NetworkMode.Standalone;

        protected BoolEvent onGrab = new BoolEvent();
        protected BoolEvent onFocus = new BoolEvent();

        protected void RegisterState( BoolEvent evt, UnityAction<bool> call)
        {
            if ( networkMode == NetworkMode.Standalone )
            {
                evt.AddListener(call);
            }
 #if USING_PHOTON
           else
            {
                PhotonBool photon = gameObject.AddComponent<PhotonBool>();
                photon.onChanged.AddListener(call);
                evt.AddListener(photon.Change);
                if (networkMode == NetworkMode.NetworkOnGrab)
                    onGrab.AddListener(photon.ChangeOwnership);
                else
                    onFocus.AddListener(photon.ChangeOwnership);
            }
#endif
        }

        protected void RegisterState( Vector3Event evt, UnityAction<Vector3> call)
        {
            if (networkMode == NetworkMode.Standalone)
            {
                evt.AddListener(call);
            }
#if USING_PHOTON
            else
            {
                PhotonVector3 photon = gameObject.AddComponent<PhotonVector3>();
                photon.onChanged.AddListener(call);
                evt.AddListener(photon.Change);
                
                if (networkMode == NetworkMode.NetworkOnGrab)
                    onGrab.AddListener(photon.ChangeOwnership);
                else
                    onFocus.AddListener(photon.ChangeOwnership);
            }
#endif
        }

        public virtual void DoFocus(Selector cursor, bool state)
        {
            // OVERRIDE THIS VIRTUALLY to allow vodgets to highlight before DoGrab.
            if (state)
            {
#if USING_HIGHLIGHTING
                Highlighter h = transform.gameObject.GetComponent<Highlighter>();
                if (h != null)
                    h.ConstantOn(0.1f);
#endif
                cursor.Rumble(3999);
            }
            else
            {
#if USING_HIGHLIGHTING
                Highlighter h = transform.gameObject.GetComponent<Highlighter>();
                if (h != null)
                    h.ConstantOff(0.1f);
#endif
            }

            if ( onFocus != null )
                onFocus.Invoke(state);
        }

        public virtual void DoButton(Selector cursor, Selector.Button button, bool state )
        {

        }

        public virtual void DoGrab(Selector cursor, bool state)
        {
            // OVERRIDE THIS VIRTUALLY 
            if ( onGrab != null )
                onGrab.Invoke(state);
        }

        public virtual void DoUpdate(Selector cursor)
        {
            // OVERRIDE THIS VIRTUALLY 
        }
    }
}