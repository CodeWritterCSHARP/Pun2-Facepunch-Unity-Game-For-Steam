using UnityEngine;

public class MovementToPoint : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform target;
    [SerializeField] private bool twoSided = false;
    private Vector3 startPos;
    private bool side = false;

    private void Start() { if (twoSided == true) startPos = transform.position; }

    private void Update()
    {
        if (twoSided == false) transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        else
        {
            if (side == false) transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            else transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);
            if (transform.position.y == target.position.y) side = true;
            if (transform.position.y == startPos.y) side = false;
        }
    }
}
