using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class PlayerMoveAxis : MonoBehaviourPun, IPunObservable
{
    public float moveSpeed = 5; //�̵��ӵ�
    public Transform gun;

    //���� ��ġ, ȸ��
    Vector3 otherPos;
    Quaternion otherRot;


    //ī�޶�
    public GameObject cam;
    public Text nickNameUI;
    public Slider hpBar;
    public Text chat;
    public float maxHP = 100;
    public float HP;

    //������ ���濡 �ʿ��� ������
    public MeshRenderer bodyMR;
    public Material matMe;
    public Material matOther;

    private void Start()
    {
        print(photonView.Owner.NickName + " �Բ��� �����ϼ̽��ϴ�.");
        nickNameUI.text = photonView.Owner.NickName;
        if (photonView.IsMine) //�� ī�޶� Ű��
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
        if (photonView.IsMine) //������ �̵�
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
        //������ ������� �ְ����
        if (stream.IsWriting)
        {
            //���� ��ġ�� ȸ���� ������
            stream.SendNext(transform.position); //��ġ
            stream.SendNext(transform.rotation); //ȸ��
            stream.SendNext(gun.rotation); //���� ȸ����

        }
        if (stream.IsReading)
        {
            //�������� ��ġ�� ȸ���� �ޱ�
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
