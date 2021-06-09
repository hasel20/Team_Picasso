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

    Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>(); //방 목록 캐시
    public Transform content; //Scrollview - content
    public GameObject roomInfoFactory; //RoomInfo 버튼 공장

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
        //방 만들때 옵션 (최대 인원, 방명 리스트 On/Off)
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = byte.Parse(maxUserInput.text); // 최대인원 0은 무제한
        roomOptions.IsVisible = true; //방 리스트에 보여줄지 말지, 기본값은 true
        roomOptions.IsOpen = true; //목록에는 있지만 비공개방, 기본값은 true

        PhotonNetwork.CreateRoom(RoomNameInput.text, roomOptions, TypedLobby.Default); //방 생성
# region 방생성 옵션
        //PhotonNetwork.JoinOrCreateRoom("TestRoom", roomOptions, TypedLobby.Default); //방 입장 혹은 생성;
        //PhotonNetwork.JoinRandomRoom(); //랜덤 방에 입장
        //PhotonNetwork.JoinRoom("방 이름"); //특정 방 입장
#endregion
    }
    public override void OnCreatedRoom()
    {
        print("방 생성 성공");
        print(PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnCreateRoomFailed(short returnCode, string message) //방 생성 실패

    {
        Debug.LogWarning("방 생설 실패");
        base.OnCreateRoomFailed(returnCode, message);
    }


    public override void OnJoinedRoom() //방 접속 성공
    {
        //base.OnJoinedRoom();
        print("방 접속 성공");
        print(PhotonNetwork.CurrentRoom.Name);

        PhotonNetwork.LoadLevel("PhotonGame1"); //Scene 이동
    }

    public override void OnJoinRoomFailed(short returnCode, string message) //방 접속 실패
    {
        Debug.LogWarning("방 접속 실패");
        base.OnJoinRoomFailed(returnCode, message);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) //현재 방 정보 갱신
    {
        base.OnRoomListUpdate(roomList);
        for (int i = 0; i < roomList.Count; i++)
        {
            print(roomList[i].Name);
        }
        //현재 만들어진 UI를 삭제
        DeleteRoomList();
        //roomCache 정보 갱신
        UpdateRoomCache(roomList);
        //UI 새롭게 만든다
        CreateRoomList();
    }
    //roomCache 갱신
    void UpdateRoomCache(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            //만약에 추가 혹은 변경된 방이 roomCache에 있다면
            if (roomCache.ContainsKey(roomList[i].Name))
            {
                //만약에 그 방을 지워야 한다면
                if (roomList[i].RemovedFromList)
                {
                    roomCache.Remove(roomList[i].Name);
                    continue;
                }
            }
            //추가/변경이 됐지만 지울게 아니라면 roomCache에 변경 또는 추가
            roomCache[roomList[i].Name] = roomList[i];
        }
    }

    //방 정보 삭제
    void DeleteRoomList()
    {
        foreach (Transform tr in content)
        {
            Destroy(tr.gameObject);
        }
    }

    void CreateRoomList() //방정보 생성
    {
        foreach (RoomInfo info in roomCache.Values)
        {
            //1. roomInfo버튼 공장에서 roomInfo 버튼 생성
            GameObject room = Instantiate(roomInfoFactory);
            //2. 만들어진 roomInfo버튼 content의 자식으로 세팅
            room.transform.SetParent(content);
            //3. 만들어진 roominfo 버튼에서 RoomInfoBtn 컴포넌트 가져와서
            RoomInfoButton btn = room.GetComponent<RoomInfoButton>();
            //4. 가져온 컴포넌트의 SetInfo 함수 호출
            btn.Setinfo(info.Name, info.PlayerCount, info.MaxPlayers);
            //5. 클릭 되었을 때 함수를 등록
            btn.clickAction = OnClickRoomInfo;
        }
    }

    void OnClickRoomInfo(string roomName)
    {
        print("방 목록 선택");
        RoomNameInput.text = roomName;
    }
}