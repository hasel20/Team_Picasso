using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class kbSetting : MonoBehaviour
{
    public GameObject KeyBoard;
    public GameObject Mallet1;
    public GameObject Mallet2;
    InputField Selected;

    [Tooltip("Object which points with Z axis. E.g. CentreEyeAnchor from OVRCameraRig")]
    public Transform rayTransform;

    [Tooltip("Gamepad button to act as gaze click")]
    public OVRInput.Button joyPadClickButton;

    private void Start()
    {
        KeyBoard.SetActive(false);
        Mallet1.SetActive(false);
        Mallet2.SetActive(false);
    }
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            print("False�� ���� Ŭ�� ��");
            if (Mallet1.activeSelf == false) return;
            Mallet1.SetActive(false);
            Mallet2.SetActive(false);
            print("False�� ���� Ŭ�� ��");

            KeyBoard.SetActive(false);
            print("Ű���� False");
        }
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch))
        {
            print("True�� ���� Ŭ�� ��");
            if (Mallet1.activeSelf == true) return;
            Mallet1.SetActive(true);
            Mallet2.SetActive(true);
            print("True�� ���� Ŭ�� ��");

            KeyBoard.SetActive(true);
            print("Ű���� True");
        }
    }

    public void As_UnuseKeyboard()
    {
        if (KeyBoard.activeSelf == false) return;
        KeyBoard.SetActive(false);
        Mallet1.SetActive(false);
        Mallet2.SetActive(false);
    }
    public void As_UseKeyboard()
    {
        if (KeyBoard.activeSelf == true) return;
        KeyBoard.SetActive(true);
        Mallet1.SetActive(true);
        Mallet2.SetActive(true);
    }

    public void FindField()
    {
        Ray ray = new Ray(rayTransform.position, rayTransform.position + rayTransform.forward);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("inputField"))
            {
                Debug.Log("��ǲ�ʵ� �߰�");
                Selected = hit.transform.GetComponent<InputField>();
            }
            else
            {
                Debug.Log("��ǲ�ʵ� �ƴ�");
            }
        }
    }
}