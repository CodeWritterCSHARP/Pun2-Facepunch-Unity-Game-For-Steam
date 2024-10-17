using UnityEngine;

public class PortalSolo : MonoBehaviour
{
    [SerializeField] private int level;
    [SerializeField] private int playerAtEnd = 0;
    [SerializeField] private GameObject firework;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private AudioClip enterSound;
    [SerializeField] private AudioClip portal1;
    [SerializeField] private AudioClip portal2;
    [SerializeField] private AudioClip VinSound;
    public bool IsVinSound = true;

    float timeWaiting = 0;

    private void FixedUpdate()
    {
        if (timeWaiting <= 0)
        {
            timeWaiting = Random.Range(2.5f, 6f);
            int i = Random.Range(0, 2);
            switch (i)
            {
                case 0: GetComponent<AudioSource>().PlayOneShot(portal1); break;
                case 1: GetComponent<AudioSource>().PlayOneShot(portal2); break;
                default: break;
            }
        }
        else timeWaiting -= Time.fixedDeltaTime;
    }

    private void LateUpdate()
    {
        if (playerAtEnd >= 1)
        {
            var buttonsArray = FindObjectsOfType<ButtonsBehaviour>();
            if (IsVinSound == true)
            {
                PlayVinSound();
                Instantiate(firework, spawnPoint.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
                ChangeVin();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) { ChangeValue(collision, true); }

    private void OnTriggerExit2D(Collider2D collision) { ChangeValue(collision, false); }

    private void PlayEnterSound() { GetComponentInChildren<AudioSource>().PlayOneShot(enterSound); }

    private void PlayVinSound() { GetComponentInChildren<AudioSource>().PlayOneShot(VinSound); }

    private void ChangeVin() { IsVinSound = false; }

    void ChangeValue(Collider2D collision, bool add)
    {
        if (collision.gameObject.CompareTag("GameController"))
        {
            PlayEnterSound();
            if (add == true) playerAtEnd = 1; else playerAtEnd = 0;
        }
    }
}
