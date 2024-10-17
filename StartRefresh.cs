using Photon.Pun;
using UnityEngine;

public class StartRefresh : MonoBehaviourPun
{
    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("Listrefresh", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void Listrefresh()
    {
        FindObjectOfType<InteractableObjectsList>().ListRefresh();
    }
}
