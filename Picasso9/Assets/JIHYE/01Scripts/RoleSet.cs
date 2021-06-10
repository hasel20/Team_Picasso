using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class RoleSet : MonoBehaviourPun
{
    public Text alim;

    public GameObject keybord;

    bool key;
    public enum Role
    {
        answerer,
        painter
    }
    public Role role = Role.answerer;

    void Start()
    {
        GameManager.instance.AddPlayer(this.gameObject);        
    }

    void Update()
    {
        AlimText();
        //내꺼일떄 
        if (photonView.IsMine)
        {
            //관중이면
            if (role == Role.answerer)
            {
                KeybordSet();
                Typing();
            }
            //else if (role == Role.painter)
            //{
            //    key = false;
            //    keybord.SetActive(false);


            //}
        }
    }
    void AlimText()
    {
        if (alim != null)
        {
            if (role == Role.painter)
            {
                List<string> que = GameManager.instance.gameQuestion;
                alim.text = "[ " + que[Random.Range(0, que.Count)] + " ] 그림을 그려보세요!";
                alim.color = Color.blue;
            }
            else
            {
                alim.text = "그림을 맞춰봐요!";
                alim.color = Color.black;
            }
        }
    }

    void KeybordSet()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two,OVRInput.Controller.LTouch))
        {
            key = !key;
            //print(key);
        }
        keybord.SetActive(key);
    }

    void Typing()
    {
        if (key)
        {
            //키보드로 모두에게 알려줘야함.
        }
    }
}
