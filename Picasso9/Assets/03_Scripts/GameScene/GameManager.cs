using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPun
{
    public static GameManager instance;

    public List<string> gameQuestion;
     public string nowQ;

    public List<GameObject> players = new List<GameObject>();
   // public List<GameObject> player_rpc = new List<GameObject>();
    RoleSet rs;

    public List<LineInfo> Lines = new List<LineInfo>();

    //�� Player�� ����
    public GameObject Master;

    bool turnOver = true;
    int index;
    int qindex;

    private void Awake()
    {
        if (this != null) instance = this;

        Master = PhotonNetwork.Instantiate("Player_Draw", new Vector3(0, 1.5f, 0), Quaternion.identity); //�� Player ����
        
    }
    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //������ ���� player�� �������� ������ �Ѹ� �׸� �׸��� �׸� �� �׸��� ��!
            if (turnOver && players.Count >= 2)
            {
                turnOver = false;
                print("start turn");
                //Randoms(Random.Range(0, players.Count));
                int a = Random.Range(0, players.Count);
                print(a);
                photonView.RPC("Randoms", RpcTarget.All, 
                    players[a].GetComponent<PhotonView>().ViewID, Random.Range(0, gameQuestion.Count));
            }
        }
    }

    
    [PunRPC]
    void Randoms(int randoms,int aa)
    {
        if(rs != null)
        {
            rs.A_RoleAlim();
            rs = null;
        }

        index = randoms ;

        nowQ = gameQuestion[aa];

        StartCoroutine(SettingRole());
    }

    IEnumerator SettingRole()
    {
        rs = GetPlayer(index).GetComponent<RoleSet>();
        rs.role = RoleSet.Role.painter;
        //nowQ = gameQuestion[Random.Range(0,gameQuestion.Count)];
        //photonView.RPC("nowQs", RpcTarget.All, gameQuestion[qindex]);
        rs.P_RoleAlim(nowQ);
        yield return new WaitForSeconds(3);
        
        turnOver = true;
    }

    //[PunRPC]
    //void nowQs(string q)
    //{
    //    nowQ = q;
    //}
    //������ ������ ����Ʈ�� �ϰ� �� ���� �˷��ִ� �Լ�
    public int AddLine(LineInfo line)
    {
        Lines.Add(line);
        return Lines.Count;
    }

    //���� ����Ʈ���� �ε����� ���ؼ� ���ϴ� ���� ã���ִ� �Լ�.
    public GameObject GetLine(int lineIdx)
    {
        for (int i = 0; i < Lines.Count; i++)
        {
            if (Lines[i].number == lineIdx)
            {
                return Lines[i].gameObject;
            }
        }
        return null;
    }

    //���� ���� �ҋ� �÷��̾� ����Ʈ ����� 
    public void AddPlayer(GameObject person)
    {
        //if (PhotonNetwork.IsMasterClient)
        {
            players.Add(person);
        }
        //return players.Count;
    }

    public GameObject GetPlayer(int id)
    {
        for (int i = 0; i < players.Count; i++)
        {
            PhotonView pv = players[i].GetComponent<PhotonView>();
            if (pv.ViewID== id)
            {
                return players[i].gameObject;
            }
        }
        return null;
    }


    public bool ChackAnswer(string ans)
    {
        return ans == nowQ;
    }

    public void ResetTurn()
    {
        StopCoroutine(SettingRole());
        rs.A_RoleAlim();
        rs = null;
        turnOver = true;
    }
}
