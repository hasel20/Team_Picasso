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

    public int playerNumber;

    void Start()
    {
        //playerNumber = GameManager.instance.AddPlayer(this.gameObject);
        GameManager.instance.AddPlayer(this.gameObject);
        A_RoleAlim();
    }

    void Update()
    {
        //내꺼일떄 
        if (photonView.IsMine)
        {
            //관중이면!
            if (role == Role.answerer)
            {
                KeybordSet();
            }
            else//painter이면 채팅치면 안댐! 
            {
                photonView.RPC("Keyonoff", RpcTarget.All, false);
            }
        }

    }
    
    
    //게임 롤 바뀔떄. 문구 지정 해주느 함수 
    public void P_RoleAlim(string ques)
    {
        alim.text = "[ " + ques + " ] 그림을 그려보세요!";
        alim.color = Color.blue;
    }
    public void A_RoleAlim()
    {
        role = Role.answerer;
        alim.text = "그림을 맞춰봐요!";
        alim.color = Color.black;
    }



    void KeybordSet()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch))
        {
            key = !key;
        }
        photonView.RPC("Keyonoff",RpcTarget.All,key);
    }
    [PunRPC]
    void Keyonoff(bool ke)
    {
        keybord.SetActive(ke);
        malletL.SetActive(ke);
        malletR.SetActive(ke);        
    }

    //키보드 센드누르면 지ㅣㄴ행하는 함수 ? 
    public void Typing()
    {
        KeyText.text = KeyText.text.Substring(0, KeyText.text.Length - 1);
        photonView.RPC("RpcTyping", RpcTarget.All, KeyText.text);
        if (GameManager.instance.ChackAnswer(KeyText.text))//정답일때
        {
            int painterID = GameManager.instance.WhoPainter();
            int answerID = this.gameObject.GetComponent<PhotonView>().ViewID;
            photonView.RPC("OKan",RpcTarget.All,painterID,answerID);
            GameManager.instance.ResetTurn();
        }
    }

    [PunRPC]
    void OKan(int painterID,int answerID)
    {
        if (painterID != -10)
        {
            GameObject paints = GameManager.instance.GetPlayer(painterID);
            GameObject answer = GameManager.instance.GetPlayer(answerID);

            paints.GetComponent<PlayerScore>().AddScore(2);
            answer.GetComponent<PlayerScore>().AddScore(1);

            print("그림쟁이 : "+painterID +", 정답자 : " + answerID + "정답축하~ !");
        }
        else print("그램쟁이 업숴.");
    }

    [PunRPC]
    void RpcTyping(string plz)
    {
        //모두에게 답을 알리는 함수 
        answer.text = plz;
    }
}
