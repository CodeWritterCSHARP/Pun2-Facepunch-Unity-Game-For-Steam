using Photon.Pun;
using UnityEngine;

public class ActivateInNetwork : MonoBehaviourPun
{
    [SerializeField] private GameObject gameObj;
    [SerializeField] private int type = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GameController")) {
            switch (type)
            {
                case 0: this.photonView.RPC("Activate", RpcTarget.All); break;
                case 1: this.photonView.RPC("ActivateWheel", RpcTarget.All); break;
                case 2: this.photonView.RPC("ActivateAnimated", RpcTarget.All); break;
                case 3: this.photonView.RPC("ActivateFinnish", RpcTarget.All); break;
                default: break;
            }
        }
    }

    [PunRPC]
    void ActivateAnimated()
    {
        try { gameObj.GetComponent<Animator>().enabled = true; } catch { }
    }

    [PunRPC]
    void ActivateWheel()
    {
        gameObj.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        gameObj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    [PunRPC]
    void ActivateFinnish()
    {
        gameObj.GetComponent<MovementToPoint>().enabled = true;
    }

    [PunRPC]
    void Activate()
    {
        gameObj.SetActive(true);
    }
}
