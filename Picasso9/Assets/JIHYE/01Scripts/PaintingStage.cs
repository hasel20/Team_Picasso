using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingStage : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.Contains("Player_Draw"))
        {
            other.GetComponent<PlayerDrawing>().IsPainter = true;
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
