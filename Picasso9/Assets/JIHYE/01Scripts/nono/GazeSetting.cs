using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
public class GazeSetting : MonoBehaviourPun
{
    OVRInputModule oim;
    public EventSystem ev;
    public Transform rhand;
    void Start()
    {
        oim = ev.GetComponent<OVRInputModule>();
        if(photonView.IsMine) oim.rayTransform = rhand;
    }

}
