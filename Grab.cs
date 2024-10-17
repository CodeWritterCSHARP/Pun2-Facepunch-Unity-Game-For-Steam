using UnityEngine;
using Photon.Pun;

public class Grab : MonoBehaviourPunCallbacks, IPunObservable
{
    public bool isGrabbing = false;
    public Transform parent;
    [SerializeField] private Transform grabpoint;
    [SerializeField] private GameObject siemenBird;
    private Vector3 dist;

    private Vector3 realposition;
    float changetimer = 1f;

    [SerializeField] private AudioClip grabSound;

    private PhotonView view;

    private void Awake() => view = GetComponent<PhotonView>();
    private void Start() => parent = GameObject.FindGameObjectWithTag("Hand").transform.parent.transform;

    private void Update()
    {
        parent = GameObject.FindGameObjectWithTag("Hand").transform.parent.transform;
        if (parent.GetComponent<CapsuleCollider2D>().enabled == false)
        {
            parent.GetComponent<Rigidbody2D>().angularVelocity = 0;
            parent.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if (view.IsMine)
            {
                if (isGrabbing == true)
                {
                        if (Vector2.Distance(parent.position, grabpoint.position) > 5) parent.position = Vector3.Lerp(parent.transform.position, grabpoint.position, .15f);
                       // parent.GetComponent<Rigidbody2D>().angularVelocity = 0;
                       // parent.GetComponent<Rigidbody2D>().velocity = grabpoint.position;

                        changetimer -= 0.25f;
                        if (changetimer <= 0) { changetimer = 0; UnGrabbing(); }
                        parent.position = grabpoint.position;
                }
            }
            else
            {
                parent.position = Vector3.Lerp(parent.transform.position, realposition, .15f);
              //  parent.GetComponent<Rigidbody2D>().angularVelocity = 0;
              //  parent.GetComponent<Rigidbody2D>().velocity = realposition;
            }
        }
        else realposition = new Vector3();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Hand" && view.IsMine) Doing(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hand" && view.IsMine) Doing(collision);
    }

    public void UnGrabbingDeatf()
    {
        foreach (Transform child in parent) { child.position -= dist; }
        isGrabbing = false;
        changetimer = 1;
        view.RPC("ColliderChange", RpcTarget.AllBuffered, true, false);
    }

    private void UnGrabbing()
    {
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Keypad0))
        {
            parent.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            parent.transform.position = grabpoint.transform.position;
            foreach (Transform child in parent) { child.position -= dist; }
            isGrabbing = false;
            changetimer = 1;
            view.RPC("ColliderChange", RpcTarget.AllBuffered, true, true);
        }
    }

    [PunRPC]
    void ColliderChange(bool status, bool playingA)
    {
        //if (view.IsMine)
        //{
        //    if (status == true) siemenBird.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        //    else siemenBird.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        //}
        //else
        //{
        //    if (status == true) siemenBird.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        //    else siemenBird.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        //}

        if (playingA == true) transform.parent.GetComponent<AudioSource>().PlayOneShot(grabSound);
        if (parent != null)
        {
            parent = GameObject.FindGameObjectWithTag("Hand").transform.parent.transform;
            parent.GetComponent<CapsuleCollider2D>().enabled = status;
//            if (status == true) parent.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        //    else parent.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
            parent.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        }
    }
   
    void Doing(Collider2D collision)
    {
        if ((Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Keypad0)) && isGrabbing == false)
        {
            parent.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            parent = collision.transform.parent;
            parent.transform.position = grabpoint.transform.position;

            dist = parent.position - collision.transform.position;

            foreach (Transform child in parent) { child.position += dist; }

            view.RPC("ColliderChange", RpcTarget.All, false, true);
            isGrabbing = true;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (parent != null) 
        {
            if (stream.IsWriting) stream.SendNext(parent.transform.position);
            else realposition = (Vector3)stream.ReceiveNext();
        }
    }
}
