using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun
{
    public float speed = 10;

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                PlayerMoveAxis pm = other.gameObject.GetComponentInParent<PlayerMoveAxis>();
                pm.OnDamaged(Random.Range(8,12));
            }
        }
        Destroy(gameObject);
    }
}
