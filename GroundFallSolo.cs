using UnityEngine;

public class GroundFallSolo : MonoBehaviour
{
    [SerializeField] private AudioClip sound;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.CompareTag("Wheel")) Activate();
    }

    void Activate()
    {
        if (GetComponent<BoxCollider2D>().enabled == true)
        {
            foreach (Transform child in gameObject.transform)
            {
                Rigidbody2D rigidbody2D = child.GetComponent<Rigidbody2D>();
                rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                rigidbody2D.AddForce(-transform.up * Random.Range(5, 10), ForceMode2D.Impulse);
                rigidbody2D.AddForce(transform.right * Random.Range(-10, 10), ForceMode2D.Impulse);
                rigidbody2D.AddTorque(Random.Range(-80, 80) * Mathf.Deg2Rad * rigidbody2D.inertia, ForceMode2D.Impulse);
            }
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<AudioSource>().PlayOneShot(sound);
        }
    }
}
