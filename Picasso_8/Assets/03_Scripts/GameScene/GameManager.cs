using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    //�� Player�� ����
    public GameObject Master;

    private void Awake()
    {
        Master = PhotonNetwork.Instantiate("Player_Draw", new Vector3(0, 1.5f, 0), Quaternion.identity); //�� Player ����
    }
}
