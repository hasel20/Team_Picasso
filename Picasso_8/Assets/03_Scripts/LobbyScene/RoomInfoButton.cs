using System;
using UnityEngine;
using UnityEngine.UI;

//public delegate void ClickAction(string s); //클릭 되었을때 함수를 등록할수 있는 delegate

public class RoomInfoButton : MonoBehaviour
{
    public Action<string> clickAction;
    //public ClickAction clickAction; //클릭 되었을때 호출 되는 함수

    //정보를 보여줄 텍스트
    public Text info;
    string room; //방제목


    //방제목 (현재인원/최대인원)
    public void Setinfo(string roomName, int currPlayer, int maxPlayer)
    {
        room = roomName;
        info.text = roomName + "   (" + currPlayer + " / " + maxPlayer + ")";
    }
    public void OnClick()
    {
        if (clickAction != null) //clickAction 값이 있다면 실행
        {
            clickAction(room);
        }

        ////LobbyManager(GameObject)찾자
        //GameObject go = GameObject.Find("LobbyManager");
        //LobbyManager lm = go.GetComponent<LobbyManager>();
        //lm.RoomNameInput.text = room;
        ////LobbyManager(GameObject)찾자
    }
}
