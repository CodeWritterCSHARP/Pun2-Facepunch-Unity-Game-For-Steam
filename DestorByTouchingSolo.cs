using UnityEngine;

public class DestorByTouchingSolo : MonoBehaviour
{
    public float timer = 4f;

    private void FixedUpdate()
    {
        timer -= Time.fixedDeltaTime;
        if (timer <= 0) Destroing();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GameController")) Destroing();
    }

    private void Destroing() => Destroy(gameObject);
}
