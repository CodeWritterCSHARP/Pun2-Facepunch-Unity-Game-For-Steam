using UnityEngine;

public class SoloGrab : MonoBehaviour
{
    public bool isGrabbing = false;
    private Transform parent;
    [SerializeField] private Transform grabpoint;
    private Vector3 dist;

    float changetimer = 1f;

    [SerializeField] private AudioClip grabSound;

    private void Start() => parent = GameObject.FindGameObjectWithTag("Hand").transform.parent.transform;

    private void Update()
    {
        parent = GameObject.FindGameObjectWithTag("Hand").transform.parent.transform;
        if (parent.GetComponent<CapsuleCollider2D>().enabled == false)
        {
            if (isGrabbing == true)
            {
                changetimer -= 0.25f;
                if (changetimer <= 0) { changetimer = 0; UnGrabbing(); }
                parent.position = grabpoint.position;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Hand") Doing(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hand") Doing(collision);
    }

    public void UnGrabbingDeatf()
    {
        foreach (Transform child in parent) { child.position -= dist; }
        isGrabbing = false;
        changetimer = 1;
        ColliderChange(true, false);
    }

    private void UnGrabbing()
    {
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Keypad0))
        {
            foreach (Transform child in parent) { child.position -= dist; }
            isGrabbing = false;
            changetimer = 1;
            ColliderChange(true, true);
        }
    }

    void ColliderChange(bool status, bool playingA)
    {
        if (playingA == true) transform.parent.GetComponent<AudioSource>().PlayOneShot(grabSound);
        if (parent != null)
        {
            parent.GetComponent<CapsuleCollider2D>().enabled = status;
        }
    }

    void Doing(Collider2D collision)
    {
        if ((Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Keypad0)) && isGrabbing == false)
        {
            parent = collision.transform.parent;
            parent.transform.position = grabpoint.transform.position;

            dist = parent.position - collision.transform.position;

            foreach (Transform child in parent) { child.position += dist; }

            ColliderChange(false, true);
            isGrabbing = true;
        }
    }
}
