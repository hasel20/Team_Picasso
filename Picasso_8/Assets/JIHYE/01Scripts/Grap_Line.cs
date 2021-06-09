using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grap_Line : MonoBehaviour
{
    LineRenderer lr;
    GameObject pick;
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 0;
    }

    void Update()
    {
        draw_line();
        Pick_ON();
    }

    void draw_line()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            lr.positionCount = 2;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, transform.position + transform.forward * 3);
        }
        else if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch))
        {
            lr.positionCount = 0;
        }
    }
    void Pick_ON()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch)&&
            OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.SphereCast(ray, 0.5f, out hit, 100))
            {
                pick = hit.transform.gameObject;
                pick.transform.SetParent(transform);
            }
        }
        if (OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.LTouch))
        {
            if (pick != null)
            {
                Vector2 pedL = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
                Vector3 dir = new Vector3(pedL.x, 0, pedL.y);

                pick.transform.Translate(dir * 3 * Time.deltaTime);
            }
        }
        if (OVRInput.GetUp(OVRInput.Button.Two, OVRInput.Controller.LTouch))
        {
            if(pick != null)
            {
                pick.transform.parent = null;
                pick = null;
            }
        }
    }
}
