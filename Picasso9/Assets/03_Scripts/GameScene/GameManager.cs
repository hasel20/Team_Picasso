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

    int count;
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
        yield return new WaitForSeconds(15);
        
        turnOver = true;
        ReSetLines();
    }

    
    //������ ������ ����Ʈ�� �ϰ� �� ���� �˷��ִ� �Լ�
    //�߰��� ������ ����Ʈ ī��Ʈ�� �پ�� ���� �� ���� �� ���ش�.
    public int AddLine(LineInfo line)
    {
        count++;
        Lines.Add(line);
        return count;
    }
    //
    public void RemoveLine(LineInfo line)
    {
        Lines.Remove(line);
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
    public int WhoPainter()
    {
        for (int i = 0; i < players.Count; i++)
        {
            PhotonView pv = players[i].GetComponent<PhotonView>();
            RoleSet rs = players[i].GetComponent<RoleSet>();
            if (rs.role == RoleSet.Role.painter)
            {
                return pv.ViewID;
            }
        }
        return -10;
    }


    public bool ChackAnswer(string ans)
    {
        return ans == nowQ;
    }

    public void ResetTurn()
    {
        photonView.RPC("RPCResetTurn", RpcTarget.All);
    }
    [PunRPC]
    void RPCResetTurn()
    {
        StopCoroutine(SettingRole());
        rs.A_RoleAlim();
        rs = null;
        turnOver = true;
        ReSetLines();
    }

    public void ReSetLines()
    {
        count = 0;
        for (int i = 0; i < Lines.Count; i++)
        {
            Destroy(Lines[i]);
        }
        Lines.Clear();
    }
}
