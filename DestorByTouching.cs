using Photon.Pun;
using UnityEngine;

public class DestorByTouching : MonoBehaviour
{
    public float timer = 4f;

    private void FixedUpdate()
    {
        timer -= Time.fixedDeltaTime;
        if (timer <= 0) GetComponent<PhotonView>().RPC("Destroing", RpcTarget.All);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GameController")) GetComponent<PhotonView>().RPC("Destroing", RpcTarget.All);
    }
    [PunRPC]
    private void Destroing() => Destroy(gameObject);
}
