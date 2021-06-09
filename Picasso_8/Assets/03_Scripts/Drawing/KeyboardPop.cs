using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRKB;

public class KeyboardPop : MonoBehaviour
{
    public GameObject PopKeyboard;
    public GameObject PlayerPos;

    private void Start()
    {
        if (PopKeyboard.activeSelf != false)
        PopKeyboard.SetActive(false);
    }

    public void KeyboardUp()
    {
        if(PopKeyboard.activeSelf != true)
        PopKeyboard.gameObject.transform.position = PlayerPos.gameObject.transform.position;
        PopKeyboard.gameObject.transform.rotation = PlayerPos.gameObject.transform.rotation;
        PopKeyboard.SetActive(true);
        return;
    }
    public void KeyboardDown()
    {
        if(PopKeyboard.activeSelf != false)
        {
            PopKeyboard.SetActive(false);
            return;
        }
    }

    public void OnKeyPress(KeyBehaviour key, Collider collider, bool autoRepeat)
    {
        OculusControllerBehaviour controller = collider.GetComponentInParent<OculusControllerBehaviour>();
        if (controller == null)
            return;

        controller.Vibrate();
    }
}
