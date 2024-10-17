using UnityEngine;
using Pathfinding;

public class Chaising : MonoBehaviour
{
    [SerializeField] private AIPath aIPath;
    [SerializeField] private AIDestinationSetter setter;

    private GameObject player;
    private bool startChasing = false;

    private void FixedUpdate()
    {
        if (startChasing == true) setter.target = player.transform;
        else
        {
            if (Vector3.Distance(setter.target.position, transform.position) <= 0.6f)
            {
                Destroy(setter.target.gameObject);
                setter.target = null;
            }
            if(setter.target != null)
            {
                if (aIPath.desiredVelocity.x >= 0.01f) transform.localScale = new Vector3(1, 1, 1);
                if (aIPath.desiredVelocity.x <= -0.01f) transform.localScale = new Vector3(-1, 1, 1);
            }
            else transform.localScale = new Vector3(1, 1, 1);
        }

        if (setter.target != null && startChasing == true)
        {
            transform.localScale = new Vector3(1, 1, 1);
            if (transform.position.x >= player.transform.position.x)
            {
                Quaternion rot = Quaternion.Euler(transform.eulerAngles.x, 180, transform.eulerAngles.z);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, 0.5f + Time.fixedDeltaTime);
            }
            else
            {
                Quaternion rot = Quaternion.Euler(transform.eulerAngles.x, 0, transform.eulerAngles.z);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, 0.5f + Time.fixedDeltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "GameController") InTrigger(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "GameController") InTrigger(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "GameController")
        {
            startChasing = false;
            Vector2 pathPosition = GetComponent<MultiplyPaths>().PlayerPosGetter;
            setter.target = Instantiate(new GameObject("point"), pathPosition, Quaternion.identity).transform;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void InTrigger(Collider2D collision)
    {

        foreach (var gameObj in FindObjectsOfType(typeof(GameObject)) as GameObject[])
            if (gameObj.name == "point") Destroy(gameObj);

        startChasing = true;
        player = collision.gameObject;
    }
}
