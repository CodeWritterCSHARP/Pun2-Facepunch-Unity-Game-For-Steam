using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    [SerializeField] private float speedValueChange = 0.025f;
    private float speed;
    private float startSpeed;
    private float speedTimeChange = 0f;

    public float SpeedGetSet { get => startSpeed; set => startSpeed = value; }

    void FixedUpdate()
    {
        speedTimeChange -= Time.fixedDeltaTime;
        if (speedTimeChange <= 0)
        {
            speed = Random.Range(startSpeed - speedValueChange, startSpeed + speedValueChange);
            speedTimeChange = Random.Range(3, 6);
        }

        transform.Translate(Vector2.right * speed);
    }
}
