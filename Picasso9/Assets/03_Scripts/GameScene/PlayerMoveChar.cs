using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMoveChar : MonoBehaviourPun
{
    private Rigidbody rigid;
    public float Speed = 7; //�̵��ӵ�
    public float JumpPower = 1.5f; //������
    public float gravity = 4f; //�߷�
    float yVelocity; // y�ӷ�

    int maxJumpCnt = 2; //�������� ����
    int jumpCnt = 0; //���� ����Ƚ��

    public Transform gun;

    //���� ��ġ, ȸ��
    Vector3 otherPos;
    Quaternion otherRot;

    //ī�޶�
    public GameObject cam;

    CharacterController cc;
    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    private void Start()
    {
        if (photonView.IsMine) { cam.SetActive(true); } //�� ī�޶� Ű��
    }
    void Update()
    {
        if (photonView.IsMine) //������ �̵�
        {
            Move();
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, otherPos, 0.2f);
            transform.rotation = Quaternion.Lerp(transform.rotation, otherRot, 0.2f);
        }
    }

    private void Move()
    {
        if (photonView.IsMine == false) return;

        //1.Ű�� ������
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //2.������ ���ϰ�
        Vector3 dir = new Vector3(h, 0, v);

        //ī�޶� ���� �������� ���� �缳��
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;
        dir.Normalize();

        if (cc.isGrounded == true)
        {
            jumpCnt = 0;
            yVelocity = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpCnt < maxJumpCnt)
        {
            yVelocity = JumpPower;
            jumpCnt++;

        }

        yVelocity -= gravity * Time.deltaTime;

        dir.y = yVelocity;

        //3. �̵��ϱ�
        //transform.position += dir * Speed * Time.deltaTime;
        cc.Move(dir * Speed * Time.deltaTime);
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
}
