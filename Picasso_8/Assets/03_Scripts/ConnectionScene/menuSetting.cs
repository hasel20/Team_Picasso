using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class menuSetting : MonoBehaviour
{
    public bool Disable_LeftMenu;
    public bool Disable_RightMenu;
    public bool Disable_Keyboard;
    [Tooltip("�޼տ� ���̴� �޴� ĵ����")]
    public GameObject LeftMenu; //�޼� �޴�â
    [Tooltip("�����տ� ���̴� �޴� ĵ����")]
    public GameObject RightMenu; //������ �޴�â

    [Tooltip("Ű���� Prefabs")]
    public GameObject KeyBoard;
    [Tooltip("Ű���带 ġ�� Mallet ��1")]
    public GameObject Mallet1;
    [Tooltip("Ű���带 ġ�� Mallet ��2")]
    public GameObject Mallet2;

    [SerializeField]
    [Tooltip("Ű���� ����� ��ǲ�ʵ�")]
    public TMPro.TMP_InputField KeyText; //Ű���尡 �� �۾� ��� �׸�
    [Tooltip("���̷� �� ��ǲ�ʵ� ���� ���, �̸� ä������ ����")]
    private InputField Selected; //Ű���忡 �ִ� ������ �ű� �׸�

    [Tooltip("���� ĸó�� ���� CenterEye")]
    public Camera CenterEye;
    [Tooltip("�޼� ���̸� ��°�, OVRCameraRig �� Left Hand Anchor")]
    public Transform LrayTransform;
    [Tooltip("������ ���̸� ��°�, OVRCameraRig �� Right Hand Anchor")]
    public Transform RrayTransform;

    private Transform CurrRay;
    private OVRInput.Controller CurrCont;

    [Tooltip("�޼� �޴��� ���� ���ӹ�ư Left Touch ����")] //� ��ư ������ �ܺο��� ����
    public OVRInput.Button LeftMenuButton;
    [Tooltip("������ �޴��� ���� ���ӹ�ư Right Touch ����")]
    public OVRInput.Button RightMenuButton;
    [Tooltip("Ű���带 ���� ���� ��ư, Left Touch ����")]
    public OVRInput.Button KeyboardButton;


    [Tooltip("�޼� �̸��� ĭ")]
    public GameObject EmojiCanvas;
    [Tooltip("�޼� ���� ĭ")]
    public GameObject SettingCanvas;

    public int FileCounter = 0;
    

    private void Awake()
    {
        Selected = null;
        if (CurrRay == null) { CurrRay = RrayTransform; }
        CurrCont = OVRInput.Controller.RTouch;
    }

    void Start()
    {
        //�޼� �޴�â ���۽� ����, ������ �˶�
        if (LeftMenu != null)
        {
            if (LeftMenu.activeSelf != false)
            {
                LeftMenu.SetActive(false);
            }
        }
        else
        {
            print("LeftMenu hand menu Canvs is 'null'");
            return;
        }
        //������ �޴�â ���۽� ����, ������ �˶�
        if (RightMenu != null)
        {
            if (RightMenu.activeSelf != false)
            {
                RightMenu.SetActive(false);
            }
        }
        else
        {
            print("Right hand menu Canvs is 'null'");
            return;
        }
        //�޼� �̸��� �޴�â ���۽� ����, ������ �˶�
        if (EmojiCanvas != null)
        {
            if (EmojiCanvas.activeSelf != false)
            {
                EmojiCanvas.SetActive(false);
            }
        }
        else
        {
            print("Emoji Canvas is 'null'");
            return;
        }
        //�޼� ���� �޴�â ���۽� ����, ������ �˶�
        if (SettingCanvas != null)
        {
            if (SettingCanvas.activeSelf != false)
            {
                SettingCanvas.SetActive(false);
            }
        }
        else
        {
            print("Setting Canvas is 'null'");
            return;
        }
        //Ű���� �� ������ ���۽� ����
            if (KeyBoard.activeSelf != false) KeyBoard.SetActive(false);
            if (Mallet1.activeSelf != false) Mallet1.SetActive(false);
            if (Mallet2.activeSelf != false) Mallet2.SetActive(false);
    }
    void Update()
    {
        LeftMenuSetting();
        RightMenuSetting();
        KeyBoardSetting();
        FindField();
    }

    void LeftMenuSetting()
    {
        if (OVRInput.GetDown(LeftMenuButton, OVRInput.Controller.LTouch))
        {
            if(LeftMenu == null)
            {
                return;
            }
            else
            {
                if (Disable_LeftMenu)
                {
                    if (RrayTransform != null)
                    {
                        CurrRay = RrayTransform;
                        CurrCont = OVRInput.Controller.RTouch;
                    }

                    if (LeftMenu.activeSelf != true)
                    {
                        LeftMenu.SetActive(true);
                        return;
                    }
                    else if (LeftMenu.activeSelf != false)
                    {
                        LeftMenu.SetActive(false);
                        return;
                    }
                }
            }            
        }
    }
    void RightMenuSetting()
    {
        if (OVRInput.GetDown(RightMenuButton, OVRInput.Controller.RTouch))
        {
            if(RightMenu == null)
            {
                return;
            }
            else
            {
                if (Disable_RightMenu)
                {
                    if (RrayTransform != null)
                    {
                        CurrRay = LrayTransform;
                        CurrCont = OVRInput.Controller.LTouch;
                    }

                    if (RightMenu == null)
                    {
                        return;
                    }

                    if (RightMenu.activeSelf != true)
                    {
                        RightMenu.SetActive(true);
                        return;
                    }
                    else if (RightMenu.activeSelf != false)
                    {
                        RightMenu.SetActive(false);
                        return;
                    }
                }
            }            
        }
    }

    void KeyBoardSetting()
    {
        if (OVRInput.GetDown(KeyboardButton, OVRInput.Controller.LTouch))
        {
            if(KeyBoard == null)
            {
                return;
            }
            else
            {
                if (Disable_Keyboard)
                {
                    return;
                }
                else
                {
                    if (KeyBoard.activeSelf != true)
                    {
                        KeyBoard.SetActive(true);
                        Mallet1.SetActive(true);
                        Mallet2.SetActive(true);
                        return;
                    }
                    if (KeyBoard.activeSelf != false)
                    {
                        KeyBoard.SetActive(false);
                        Mallet1.SetActive(false);
                        Mallet2.SetActive(false);
                        return;
                    }
                }
            }
        }
    }

    public void LMenu_Up() //Emoji
    {
        if (EmojiCanvas != null)
        {
            if (EmojiCanvas.activeSelf != true)
            {
                EmojiCanvas.SetActive(true);
                return;
            }
            if (EmojiCanvas.activeSelf != false)
            {
                EmojiCanvas.SetActive(false);
                return;
            }
        }
        else { return; }
    }

    public void LMenu_Right() //Capture
    {
        Capture();
    }

    public void LMenu_Down() //Quit
    {
        Application.Quit();
    }

    public void LMenu_Left() //Settings
    {
        if (SettingCanvas == null)
        {
            return;
        }
        else 
        {
            if (SettingCanvas.activeSelf != true)
            {
                SettingCanvas.SetActive(true);
                return;
            }
            if (SettingCanvas.activeSelf != false)
            {
                SettingCanvas.SetActive(false);
                return;
            }
        }
    }

    public void As_UseKeyboard()
    {
        if (KeyBoard.activeSelf != true)
        {
            StartCoroutine(UseKeyboard(0.5f));
            return;
        }
    }

    public void As_UnUseKeyboard()
    {
        if (KeyBoard.activeSelf != false)
        {
            StartCoroutine(UnUseKeyboard(0.5f));
            return;
        }
    }

    public void FindField()
    {
        Ray ray = new Ray(CurrRay.position, CurrRay.forward);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("inputField"))
            {
                Debug.Log("��ǲ�ʵ� �߰�");
                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, CurrCont))
                {
                    Debug.Log("��ǲ�ʵ� ����");
                    Selected = hit.transform.GetComponent<InputField>();
                    As_UseKeyboard();
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

    void Capture()
    {
        CenterEye = GetComponent<Camera>();

        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = CenterEye.targetTexture;

        CenterEye.Render();

        Texture2D Image = new Texture2D(CenterEye.targetTexture.width, CenterEye.targetTexture.height);
        Image.ReadPixels(new Rect(0, 0, CenterEye.targetTexture.width, CenterEye.targetTexture.height), 0, 0);
        Image.Apply();
        RenderTexture.active = currentRT;

        var Bytes = Image.EncodeToPNG();
        Destroy(Image);

        File.WriteAllBytes(Application.dataPath + "/Backgrounds/" + FileCounter + ".png", Bytes);
        FileCounter++;
    }

    IEnumerator UseKeyboard(float t)
    {
        yield return new WaitForSeconds(t);
        KeyBoard.SetActive(true);
        Mallet1.SetActive(true);
        Mallet2.SetActive(true);
    }
    IEnumerator UnUseKeyboard(float t)
    {
        yield return new WaitForSeconds(t);
        KeyBoard.SetActive(false);
        Mallet1.SetActive(false);
        Mallet2.SetActive(false);
    }
}