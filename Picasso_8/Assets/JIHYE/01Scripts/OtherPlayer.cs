using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class OtherPlayer : MonoBehaviourPunCallbacks
{
    struct SyncData
    {
        public Vector3 pos;
        public Quaternion rot;
    }

    public Transform[] body;
    SyncData[] syncData;
    Vector3 pos;

    public GameObject line;
    LineRenderer lr;

    void Awake()
    {
        if (photonView.IsMine == false)
        {
            syncData = new SyncData[body.Length];
        }
    }
    
    void Update()
    {
        if (!photonView.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, pos, 0.2f);
            for (int i = 0; i < body.Length; i++)
            {
                body[i].position = Vector3.Lerp(
                    body[i].position,
                    syncData[i].pos, 0.2f);

                body[i].rotation = Quaternion.Lerp(
                    body[i].rotation,
                    syncData[i].rot, 0.2f);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            for (int i = 0; i < body.Length; i++)
            {
                stream.SendNext(body[i].position);
                stream.SendNext(body[i].rotation);
            }
        }
        if (stream.IsReading)
        {
            pos = (Vector3)stream.ReceiveNext();

            if (syncData != null)
            {
                for (int i = 0; i < body.Length; i++)
                {
                    syncData[i].pos = (Vector3)stream.ReceiveNext();
                    syncData[i].rot = (Quaternion)stream.ReceiveNext();
                }
            }
        }
    }
}
