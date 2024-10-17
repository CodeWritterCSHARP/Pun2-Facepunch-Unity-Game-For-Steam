using UnityEngine;
using Photon.Pun;
using System.Linq;

public class CollisionSound : MonoBehaviourPun
{
    [SerializeField] private int[] layerNum;
    [SerializeField] private string[] layerName;
    [SerializeField] private AudioClip sound;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (layerNum.Contains(collision.gameObject.layer) || layerName.Contains(collision.gameObject.tag))
            this.photonView.RPC("PlaySound", RpcTarget.All);
    }

    [PunRPC]
    void PlaySound()
    {
        GetComponent<AudioSource>().PlayOneShot(sound);
    }
}
