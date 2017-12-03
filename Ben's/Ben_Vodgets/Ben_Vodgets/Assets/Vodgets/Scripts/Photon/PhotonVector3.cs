using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Reflection;

namespace Vodgets
{
    public class PhotonVector3 : PhotonState
    {
        public Vector3Event onChanged = new Vector3Event();

        public void Change(Vector3 v)
        {
#if USING_PHOTON
            // Check for special case where photon is not active.
            if (!this.photonView || !PhotonNetwork.connected)
            {
                RpcChange(v);
                return;
            }

            if (!this.photonView.isMine)
                return;

            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("RpcChange", PhotonTargets.All, v);
#else
            RpcChange(v);
#endif
        }

#if USING_PHOTON
        [PunRPC]
#endif
        public void RpcChange( Vector3 v )
        {
            // < Send rpc state change here >
            onChanged.Invoke(v);
        }

    }
}