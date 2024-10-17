using UnityEngine;

public class ParticleCollisionSolo : MonoBehaviour
{
    private bool canDamage = false;
    [SerializeField] private float timer = 4f;

    private void Start() => Invoke("canDamageChange", 0.5f);

    private void FixedUpdate()
    {
        timer -= Time.fixedDeltaTime;
        if (timer <= 0) Destroing();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("GameController") && canDamage)
        {
            canDamage = false;
            other.GetComponent<PlayerSoloController>().Invoke("InvokeRestart", .25f);
            other.GetComponent<PlayerSoloController>().PlayDefeatAudio();
            Destroing();
        }
    }

    private void canDamageChange() { canDamage = true; }

    private void Destroing() => Destroy(gameObject);
}
