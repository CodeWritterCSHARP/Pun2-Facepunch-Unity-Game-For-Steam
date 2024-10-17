using System.Linq;
using UnityEngine;

public class CollisionSoundSolo : MonoBehaviour
{
    [SerializeField] private int[] layerNum;
    [SerializeField] private string[] layerName;
    [SerializeField] private AudioClip sound;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (layerNum.Contains(collision.gameObject.layer) || layerName.Contains(collision.gameObject.tag)) PlaySound();
    }

    void PlaySound()
    {
        GetComponent<AudioSource>().PlayOneShot(sound);
    }
}
