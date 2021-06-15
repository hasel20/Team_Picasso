using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectButton : MonoBehaviour
{

    [SerializeField]
    [Tooltip("키보드 상단의 인풋필드")]
    public TMPro.TMP_InputField KeyText; //키보드가 쓴 글씨 담는 그릇
    [SerializeField]
    [Tooltip("레이로 쏜 인풋필드 적용 대상, 미리 채워두지 말것")]
    private InputField Selected; //키보드에 있는 내용을 옮길 그릇

    private Transform CurrRay;
    private OVRInput.Controller CurrCont;

    [Tooltip("왼손 레이를 쏘는곳, OVRCameraRig 의 Left Hand Anchor")]
    public Transform LrayTransform;
    [Tooltip("오른손 레이를 쏘는곳, OVRCameraRig 의 Right Hand Anchor")]
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
                Debug.Log("인풋필드 발견");
                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, CurrCont))
                {
                    Debug.Log("인풋필드 선택");
                    Selected = hit.transform.GetComponent<InputField>();
                    //menuSetting.instante.As_UseKeyboard();
                    return;
                }
                else
                {
                    Debug.Log("인풋필드 발견못함");
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
