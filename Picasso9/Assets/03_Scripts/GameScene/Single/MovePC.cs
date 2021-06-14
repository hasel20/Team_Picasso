using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePC : MonoBehaviour
{
    [Tooltip("ī�޶� �������� �̵��ϱ� ���� ����")]
    public Transform CenterEye;
    [Tooltip("ĳ���� �̵� �ӵ�")]
    public float MoveSpeed;
    public float RotSpeed;

    [Tooltip("�̵��� ���� �� ����")]
    public bool LockFly = false;
    public bool UseMoving = false;
    public bool UseRotate = false;

    void Update()
    {
        if(UseMoving) Moving();
        if(UseRotate) Rotate();
    }

    void Moving()
    {
        Vector2 pedL = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
        Vector3 dir = new Vector3(pedL.x, 0, pedL.y);

        dir = CenterEye.TransformDirection(dir);
        if (LockFly) { dir.y = 0; }
        transform.Translate(dir * MoveSpeed * Time.deltaTime);
    }
    void Rotate()
    {
        Vector2 padRot = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
        Quaternion dir = new Quaternion(padRot.y, padRot.x, 0, 1);
        ;
    }
}
