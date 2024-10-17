using System.Collections;
using UnityEngine;

public class ProjectileSolo : MonoBehaviour
{
    [SerializeField] private GameObject Prefab;
    [SerializeField] private float shootWaitingTime;
    [SerializeField] private float bulletFlyingTime;
    [SerializeField] Transform frowingPoint;

    private Rigidbody2D projectile;

    private bool canShoot = false;
    private bool coroutuneCheck = true;

    private float delay;
    private int playersInRange;

    private GameObject player;

    private Animator animator;

    private IEnumerator ShootWaiting(float waitTime)
    {
        coroutuneCheck = false;
        yield return new WaitForSeconds(waitTime);
        coroutuneCheck = true;

        if (canShoot == true && player != null && delay <= 0)
        {
            RotationChange();
            projectile = Instantiate(Prefab, new Vector3(frowingPoint.position.x, frowingPoint.position.y, 0.2f), gameObject.transform.rotation).GetComponent<Rigidbody2D>();
            Vector3 Velocity = CalculatorOfVelocity(player.transform.position, projectile.position, bulletFlyingTime);
            projectile.velocity = Velocity;
        }
        delay = shootWaitingTime - 1;
        Animate(false);
    }

    private void Awake() => animator = GetComponent<Animator>();
    private void Start() => delay = shootWaitingTime - 1;

    void RotationChange()
    {
        if (player.transform.position.x < transform.position.x) transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        else transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
    }

    void Animate(bool type) { animator.SetBool("set", type); }

    private void Update()
    {
        if (canShoot == true && coroutuneCheck == true && delay == (shootWaitingTime - 1)) StartCoroutine(ShootWaiting(shootWaitingTime));
        if (coroutuneCheck == false) delay -= Time.deltaTime;
        if (delay <= 0) Animate(true);
        if (canShoot == false) { Animate(false); delay = shootWaitingTime - 1; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "GameController") { canShoot = true; player = collision.gameObject; }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "GameController") { canShoot = true; player = collision.gameObject; }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "GameController") canShoot = false;
    }

    Vector3 CalculatorOfVelocity(Vector3 target, Vector3 origin, float time)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0;

        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;

        float Vxz = Sxz / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;
        return result;
    }
}
