using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMoveChar : MonoBehaviourPun
{
    private Rigidbody rigid;
    public float Speed = 7; //이동속도
    public float JumpPower = 1.5f; //점프력
    public float gravity = 4f; //중력
    float yVelocity; // y속력

    int maxJumpCnt = 2; //다중점프 제한
    int jumpCnt = 0; //현재 점프횟수

    public Transform gun;

    //상대방 위치, 회전
    Vector3 otherPos;
    Quaternion otherRot;

    //카메라
    public GameObject cam;

    CharacterController cc;
    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    private void Start()
    {
        if (photonView.IsMine) { cam.SetActive(true); } //내 카메라만 키기
    }
    void Update()
    {
        if (photonView.IsMine) //내꺼만 이동
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

        //1.키가 눌리면
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //2.방향을 정하고
        Vector3 dir = new Vector3(h, 0, v);

        //카메라가 보는 방향으로 방향 재설정
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

        //3. 이동하기
        //transform.position += dir * Speed * Time.deltaTime;
        cc.Move(dir * Speed * Time.deltaTime);
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
}
