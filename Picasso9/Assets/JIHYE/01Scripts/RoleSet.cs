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

    //public int playerNumber;//플레이어 구분은 PhotonViewID로 구분하기떄ㅜㄴ에 별도의 인덱스 불필요하다, 

    void Start()
    {
        //playerNumber = GameManager.instance.AddPlayer(this.gameObject);
        GameManager.instance.AddPlayer(this.gameObject);
        role = Role.answerer;
        alim.text = "기다려 주세용";
        alim.color = Color.black;
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
                photonView.RPC("Keyonoff", RpcTarget.All, false); // 문제내는 사람은 키보드 바로 꺼지게 만들기! 
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



    void KeybordSet()//내꺼 일떄만 조종하는 함수,, 아니,,, 뭔지 모륵ㅆ다,
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

    //키보드 센드누르면 진행하는 함수 ? 
    public void Typing()
    {
        KeyText.text = KeyText.text.Substring(0, KeyText.text.Length - 1);
        photonView.RPC("RpcTyping", RpcTarget.All, KeyText.text);
        if (GameManager.instance.ChackAnswer(KeyText.text))//정답일때
        {
            int painterID = GameManager.instance.WhoPainter();
            int answerID = this.gameObject.GetComponent<PhotonView>().ViewID;
            photonView.RPC("OKanswer", RpcTarget.All,painterID,answerID);
            GameManager.instance.ResetTurn();
        }
    }
    [PunRPC]
    void RpcTyping(string plz)
    {
        //모두에게 답을 알리는 함수 
        answer.text = plz;
    }

    [PunRPC]//모두에게 득점자를 알려주는 함수
    void OKanswer(int painterID,int answerID)
    {
        if (painterID != -10)
        {
            GameObject paints = GameManager.instance.GetPlayer(painterID);
            GameObject answer = GameManager.instance.GetPlayer(answerID);

            if(paints!=null) paints.GetComponent<PlayerScore>().AddScore(2);
            if(answer != null) answer.GetComponent<PlayerScore>().AddScore(1);

            print("그림쟁이 : " + painterID + ", 정답자 : " + answerID + "정답축하~ !");
        }
        else print("그램쟁이 업숴.");
    }

}
