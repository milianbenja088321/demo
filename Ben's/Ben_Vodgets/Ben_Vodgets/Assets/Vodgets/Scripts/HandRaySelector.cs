using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vodgets {

    [RequireComponent(typeof(SteamVR_TrackedController))]

    public class HandRaySelector : Selector
    {
        Vector3 grabpos = Vector3.zero;
        static Object cursor_prefab = null;
        GameObject cursor_obj = null;
        public float ray_length = 10f;

        protected override void SetCursor()
        {
            cursor.localPosition = transform.TransformPoint(grabpos);
            cursor.localRotation = transform.rotation;
        }

        private void OnEnable()
        {
            SetupController();

            if ( cursor_prefab == null )
                cursor_prefab = Resources.Load("cursor_blue_ray");
            if (cursor_prefab != null)
            {
                cursor_obj = (GameObject)GameObject.Instantiate(cursor_prefab);
                cursor_obj.transform.SetParent(transform, false);
                cursor_obj.transform.localPosition = Vector3.zero;
                cursor_obj.transform.localRotation = Quaternion.identity;
            }
        }

        private void OnDisable()
        {
            ReleaseController();

            if (cursor_obj != null)
                Destroy(cursor_obj);
        }

        private void FixedUpdate()
        {
            if (isGrabbing)
            {
                DoGrabbedUpdate();
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, 10))
                {
                    grabpos = transform.InverseTransformPoint(hit.point);
                    DetectVodget(hit.transform.gameObject);
                }
                else
                {
                    DetectVodget(null);
                }
            }
        }
    }
}
