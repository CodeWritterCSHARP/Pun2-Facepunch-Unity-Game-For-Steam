using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rigidbody;
    public Vector2 move;

    void Start() => rigidbody = GetComponent<Rigidbody2D>();

    void FixedUpdate()
    {
        move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (move != Vector2.zero)
        {
            if (move.x > 0) transform.Translate(Vector3.right * speed * Time.fixedDeltaTime);
            if (move.x < 0) transform.Translate(-Vector3.right * speed * Time.fixedDeltaTime);
            if (move.y > 0) transform.Translate(Vector3.up * speed * Time.fixedDeltaTime);
            if (move.y < 0) transform.Translate(-Vector3.up * speed * Time.fixedDeltaTime);
        }
        if (Input.GetKey(KeyCode.Q)) transform.Rotate(0, 0, transform.rotation.z + 2f, Space.Self);
        if (Input.GetKey(KeyCode.E)) transform.Rotate(0, 0, transform.rotation.z - 2f, Space.Self);
    }
}
