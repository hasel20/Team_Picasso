using UnityEngine;
using Photon.Pun;

public class ObjRotate : MonoBehaviour
{
    public bool useRotH = false; //좌우회전 가능
    public bool useRotV = false; //상하회전 가능

    public PhotonView pv;

    float rotX;
    float rotY;
    float rotSpeed = 200;

    void Update()
    {
        if (pv.IsMine == false) return;

        //마우스 좌우상하 움직임
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        //오브젝트의 각도를 누적
        if (useRotV) rotX += my * rotSpeed * Time.deltaTime;
        if (useRotH) rotY += mx * rotSpeed * Time.deltaTime;

        //각도를 세팅
        transform.localEulerAngles = new Vector3(-rotX, rotY, 0);
    }
}
