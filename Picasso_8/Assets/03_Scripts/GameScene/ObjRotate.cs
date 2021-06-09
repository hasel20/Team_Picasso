using UnityEngine;
using Photon.Pun;

public class ObjRotate : MonoBehaviour
{
    public bool useRotH = false; //�¿�ȸ�� ����
    public bool useRotV = false; //����ȸ�� ����

    public PhotonView pv;

    float rotX;
    float rotY;
    float rotSpeed = 200;

    void Update()
    {
        if (pv.IsMine == false) return;

        //���콺 �¿���� ������
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        //������Ʈ�� ������ ����
        if (useRotV) rotX += my * rotSpeed * Time.deltaTime;
        if (useRotH) rotY += mx * rotSpeed * Time.deltaTime;

        //������ ����
        transform.localEulerAngles = new Vector3(-rotX, rotY, 0);
    }
}
