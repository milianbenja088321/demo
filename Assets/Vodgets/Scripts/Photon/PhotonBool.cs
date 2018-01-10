using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Reflection;

namespace Vodgets
{
    public class PhotonBool : PhotonState
    {

        public BoolEvent onChanged = new BoolEvent();

        public void Change(bool v)
        {
#if USING_PHOTON
            // Check for special case where photon is not active.
            if (! this.photonView || !PhotonNetwork.connected )
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
        public void RpcChange(bool v)
        {
            // < Send rpc state change here >
            onChanged.Invoke(v);
        }

    }
}