using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoleSet : MonoBehaviourPun
{
    public Text alim;

    public TMPro.TMP_InputField KeyText; //키보드가 쓴 글씨 담는 그릇
    public Text answer;
    //Text otherAnswer;
    public GameObject keybord;
    public GameObject malletL;
    public GameObject malletR;

    bool key;
    public enum Role
    {
        answerer,
        painter
    }
    public Role role = Role.answerer;

    void Start()
    {
        GameManager.instance.AddPlayer(this.gameObject);        
    }

    void Update()
    {
        //내꺼일떄 
        if (photonView.IsMine)
        {
            AlimText();
            //관중이면!
            if (role == Role.answerer)
            {
                KeybordSet();
                //photonView.RPC("KeybordSet",RpcTarget.All);
            }
            //else if (role == Role.painter)
            //{
            //    key = false;
            //    keybord.SetActive(false);
            //    malletL.SetActive(false);
            //    malletR.SetActive(false);
            //}
        }

    }
    void AlimText()
    {
        if (alim != null)
        {
            if (role == Role.painter)
            {
                List<string> que = GameManager.instance.gameQuestion;
                alim.text = "[ " + que[Random.Range(0, que.Count)] + " ] 그림을 그려보세요!";
                alim.color = Color.blue;
            }
            else
            {
                alim.text = "그림을 맞춰봐요!";
                alim.color = Color.black;
            }
        }
    }

    //[PunRPC]
    void KeybordSet()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two,OVRInput.Controller.LTouch))
        {
            key = !key;
        }
        keybord.SetActive(key);
        malletL.SetActive(key);
        malletR.SetActive(key);
    }


    //public void SetChat(string an)
    //{
    //    answer.text = an;
    //}

    public void Typing()
    {
        KeyText.text = KeyText.text.Substring(0, KeyText.text.Length - 1);
        photonView.RPC("Ping",RpcTarget.All, KeyText.text);
    }

    [PunRPC]
    void Ping(string plz)
    {
        answer.text = plz;
    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsWriting)
    //    {
    //        stream.SendNext(answer.text);
    //    }

    //    if (stream.IsReading)
    //    {
    //        otherAnswer.text = (string)stream.ReceiveNext();
    //    }
    //}

    



}
