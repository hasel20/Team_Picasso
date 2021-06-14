using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoleSet : MonoBehaviourPun
{
    public Text alim;

    public TMPro.TMP_InputField KeyText; //Ű���尡 �� �۾� ��� �׸�
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
        //�����ϋ� 
        if (photonView.IsMine)
        {
            //�����̸�!
            if (role == Role.answerer)
            {
                KeybordSet();
            }
            else//painter�̸� ä��ġ�� �ȴ�! 
            {
                photonView.RPC("Keyonoff", RpcTarget.All, false);
            }
        }

    }
    
    
    //���� �� �ٲ���. ���� ���� ���ִ� �Լ� 
    public void P_RoleAlim(string ques)
    {
        alim.text = "[ " + ques + " ] �׸��� �׷�������!";
        alim.color = Color.blue;
    }
    public void A_RoleAlim()
    {
        role = Role.answerer;
        alim.text = "�׸��� �������!";
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

    //Ű���� ���崩���� ���Ӥ����ϴ� �Լ� ? 
    public void Typing()
    {
        KeyText.text = KeyText.text.Substring(0, KeyText.text.Length - 1);
        photonView.RPC("RpcTyping", RpcTarget.All, KeyText.text);
        if (GameManager.instance.ChackAnswer(KeyText.text))//�����϶�
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

            print("�׸����� : "+painterID +", ������ : " + answerID + "��������~ !");
        }
        else print("�׷����� ����.");
    }

    [PunRPC]
    void RpcTyping(string plz)
    {
        //��ο��� ���� �˸��� �Լ� 
        answer.text = plz;
    }
}
