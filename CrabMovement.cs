using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody2D))]
public class CrabMovement : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private Vector2 start;
    [SerializeField] private float end;
    public float timer = 5f;
    private bool changeDir = false;
    public bool canStart = false;
    private Rigidbody2D rb;

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    private void Start() => start = transform.position;

    private void FixedUpdate()
    {
        if (canStart == true && timer >= 0) timer -= Time.fixedDeltaTime;
        if (timer <= 0) rb.bodyType = RigidbodyType2D.Dynamic;

        if (canStart != true) return;
        if (Mathf.Abs(rb.velocity.x) <= speed && timer <= 0)
        {
            if (changeDir == false) {
                if (Mathf.Abs(gameObject.transform.position.x - end) > 1 && gameObject.transform.position.x > end) rb.AddForce(-Vector2.right * 60f);
                else changeDir = true;
            }
            if(changeDir == true)
            {
                if (Mathf.Abs(gameObject.transform.position.x - start.x) > 1 && gameObject.transform.position.x < start.x) rb.AddForce(Vector2.right * 60f);
                else changeDir = false;
            }
        }
    }


    public void StatusRebuild()
    {
        canStart = false; timer = 5f; rb.bodyType = RigidbodyType2D.Static;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GameController")) canStart = true;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GameController")) canStart = true;
    }
}
