using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;

    public List<LineInfo> Lines = new List<LineInfo>();

    //�� Player�� ����
    public GameObject Master;

    private void Awake()
    {
        if (this != null) instance = this;

        Master = PhotonNetwork.Instantiate("Player_Draw", new Vector3(0, 1.5f, 0), Quaternion.identity); //�� Player ����
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
}
