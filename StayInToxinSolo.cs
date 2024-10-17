using UnityEngine;

public class StayInToxinSolo : MonoBehaviour
{
    [SerializeField] private float timer;
    private float startTimer;
    private bool In = false;
    private GameObject player;
    private GameObject Lastplayer;

    private void Awake() => startTimer = timer;

    private void FixedUpdate()
    {
        if (In == true) timer -= Time.fixedDeltaTime;
        if (timer <= 0)
        {
            In = false; timer = startTimer;
            player.GetComponent<PlayerSoloController>().PlayDefeatAudio();
            player.GetComponent<PlayerSoloController>().Invoke("InvokeRestart", .01f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GameController"))
        {
            player = collision.gameObject;
            Lastplayer = player;
            In = true;
            player.GetComponent<PlayerSoloController>().PlayingWaterAudio();
            player.GetComponent<PlayerSoloController>().ActivateScreenEffect(0, true);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("GameController")) { player = collision.gameObject; Lastplayer = player; In = true; }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("GameController"))
        {
            player = collision.gameObject;
            player.GetComponent<PlayerSoloController>().ActivateScreenEffect(0, false);
            if (player == Lastplayer) { timer = startTimer; In = false; }
            player = null;
        }
    }
}
