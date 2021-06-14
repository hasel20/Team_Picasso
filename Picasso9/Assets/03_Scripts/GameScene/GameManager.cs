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

    //내 Player만 저장
    public GameObject Master;

    bool turnOver = true;
    int index;
    int qindex;

    private void Awake()
    {
        if (this != null) instance = this;

        Master = PhotonNetwork.Instantiate("Player_Draw", new Vector3(0, 1.5f, 0), Quaternion.identity); //내 Player 생성
        
    }
    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //턴제로 들어온 player를 랜덤으로 돌려서 한명씩 그림 그리고 그림 다 그리면 끝!
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

    
    //생성된 라인을 리스트업 하고 그 순번 알려주는 함수
    //중간에 빠지면 리스트 카운트도 줄어들어서 별도 의 변수 를 놔준다.
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

    //라인 리스트에서 인덱스와 비교해서 원하는 라인 찾아주는 함수.
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

    //게임 시작 할떄 플레이어 리스트 만들긔 
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
