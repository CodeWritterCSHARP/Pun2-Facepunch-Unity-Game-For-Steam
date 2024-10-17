using UnityEngine;

public class BaloonMovement : MonoBehaviour
{
    [SerializeField] private float smoothSpeed;
    [SerializeField] private float speed;
    [SerializeField] private float rotSpeed;
    private float x;
    private float y;
    private Vector2 currentDampVelocity;

    private void Update()
    {
        x += Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        y += Input.GetAxis("Vertical") * Time.deltaTime * speed;

        Vector2 playerInput = new Vector2(x, y);
        Vector2 moveVector = transform.TransformDirection(playerInput);

        transform.position = Vector2.SmoothDamp(transform.position, moveVector, ref currentDampVelocity, smoothSpeed);

        if (Input.GetAxis("Horizontal") != 0)
        {
            float input = Input.GetAxis("Horizontal");
            Quaternion targetrotation = new Quaternion();
            if (x >= 0) targetrotation = Quaternion.LookRotation(transform.forward, playerInput * input);
            else targetrotation = Quaternion.LookRotation(transform.forward, -playerInput * input);

            Quaternion rot = Quaternion.RotateTowards(transform.rotation, targetrotation, rotSpeed * Time.deltaTime);
            if (rot.z >= 0.25) rot.z = 0.25f;
            if (rot.z <= -0.25) rot.z = -0.25f;
            transform.rotation = rot;
        }
        else transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0,0,0), rotSpeed * 1.25f * Time.deltaTime);
    }
}
