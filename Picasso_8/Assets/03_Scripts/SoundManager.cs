using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour

{
    public static SoundManager instance;

    public enum BGM_TYPE
    {
        BGM_1,
        BGM_2,
        BGM_3,
        BGM_4,
        BGM_5,
    }

    public enum EFG_TYPE
    {
        EFT_1,
        EFT_2,
        EFT_3,
        EFT_4,
        EFT_5,
    }

    public AudioSource bgm; //BGM 플레이하는 AudioSource
    public AudioSource eft; //EFT 담당하는 AudioSource

    //bgm 파일
    public AudioClip[] bgmS;
    public AudioClip[] eftS;


    private void Awake()
    {
        instance = this;
    }
    public void PlayBGM(BGM_TYPE type)
    {
        bgm.clip = bgmS[(int)type];
        bgm.Play();
    }
    public void PlayEFT(EFG_TYPE type)
    {
        //eft.clip = eftS[(int)type];
        eft.PlayOneShot(eftS[(int)type]);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //SoundManager.instance.PlayEFT(SoundManager.EFG_TYPE.EFT_1);
            SoundManager.instance.PlayBGM(SoundManager.BGM_TYPE.BGM_1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //SoundManager.instance.PlayEFT(SoundManager.EFG_TYPE.EFT_2);
            SoundManager.instance.PlayBGM(SoundManager.BGM_TYPE.BGM_2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //SoundManager.instance.PlayEFT(SoundManager.EFG_TYPE.EFT_3);
            SoundManager.instance.PlayBGM(SoundManager.BGM_TYPE.BGM_3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            //SoundManager.instance.PlayEFT(SoundManager.EFG_TYPE.EFT_4);
            SoundManager.instance.PlayBGM(SoundManager.BGM_TYPE.BGM_4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            //SoundManager.instance.PlayEFT(SoundManager.EFG_TYPE.EFT_5);
            SoundManager.instance.PlayBGM(SoundManager.BGM_TYPE.BGM_5);
        }
    }
}