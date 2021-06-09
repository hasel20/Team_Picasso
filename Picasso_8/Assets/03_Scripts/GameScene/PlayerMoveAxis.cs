using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class PlayerMoveAxis : MonoBehaviourPun, IPunObservable
{
    public float moveSpeed = 5; //이동속도
    public Transform gun;

    //상대방 위치, 회전
    Vector3 otherPos;
    Quaternion otherRot;


    //카메라
    public GameObject cam;
    public Text nickNameUI;
    public Slider hpBar;
    public Text chat;
    public float maxHP = 100;
    public float HP;

    //몸색상 변경에 필요한 변수들
    public MeshRenderer bodyMR;
    public Material matMe;
    public Material matOther;

    private void Start()
    {
        print(photonView.Owner.NickName + " 님께서 입장하셨습니다.");
        nickNameUI.text = photonView.Owner.NickName;
        if (photonView.IsMine) //내 카메라만 키기
        {
            cam.SetActive(true);
            nickNameUI.gameObject.SetActive(false);
            hpBar.gameObject.SetActive(false);
            chat.gameObject.SetActive(false);
            hpBar.maxValue = maxHP;
            hpBar.value = hpBar.maxValue;
            bodyMR.material = matMe;
            //gameObject.layer = LayerMask.NameToLayer("MyPlayer");
        }
        else
        {
            cam.SetActive(false);
            bodyMR.material = matOther;
            //gameObject.layer = LayerMask.NameToLayer("OtherPlayer");
        }
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (photonView.IsMine) //내꺼만 이동
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 dir = new Vector3(h, 0, v);
            dir.Normalize();

            dir = Camera.main.transform.TransformDirection(dir);
            dir.y = 0;

            transform.position += dir * moveSpeed * Time.deltaTime;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, otherPos, 0.2f);
            transform.rotation = Quaternion.Lerp(transform.rotation, otherRot, 0.2f);
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //서술한 순서대로 주고받음
        if (stream.IsWriting)
        {
            //나의 위치와 회전값 보내기
            stream.SendNext(transform.position); //위치
            stream.SendNext(transform.rotation); //회전
            stream.SendNext(gun.rotation); //총의 회전값

        }
        if (stream.IsReading)
        {
            //누군가의 위치와 회전값 받기
            otherPos = (Vector3)stream.ReceiveNext();
            otherRot = (Quaternion)stream.ReceiveNext();
            gun.rotation = (Quaternion)stream.ReceiveNext();
        }
    }

    public void OnDamaged(float damage)
    {
        photonView.RPC("RpcOnDamaged", RpcTarget.All, damage);
    }

    [PunRPC]
    void RpcOnDamaged(float damage)
    {
        if (HP > 0)
        {
            HP -= damage;
            hpBar.value = HP / maxHP;
            print(photonView.Owner.NickName + " HP : " + HP);
        }
        else { /*Destroy(this);*/ }
    }
}
