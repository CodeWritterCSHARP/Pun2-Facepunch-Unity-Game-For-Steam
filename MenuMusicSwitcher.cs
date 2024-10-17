using System.Collections;
using UnityEngine;

public class MenuMusicSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject MuteBtn;
    [SerializeField] private GameObject UnMuteBtn;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clip1;
    [SerializeField] private AudioClip clip2;
    private bool mute = false;

    private void Start()
    {
        source.clip = clip1;
        source.Play();
        if (clip2 != null) StartCoroutine(playAnotherAudio());
    }

    public void ChangeSoundVolume()
    {
        mute = !mute;
        if (mute == true)
        {
            MuteBtn.SetActive(false);
            UnMuteBtn.SetActive(true);
            source.Pause();
            source.mute = true;
        }
        else
        {
            MuteBtn.SetActive(true);
            UnMuteBtn.SetActive(false);
            source.Play();
            source.mute = false;
        }
    }

    private IEnumerator playAnotherAudio()
    {
        yield return new WaitForSeconds(source.clip.length);
        if (source.clip == clip1) source.clip = clip2; else source.clip = clip1;
        source.Play();

        if (!MuteBtn.activeSelf)
        {
            source.Pause();
            source.mute = true;
        }

        StartCoroutine(playAnotherAudio());
    }
}
