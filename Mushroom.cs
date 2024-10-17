using Photon.Pun;
using UnityEngine;

public class Mushroom : MonoBehaviourPun
{
    [SerializeField] private GameObject smokePart;
    PhotonView view;

    private void Awake() => view = GetComponent<PhotonView>();

    private void Start() => ChangeOff(true);

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GameController"))
        {
            ChangeOff(false);
            Invoke("InstantiateObj", 0.5f);
        }
    }

    public void ChangeOff(bool status) => view.RPC("Off", RpcTarget.AllBuffered, status);

    private void InstantiateObj() => PhotonNetwork.Instantiate(smokePart.name, transform.position, Quaternion.identity);

    [PunRPC]
    private void Off(bool status)
    {
        if (status == true) gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        else gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        gameObject.GetComponent<Collider2D>().enabled = status;
    }
}
