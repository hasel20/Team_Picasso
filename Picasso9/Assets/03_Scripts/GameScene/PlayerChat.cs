using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerChat : MonoBehaviourPun
{
    public Text chatUi;
    
    public void SetChatValue(string text)
    {
        photonView.RPC("RpcSetChatValue", RpcTarget.All, text);
    }

    [PunRPC]
    void RpcSetChatValue(string text)
    {
        chatUi.text = text;
    }
}
