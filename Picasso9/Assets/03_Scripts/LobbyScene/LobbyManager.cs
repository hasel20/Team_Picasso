using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public InputField RoomNameInput;
    public InputField maxUserInput;

    public Button joinButton;
    public Button createButton;

    Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>(); //�� ��� ĳ��
    public Transform content; //Scrollview - content
    public GameObject roomInfoFactory; //RoomInfo ��ư ����

    void Start()
    {
        //RoomNameInput.onValueChanged.AddListener(OnChangedRoomName);
        //maxUserInput.onValueChanged.AddListener(OnChangedMaxUser);
    }
    void Update()
    {
        createButton.interactable = maxUserInput.text.Length > 0 && RoomNameInput.text.Length > 0;
        joinButton.interactable = RoomNameInput.text.Length > 0 && !createButton.interactable;
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(RoomNameInput.text);
    }

    public void CreateRoom()
    {
        //�� ���鶧 �ɼ� (�ִ� �ο�, ��� ����Ʈ On/Off)
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = byte.Parse(maxUserInput.text); // �ִ��ο� 0�� ������
        roomOptions.IsVisible = true; //�� ����Ʈ�� �������� ����, �⺻���� true
        roomOptions.IsOpen = true; //��Ͽ��� ������ �������, �⺻���� true

        PhotonNetwork.CreateRoom(RoomNameInput.text, roomOptions, TypedLobby.Default); //�� ����
# region ����� �ɼ�
        //PhotonNetwork.JoinOrCreateRoom("TestRoom", roomOptions, TypedLobby.Default); //�� ���� Ȥ�� ����;
        //PhotonNetwork.JoinRandomRoom(); //���� �濡 ����
        //PhotonNetwork.JoinRoom("�� �̸�"); //Ư�� �� ����
#endregion
    }
    public override void OnCreatedRoom()
    {
        print("�� ���� ����");
        print(PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnCreateRoomFailed(short returnCode, string message) //�� ���� ����

    {
        Debug.LogWarning("�� ���� ����");
        base.OnCreateRoomFailed(returnCode, message);
    }


    public override void OnJoinedRoom() //�� ���� ����
    {
        //base.OnJoinedRoom();
        print("�� ���� ����");
        print(PhotonNetwork.CurrentRoom.Name);

        PhotonNetwork.LoadLevel("PhotonGame1"); //Scene �̵�
    }

    public override void OnJoinRoomFailed(short returnCode, string message) //�� ���� ����
    {
        Debug.LogWarning("�� ���� ����");
        base.OnJoinRoomFailed(returnCode, message);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) //���� �� ���� ����
    {
        base.OnRoomListUpdate(roomList);
        for (int i = 0; i < roomList.Count; i++)
        {
            print(roomList[i].Name);
        }
        //���� ������� UI�� ����
        DeleteRoomList();
        //roomCache ���� ����
        UpdateRoomCache(roomList);
        //UI ���Ӱ� �����
        CreateRoomList();
    }
    //roomCache ����
    void UpdateRoomCache(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            //���࿡ �߰� Ȥ�� ����� ���� roomCache�� �ִٸ�
            if (roomCache.ContainsKey(roomList[i].Name))
            {
                //���࿡ �� ���� ������ �Ѵٸ�
                if (roomList[i].RemovedFromList)
                {
                    roomCache.Remove(roomList[i].Name);
                    continue;
                }
            }
            //�߰�/������ ������ ����� �ƴ϶�� roomCache�� ���� �Ǵ� �߰�
            roomCache[roomList[i].Name] = roomList[i];
        }
    }

    //�� ���� ����
    void DeleteRoomList()
    {
        foreach (Transform tr in content)
        {
            Destroy(tr.gameObject);
        }
    }

    void CreateRoomList() //������ ����
    {
        foreach (RoomInfo info in roomCache.Values)
        {
            //1. roomInfo��ư ���忡�� roomInfo ��ư ����
            GameObject room = Instantiate(roomInfoFactory);
            //2. ������� roomInfo��ư content�� �ڽ����� ����
            room.transform.SetParent(content);
            //3. ������� roominfo ��ư���� RoomInfoBtn ������Ʈ �����ͼ�
            RoomInfoButton btn = room.GetComponent<RoomInfoButton>();
            //4. ������ ������Ʈ�� SetInfo �Լ� ȣ��
            btn.Setinfo(info.Name, info.PlayerCount, info.MaxPlayers);
            //5. Ŭ�� �Ǿ��� �� �Լ��� ���
            btn.clickAction = OnClickRoomInfo;
        }
    }

    void OnClickRoomInfo(string roomName)
    {
        print("�� ��� ����");
        RoomNameInput.text = roomName;
    }
}