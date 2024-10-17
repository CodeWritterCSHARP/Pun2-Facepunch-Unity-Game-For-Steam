using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineMovement : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float freq = 6.25f;
    [SerializeField] private float magnitude = 0.4f;
    [SerializeField] private float distanceToDestoy = 2.5f;
    private Camera camera;
    private Vector3 pos;

    void Start() => RandomGenerate();

    void Update() => Move();

    void FixedUpdate() => CheckForBorder();

    private void Move()
    {
        pos -= transform.right * Time.deltaTime * speed;
        transform.position = pos + transform.up * Mathf.Sin(Time.time * freq) * magnitude;
    }

    private void RandomGenerate()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        pos = transform.position;
        speed = Random.Range(2.5f, 3.5f);
        freq = Random.Range(5.5f, 6.75f);
        magnitude = Random.Range(0.3f, 0.5f);
    }

    private void CheckForBorder()
    {
        Vector2 bound = camera.ViewportToWorldPoint(new Vector2(0, 0f));
        if (transform.position.x < bound.x - distanceToDestoy) Destroy(this.gameObject);
    }
}
