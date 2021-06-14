using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBall : MonoBehaviour
{
    public GameObject BasketBall;
    public Transform BasketBall_SpawnPoint;
    public OVRInput.Button SpawnButton;

    private void Update()
    {
        if(OVRInput.GetDown(SpawnButton, OVRInput.Controller.RTouch))
        {
            Instantiate(BasketBall).transform.position = BasketBall_SpawnPoint.position;
        }        
    }
}
