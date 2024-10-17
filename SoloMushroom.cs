using UnityEngine;

public class SoloMushroom : MonoBehaviour
{
    [SerializeField] private GameObject smokePart;

    private void Start() => ChangeOff(true);

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GameController"))
        {
            ChangeOff(false);
            Invoke("InstantiateObj", 0.5f);
        }
    }

    public void ChangeOff(bool status) => Off(status);

    private void InstantiateObj() => Instantiate(smokePart, transform.position, Quaternion.identity);

    private void Off(bool status)
    {
        if (status == true) gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        else gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        gameObject.GetComponent<Collider2D>().enabled = status;
    }
}
