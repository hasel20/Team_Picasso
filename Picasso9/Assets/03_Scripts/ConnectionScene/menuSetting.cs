using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class menuSetting : MonoBehaviour
{
    public static menuSetting instant;

    [SerializeField]
    public bool Disable_LeftMenu;
    [SerializeField]
    public bool Disable_RightMenu;
    [SerializeField]
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

    [Tooltip("�޼� �� ĭ")]
    public GameObject canvasLU;
    [Tooltip("�޼� ���� ĭ")]
    public GameObject canvasLL;

    public int FileCounter = 0;


    private void Awake()
    {
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
        if (canvasLU != null)
        {
            if (canvasLU.activeSelf != false)
            {
                canvasLU.SetActive(false);
            }
        }
        else
        {
            print("Emoji Canvas is 'null'");
            return;
        }
        //�޼� ���� �޴�â ���۽� ����, ������ �˶�
        if (canvasLL != null)
        {
            if (canvasLL.activeSelf != false)
            {
                canvasLL.SetActive(false);
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
    }

    void LeftMenuSetting()
    {
        if (OVRInput.GetDown(LeftMenuButton, OVRInput.Controller.LTouch))
        {
            if (LeftMenu == null)
            {
                return;
            }
            else
            {
                if (!Disable_LeftMenu)
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
            if (RightMenu == null)
            {
                return;
            }
            else
            {
                if (!Disable_RightMenu)
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
            if (KeyBoard == null)
            {
                return;
            }
            else
            {
                if (!Disable_Keyboard)
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
        if (canvasLU != null)
        {
            if (canvasLU.activeSelf != true)
            {
                canvasLU.SetActive(true);
                if (canvasLL.activeSelf)
                {
                    canvasLL.SetActive(false);
                }
                return;
            }
            if (canvasLU.activeSelf != false)
            {
                canvasLU.SetActive(false);
                return;
            }
        }
        else { return; }
    }

    public void LMenu_Right() //Capture
    {
        Capture();
    }

    public void LMenu_Down()
    {
    }

    public void LMenu_Left() //Settings
    {
        if (canvasLL == null)
        {
            return;
        }
        else
        {
            if (canvasLL.activeSelf != true)
            {
                canvasLL.SetActive(true);
                if (canvasLU.activeSelf)
                {
                    canvasLU.SetActive(false);
                }
                return;
            }
            if (canvasLL.activeSelf != false)
            {
                canvasLL.SetActive(false);
                return;
            }
        }
    }

    public void RMenu_Up() //QuickConnect
    {
        ConnectionManager.instant.OnQuickConnect();
    }

    public void RMenu_Right() //
    {
    }

    public void RMenu_Down() //Quit
    {
        Application.Quit();
    }
    
    public void RMenu_Left() //
    {
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