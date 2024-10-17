using Photon.Pun;
using UnityEngine;

public class Toukka : MonoBehaviourPun
{
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject extraText;
    [SerializeField] private AudioClip grabSound;

    private PhotonView view;
    private Transform parent;

    private void Awake() => view = this.photonView;

    private void Start() => parent = this.gameObject.transform.parent;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Toukka" && view.IsMine) Doing(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Toukka" && view.IsMine) Doing(collision);
    }

    private void Doing(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Keypad0))
        {
            view.RPC("ChangeExtraLife", RpcTarget.All);
            GameObject intatinable = PhotonNetwork.Instantiate(shield.name, new Vector3(parent.position.x, parent.position.y, 7), Quaternion.identity);
            PhotonNetwork.Instantiate(extraText.name, collision.transform.position, Quaternion.identity);
            intatinable.transform.SetParent(parent);
            collision.gameObject.GetComponent<DestroyInNetwork>().enabled = true;
        }
    }

    [PunRPC]
    void ChangeExtraLife() {
        parent.transform.GetComponent<AudioSource>().PlayOneShot(grabSound);
        parent.transform.GetComponent<PlayerController>().extraLife = true;
    }
}
