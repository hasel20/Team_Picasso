using System;
using UnityEngine;
using UnityEngine.UI;

//public delegate void ClickAction(string s); //Ŭ�� �Ǿ����� �Լ��� ����Ҽ� �ִ� delegate

public class RoomInfoButton : MonoBehaviour
{
    public Action<string> clickAction;
    //public ClickAction clickAction; //Ŭ�� �Ǿ����� ȣ�� �Ǵ� �Լ�

    //������ ������ �ؽ�Ʈ
    public Text info;
    string room; //������


    //������ (�����ο�/�ִ��ο�)
    public void Setinfo(string roomName, int currPlayer, int maxPlayer)
    {
        room = roomName;
        info.text = roomName + "   (" + currPlayer + " / " + maxPlayer + ")";
    }
    public void OnClick()
    {
        if (clickAction != null) //clickAction ���� �ִٸ� ����
        {
            clickAction(room);
        }

        ////LobbyManager(GameObject)ã��
        //GameObject go = GameObject.Find("LobbyManager");
        //LobbyManager lm = go.GetComponent<LobbyManager>();
        //lm.RoomNameInput.text = room;
        ////LobbyManager(GameObject)ã��
    }
}
