using Photon.Pun;
using UnityEngine;

public class DestroyInNetwork : MonoBehaviourPun
{
    public int type = 0;
    [SerializeField] private float time;

    public float TimeGetSet { get => time; set => time = value; }

    void Start()
    {
       if (type == 0) GetComponent<PhotonView>().RPC("Destroing", RpcTarget.All);
       if (type == 1) GetComponent<PhotonView>().RPC("Disable", RpcTarget.All);
    }

    [PunRPC]
    private void Destroing() => Destroy(gameObject, time);

    [PunRPC]
    private void Disable()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        gameObject.GetComponent<Collider2D>().enabled = false;
    }
}
