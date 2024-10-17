using UnityEngine;
using Photon.Pun;

public class ParticleCollision : MonoBehaviour
{
    private bool canDamage = false;
    [SerializeField] private float timer = 4f;

    private void Start() => Invoke("canDamageChange", 0.5f);

    private void FixedUpdate()
    {
        timer -= Time.fixedDeltaTime;
        if (timer <= 0) GetComponent<PhotonView>().RPC("Destroing", RpcTarget.All);
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("GameController") && canDamage)
        {
            canDamage = false;
            other.GetComponent<PlayerController>().Invoke("InvokeRestart", .25f);
            other.GetComponent<PlayerController>().PlayingDefeatAudio();
            GetComponent<PhotonView>().RPC("Destroing", RpcTarget.All);
        }
    }

    private void canDamageChange() { canDamage = true; }

    [PunRPC]
    private void Destroing() => Destroy(gameObject);
}
