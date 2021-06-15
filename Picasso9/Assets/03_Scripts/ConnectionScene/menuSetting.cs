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
    [Tooltip("왼손에 붙이는 메뉴 캔버스")]
    public GameObject LeftMenu; //왼손 메뉴창
    [Tooltip("오른손에 붙이는 메뉴 캔버스")]
    public GameObject RightMenu; //오른손 메뉴창

    [Tooltip("키보드 Prefabs")]
    public GameObject KeyBoard;
    [Tooltip("키보드를 치는 Mallet 봉1")]
    public GameObject Mallet1;
    [Tooltip("키보드를 치는 Mallet 봉2")]
    public GameObject Mallet2;

    [Tooltip("사진 캡처를 위한 CenterEye")]
    public Camera CenterEye;
    [Tooltip("왼손 레이를 쏘는곳, OVRCameraRig 의 Left Hand Anchor")]
    public Transform LrayTransform;
    [Tooltip("오른손 레이를 쏘는곳, OVRCameraRig 의 Right Hand Anchor")]
    public Transform RrayTransform;

    private Transform CurrRay;
    private OVRInput.Controller CurrCont;

    [Tooltip("왼손 메뉴를 위한 게임버튼 Left Touch 기준")] //어떤 버튼 누를지 외부에서 설정
    public OVRInput.Button LeftMenuButton;
    [Tooltip("오른손 메뉴를 위한 게임버튼 Right Touch 기준")]
    public OVRInput.Button RightMenuButton;
    [Tooltip("키보드를 띄우기 위한 버튼, Left Touch 기준")]
    public OVRInput.Button KeyboardButton;

    [Tooltip("왼손 윗 칸")]
    public GameObject canvasLU;
    [Tooltip("왼손 설정 칸")]
    public GameObject canvasLL;

    public int FileCounter = 0;


    private void Awake()
    {
        if (CurrRay == null) { CurrRay = RrayTransform; }
        CurrCont = OVRInput.Controller.RTouch;
    }

    void Start()
    {
        //왼손 메뉴창 시작시 끄기, 없으면 알람
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
        //오른손 메뉴창 시작시 끄기, 없으면 알람
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
        //왼손 이모지 메뉴창 시작시 끄기, 없으면 알람
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
        //왼손 셋팅 메뉴창 시작시 끄기, 없으면 알람
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
        //키보드 및 누름봉 시작시 끄기
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