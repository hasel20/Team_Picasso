using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveObj : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        Erase();
    }

    void Erase()
    {
        if (!OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            Collider[] coll = Physics.OverlapSphere(transform.position, 0.1f);
            if (coll.Length > 0)
            {
                if (OVRInput.GetDown(OVRInput.Button.Two,OVRInput.Controller.RTouch)&&
                    coll[0].gameObject.layer != LayerMask.NameToLayer("Player"))
                { 
                    Destroy(coll[0].gameObject);
                }
            }            
        }
    }
}
