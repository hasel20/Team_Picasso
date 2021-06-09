using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    public string gameVersion = "1.0"; //게임 버전
    public InputField blankID; //닉네임
    public InputField blackPW; //패스워드
    public string[] nickname;

    public GameObject Selector;
    public GameObject ConnectCanvas;
    public GameObject LobbyCanvas;
    public Text Description;

    private void Awake()
    {
        if (Selector != null) Selector.SetActive(true);
        ConnectCanvas.SetActive(true);
        LobbyCanvas.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            OnQuickConnect();
        }
    }

    public void Connect()
    {
        //만약에 닉네임의 길이가 0이라면, 접속 불가
        if (blankID.text.Length == 0)
        {
            Debug.LogWarning("아이디를 입력해 주세요");
        }
        else
        {
            PhotonNetwork.GameVersion = gameVersion; // 1. Game Version 설정
            PhotonNetwork.AutomaticallySyncScene = true; // 2. Scene을 동기화 할지
            PhotonNetwork.ConnectUsingSettings(); // 3. 접속
        }
    }

    public void SandBox()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public override void OnConnected() //Name Server 접속 성공
    {
        print("OnConnected");
        ConnectCanvas.SetActive(true);
        //base.OnConnected();
        Description.text = "Server Connected Success";
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Description.text = "Server Connected Failed";
    }

    public override void OnConnectedToMaster() //Master Server 접속 성공
    {
        //base.OnConnectedToMaster();
        print("OnConnectedToMaster");

        PhotonNetwork.NickName = blankID.text; //닉네임을 설정하고
        PhotonNetwork.JoinLobby(TypedLobby.Default); //로비로 접속요청
        //PhotonNetwork.JoinLobby(new TypedLobby("로비이름", LobbyType.Default));
    }

    //Lobby 접속 성공시
    public override void OnJoinedLobby()
    {
        //base.OnJoinedLobby();
        print("OnJoinedLobby");

        ConnectCanvas.SetActive(false);
        if (Selector != null) Selector.SetActive(false);
        LobbyCanvas.SetActive(true);


    }

    //public override void OnJoinedRoom()
    //{
    //    PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    //}
    public void OnQuickConnect()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = "Tester";
        StartCoroutine("QuickConnect");
    }
    IEnumerator QuickConnect()
    {
        PhotonNetwork.ConnectUsingSettings();
        yield return new WaitForSeconds(3);
        PhotonNetwork.JoinOrCreateRoom("QC", new RoomOptions(), TypedLobby.Default);
    }
}