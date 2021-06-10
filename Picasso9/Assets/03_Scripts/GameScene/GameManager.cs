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

    //내 Player만 저장
    public GameObject Master;

    private void Awake()
    {
        if (this != null) instance = this;

        Master = PhotonNetwork.Instantiate("Player_Draw", new Vector3(0, 1.5f, 0), Quaternion.identity); //내 Player 생성
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
}
