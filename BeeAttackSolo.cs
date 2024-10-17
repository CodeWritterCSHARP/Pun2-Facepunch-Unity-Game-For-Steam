using UnityEngine;

public class BeeAttackSolo : MonoBehaviour
{
    public LayerMask Ground;

    [SerializeField] private float timer = 2f;
    [SerializeField] private float speed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float radius;
    [SerializeField] private AudioClip[] sounds;
    [SerializeField] private Transform circlePoint;

    private GameObject player;

    private bool startChasing = false;
    private bool isGround;

    private float timerStart;

    private Rigidbody2D rigidbody;
    private Animator animator;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start() => timerStart = timer;

    private void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(circlePoint.position, radius, Ground);
        if (isGround == true) Deactivate();

        if (startChasing)
        {
            timer -= Time.fixedDeltaTime;
            if (timer <= 0 && isGround == false) Move();
        }
        else
        {
            timer = timerStart;
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.x, transform.eulerAngles.z);
            Animation(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "GameController") { startChasing = true; player = collision.gameObject; PlaySound(1); }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "GameController") { startChasing = true; player = collision.gameObject; }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "GameController")
        {
            startChasing = false;
            timer = timerStart;
            PlaySound(0);
            if (rigidbody != null) rigidbody.bodyType = RigidbodyType2D.Static;
        }
    }

    private void Deactivate()
    {
        GetComponentInChildren<CapsuleCollider2D>().enabled = false;
        Destroy(rigidbody);
        rigidbody = this.gameObject.AddComponent<Rigidbody2D>();
        rigidbody.gravityScale = 0;
        PlaySound(0);
        GetComponent<BeeAttackSolo>().enabled = false;
    }

    private void PlaySound(int i) { GetComponent<AudioSource>().clip = sounds[i]; GetComponent<AudioSource>().Play(); }

    private void Animation(bool type) => animator.SetBool("Attack", type);

    private void Move()
    {
        Animation(true);
        rigidbody.bodyType = RigidbodyType2D.Dynamic;
        Vector3 target = player.transform.position;
        Vector2 direction = (Vector2)target - rigidbody.position;
        direction.Normalize();
        float rotate = Vector3.Cross(direction, transform.up).z;
        rigidbody.angularVelocity = -rotate * rotateSpeed;
        rigidbody.velocity = transform.up * speed;


        if (transform.position.x >= target.x)
        {
            Quaternion rot = Quaternion.Euler(transform.eulerAngles.x, 180, transform.eulerAngles.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 0.5f + Time.fixedDeltaTime);
        }
        else
        {
            Quaternion rot = Quaternion.Euler(transform.eulerAngles.x, 0, transform.eulerAngles.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 0.5f + Time.fixedDeltaTime);
        }

        if (transform.position.x == target.x && transform.position.y == target.y) Deactivate();
    }
}
