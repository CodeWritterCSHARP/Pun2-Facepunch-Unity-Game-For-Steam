using Photon.Pun;
using UnityEngine;

public class BirdSounds : MonoBehaviour
{
    [SerializeField] private float timerMin = 0f;
    [SerializeField] private float timerMax = 0f;
    [SerializeField] private float timer = 0f;

    public AudioClip[] audioClips;

    private void Start() => timer = Random.Range(timerMin, timerMax);

    private void FixedUpdate()
    {
        timer -= Time.fixedDeltaTime;
        if (timer <= 0)
        {
            timer = Random.Range(timerMin, timerMax);
            int i = Random.Range(0, audioClips.Length);
            GetComponent<AudioSource>().PlayOneShot(audioClips[i]);
        }
    }
}
