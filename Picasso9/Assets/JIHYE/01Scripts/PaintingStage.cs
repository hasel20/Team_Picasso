using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingStage : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        //if (other.gameObject.name.Contains("Player_Draw"))
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerDrawing pd = other.GetComponent<PlayerDrawing>();
            if (pd != null) pd.IsPainter = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            other.GetComponent<PlayerDrawing>().IsPainter = false;
        }
    }
}
