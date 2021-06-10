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

    [SerializeField]
    [Tooltip("키보드 상단의 인풋필드")]
    public TMPro.TMP_InputField KeyText; //키보드가 쓴 글씨 담는 그릇
    [Tooltip("레이로 쏜 인풋필드 적용 대상, 미리 채워두지 말것")]
    private InputField Selected; //키보드에 있는 내용을 옮길 그릇

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


    [Tooltip("왼손 이모지 칸")]
    public GameObject EmojiCanvas;
    [Tooltip("왼손 설정 칸")]
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
        //왼손 셋팅 메뉴창 시작시 끄기, 없으면 알람
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
                Debug.Log("인풋필드 발견");
                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, CurrCont))
                {
                    Debug.Log("인풋필드 선택");
                    Selected = hit.transform.GetComponent<InputField>();
                    As_UseKeyboard();
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