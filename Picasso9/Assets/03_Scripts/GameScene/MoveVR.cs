using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MoveVR : MonoBehaviourPun, IPunObservable
{
    struct SyncData
    {
        public Vector3 pos;
        public Quaternion rot;
    }

    public Transform CenterEye;
    public float MoveSpeed;

    public GameObject myBody;
    public GameObject otherBody;

    public Transform[] myBodyMesh;
    public Transform[] otherBodyMesh;

    Vector3 pos;
    SyncData[] syncData;

    public bool LockFly = false;

    private void Awake()
    {
        if (photonView.IsMine == false)
        {
            if(myBodyMesh != null)
            syncData = new SyncData[myBodyMesh.Length];
        }

        if (myBody != null && otherBody != null)
        {
            myBody.SetActive(photonView.IsMine);
            otherBody.SetActive(!photonView.IsMine);
        }
    }

    void Update()
    {
        //상대방움직임
        if (!photonView.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, pos, 0.2f);
            for (int i = 0; i < otherBodyMesh.Length; i++)
            {
                otherBodyMesh[i].position = Vector3.Lerp(
                    otherBodyMesh[i].position,
                    syncData[i].pos, 0.2f);

                otherBodyMesh[i].rotation = Quaternion.Lerp(
                    otherBodyMesh[i].rotation,
                    syncData[i].rot, 0.2f);
            }
        }
        Moving();
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
        Quaternion dir = new Quaternion(0, padRot.y, 0, 1);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            for (int i = 0; i < myBodyMesh.Length; i++)
            {
                stream.SendNext(myBodyMesh[i].position);
                stream.SendNext(myBodyMesh[i].rotation);
            }
        }

        if (stream.IsReading)
        {
            pos = (Vector3)stream.ReceiveNext();

            if (syncData != null)
            {
                for (int i = 0; i < otherBodyMesh.Length; i++)
                {
                    syncData[i].pos = (Vector3)stream.ReceiveNext();
                    syncData[i].rot = (Quaternion)stream.ReceiveNext();
                }
            }
        }
    }
}
