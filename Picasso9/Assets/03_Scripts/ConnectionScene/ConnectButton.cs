using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectButton : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Ű���� ����� ��ǲ�ʵ�")]
    public TMPro.TMP_InputField KeyText; //Ű���尡 �� �۾� ��� �׸�
    [SerializeField]
    [Tooltip("���̷� �� ��ǲ�ʵ� ���� ���, �̸� ä������ ����")]
    private InputField Selected; //Ű���忡 �ִ� ������ �ű� �׸�

    private Transform CurrRay;
    private OVRInput.Controller CurrCont;

    [Tooltip("�޼� ���̸� ��°�, OVRCameraRig �� Left Hand Anchor")]
    public Transform LrayTransform;
    [Tooltip("������ ���̸� ��°�, OVRCameraRig �� Right Hand Anchor")]
    public Transform RrayTransform;

    private void Awake()
    {
        Selected = null;
    }

    private void Start()
    {
        if (CurrRay == null)
        {
            CurrRay = LrayTransform;
            CurrCont = OVRInput.Controller.LTouch;
        }
    }

    public void FindField()
    {
        Ray ray = new Ray(CurrRay.position, CurrRay.forward);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.gameObject.tag == ("inputField"))
            {
                Debug.Log("��ǲ�ʵ� �߰�");
                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, CurrCont))
                {
                    Debug.Log("��ǲ�ʵ� ����");
                    Selected = hit.transform.GetComponent<InputField>();
                    //menuSetting.instante.As_UseKeyboard();
                    return;
                }
                else
                {
                    Debug.Log("��ǲ�ʵ� �߰߸���");
                    return;
                }
            }
        }
    }

    public void SendMessage()
    {
        if (Selected == null) return;

        KeyText.text = KeyText.text.Substring(0, KeyText.text.Length - 1);
        Selected.text = KeyText.text;
        return;
    }

    void ChangeRay()
    {
        if (RrayTransform != null)
        {
            CurrRay = LrayTransform;
            CurrCont = OVRInput.Controller.LTouch;
        }
    }
}
