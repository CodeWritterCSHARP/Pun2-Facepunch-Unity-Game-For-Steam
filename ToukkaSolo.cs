using UnityEngine;

public class ToukkaSolo : MonoBehaviour
{
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject extraText;
    [SerializeField] private AudioClip grabSound;

    private Transform parent;

    private void Start() => parent = this.gameObject.transform.parent;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Toukka") Doing(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Toukka") Doing(collision);
    }

    private void Doing(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Keypad0))
        {
            ChangeExtraLife();
            GameObject intatinable = Instantiate(shield, new Vector3(parent.position.x, parent.position.y, 7), Quaternion.identity);
            Instantiate(extraText, collision.transform.position, Quaternion.identity);
            intatinable.transform.SetParent(parent);
            collision.gameObject.GetComponent<SoloDestroy>().enabled = true;
        }
    }

    void ChangeExtraLife()
    {
        parent.transform.GetComponent<AudioSource>().PlayOneShot(grabSound);
        parent.transform.GetComponent<PlayerSoloController>().extraLife = true;
    }
}
