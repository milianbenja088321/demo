using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vodgets
{
    public class ButtonBehavior : Vodget
    {
        public override void DoGrab(Selector cursor, bool state)
        {
            base.DoGrab(cursor, state);
            if (state)
            {
                // do code for the rest of the stuff...
                this.gameObject.GetComponentInChildren<UnityEngine.UI.Button>().onClick.Invoke();

            }
        }

        public override void DoFocus(Selector cursor, bool state)
        {
            base.DoFocus(cursor, state);
            if (state)
            {
                Debug.Log("Focusing On: " + this.gameObject.GetComponentInChildren<Selectable>().name);
                this.gameObject.GetComponentInChildren<Text>().color = Color.red;
            }
            else
                this.gameObject.GetComponentInChildren<Text>().color = Color.white;
            
        }
    }
}