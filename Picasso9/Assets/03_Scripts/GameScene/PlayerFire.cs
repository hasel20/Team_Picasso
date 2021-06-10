using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerFire : MonoBehaviourPun
{
    public Transform firePos;
    public GameObject bulletFactory;
    public GameObject bulletImpact;

    void Update()
    {
        Fire();
    }
    void Fire()
    {
        //������ �ƴϸ� �Լ��� ���´�
        if (photonView.IsMine == false) return;

        if (Input.GetButtonDown("Fire1"))
        {
            photonView.RPC("RpcFire", RpcTarget.All, firePos.position, firePos.rotation);
        }
        if (Input.GetButtonDown("Fire2"))
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                photonView.RPC("RpcFire2", RpcTarget.All, hit.point);

                if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    PlayerMoveAxis pm = hit.transform.GetComponentInParent<PlayerMoveAxis>();
                    pm.OnDamaged(Random.Range(12,15));
                }
            #region
            //PhotonNetwork.Instantiate("Bullet", firePos.position, firePos.rotation);
            //���� ���ͳ� �����, �Ʒ��� �������ν� 
            //GameObject bullet = Instantiate(bulletFactory);
            //bullet.transform.position = firePos.position;
            //bullet.transform.rotation = firePos.rotation;
            #endregion
            }
        }
    }
    [PunRPC]
    void RpcFire(Vector3 pos, Quaternion rot)
    {
        //PhotonNetwork.Instantiate("Bullet", pos, rot);
        //���� ���ͳ�, �Ʒ��� ��������        
        GameObject bullet = Instantiate(bulletFactory);
        bullet.transform.position = pos;
        bullet.transform.rotation = rot;
    }
    [PunRPC]
    void RpcFire2(Vector3 hit)
    {
        GameObject bulletEft = Instantiate(bulletImpact);
        bulletEft.transform.position = hit;
        bulletEft.GetComponentInChildren<ParticleSystem>().Play();
        Destroy(bulletEft, 1f);
    }
}
