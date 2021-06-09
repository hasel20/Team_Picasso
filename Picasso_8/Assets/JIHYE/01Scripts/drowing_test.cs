using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drowing_test : MonoBehaviour
{
    public GameObject sphere;
    Material mat;


    public Transform rHand;
    List<Vector3> points = new List<Vector3>();
    void Start()
    {
        
    }

    void Update()
    {
        Drow();
    }

     void Drow()
     {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            GameObject one = Instantiate(sphere);
            mat = one.GetComponent<Material>();
            //Set_Line(lr);

        }
        else if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            Vector3 pos = rHand.position;
            points.Add(pos);
            if (Vector3.Distance(points[points.Count - 1], pos) > 0.01f)
            {
                points.Add(pos);
                GameObject one = Instantiate(sphere);
                one.transform.position = points[points.Count-1];
            }
        }
        else if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            points.Clear();
            mat = null;
        }
     }
}
