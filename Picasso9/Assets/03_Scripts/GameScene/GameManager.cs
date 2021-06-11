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

    //�� Player�� ����
    public GameObject Master;

    bool turnOver = false;

    private void Awake()
    {
        if (this != null) instance = this;

        Master = PhotonNetwork.Instantiate("Player_Draw", new Vector3(0, 1.5f, 0), Quaternion.identity); //�� Player ����
    }
    private void Update()
    {
        //������ ���� player�� �������� ������ �Ѹ� �׸� �׸��� �׸� �� �׸��� ��!
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
        players.Add(person);
    }

    ////���� ġ�� �� Ȯ�� �ϱ�.
    //public void OnClickSend()
    //{
    //    RoleSet rs = Master.GetComponent<RoleSet>();
        
    //    rs.SetChat("sd");
    //}
}
