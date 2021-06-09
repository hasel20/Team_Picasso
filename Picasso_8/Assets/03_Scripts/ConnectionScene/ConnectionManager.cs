using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    public string gameVersion = "1.0"; //���� ����
    public InputField blankID; //�г���
    public InputField blackPW; //�н�����
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
        //���࿡ �г����� ���̰� 0�̶��, ���� �Ұ�
        if (blankID.text.Length == 0)
        {
            Debug.LogWarning("���̵� �Է��� �ּ���");
        }
        else
        {
            PhotonNetwork.GameVersion = gameVersion; // 1. Game Version ����
            PhotonNetwork.AutomaticallySyncScene = true; // 2. Scene�� ����ȭ ����
            PhotonNetwork.ConnectUsingSettings(); // 3. ����
        }
    }

    public void SandBox()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public override void OnConnected() //Name Server ���� ����
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

    public override void OnConnectedToMaster() //Master Server ���� ����
    {
        //base.OnConnectedToMaster();
        print("OnConnectedToMaster");

        PhotonNetwork.NickName = blankID.text; //�г����� �����ϰ�
        PhotonNetwork.JoinLobby(TypedLobby.Default); //�κ�� ���ӿ�û
        //PhotonNetwork.JoinLobby(new TypedLobby("�κ��̸�", LobbyType.Default));
    }

    //Lobby ���� ������
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