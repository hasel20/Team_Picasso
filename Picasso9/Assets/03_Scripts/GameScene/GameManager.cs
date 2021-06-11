using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;

    public List<string> gameQuestion;

    public List<GameObject> players = new List<GameObject>();

    public List<LineInfo> Lines = new List<LineInfo>();

    //내 Player만 저장
    public GameObject Master;

    bool turnOver = false;

    private void Awake()
    {
        if (this != null) instance = this;

        Master = PhotonNetwork.Instantiate("Player_Draw", new Vector3(0, 1.5f, 0), Quaternion.identity); //내 Player 생성
    }
    private void Update()
    {
        //턴제로 들어온 player를 랜덤으로 돌려서 한명씩 그림 그리고 그림 다 그리면 끝!
        //if (turnOver)
        //{
        //    Randoms(players);
        //}               
    }

    void Randoms(List<GameObject> lili)
    {
        turnOver = false;
        for (int i = 0; i < 100; i++)
        {
            GameObject a = lili[Random.Range(0, lili.Count)];
            GameObject b = lili[Random.Range(0, lili.Count)];

            GameObject xx = a;
            a = b;
            b = xx;
        }
        StartCoroutine(SettingRole());
    }

    IEnumerator SettingRole()
    {
        players[0].GetComponent<RoleSet>().role = RoleSet.Role.painter;
        yield return new WaitForSeconds(60);
        players[0].GetComponent<RoleSet>().role = RoleSet.Role.answerer;
        turnOver = true;
    }

    //생성된 라인을 리스트업 하고 그 순번 알려주는 함수
    public int AddLine(LineInfo line)
    {
        Lines.Add(line);
        return Lines.Count;
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
        players.Add(person);
    }

    ////정답 치는 란 확인 하긔.
    //public void OnClickSend()
    //{
    //    RoleSet rs = Master.GetComponent<RoleSet>();
        
    //    rs.SetChat("sd");
    //}
}
