using Photon.Pun;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject MuteBtn;
    [SerializeField] private GameObject UnMuteBtn;
    [SerializeField] private GameObject SFXMuteBtn;
    [SerializeField] private GameObject SFXUnMuteBtn;
    [SerializeField] private GameObject PlayBtn;
    [SerializeField] private GameObject PauseBtn;
    [SerializeField] private GameObject PrevBtn;
    [SerializeField] private GameObject NextBtn;
    [SerializeField] private GameObject MusicSlider;
    [SerializeField] private GameObject SFXSlider;

    [SerializeField] private GameObject Musicpannel;
    public bool isMusic;

    [SerializeField] private Text changeText;

    public AudioClip[] audioClips;
    private AudioSource audioSource;
    private int current = 0;
    private bool isPlaying = true;
    private float Value;

    [SerializeField] private AudioClip click;
    private AudioSource sfxSource;
    private float SFXValue;

    [SerializeField] private AudioMixer audioMixer;

    private bool doCheck = true;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        sfxSource = GetComponentInChildren<AudioSource>();
        current = Random.Range(0, audioClips.Length);
        audioSource.clip = audioClips[current];
        isMusic = true; UnMute();
    }

    void Update()
    {
        if (!UnMuteBtn.activeSelf)
        {
            Value = MusicSlider.GetComponent<Slider>().value;
            Value = Mathf.Log10(Value) * 20;
            audioMixer.SetFloat("Music", Value);
        }
        if (!SFXUnMuteBtn.activeSelf)
        {
            SFXValue = SFXSlider.GetComponent<Slider>().value;
            SFXValue = Mathf.Log10(SFXValue) * 20;
            audioMixer.SetFloat("SFX", SFXValue);
        }
        if (PhotonNetwork.PlayerList.Length > 1 && doCheck == true)
        {
            try
            {
                GameObject[] gb = GameObject.FindGameObjectsWithTag("GameController");
                int count = 0;
                foreach (var item in gb){ if (item.GetComponent<PlayerController>().hashtableName == "PlayerPos1") count++; }
                //Debug.Log(count);
                if (count >= 1) Destroy(gb[1].GetComponent<AudioListener>());
                else Destroy(gb[1].GetComponent<AudioListener>());
                doCheck = false;
            }
            catch { }
        }
    }

    public void ChangeSoundType()
    {
        isMusic =! isMusic;
        if(isMusic == false) { Musicpannel.SetActive(false); SFXSlider.SetActive(true); changeText.text = "SFX"; }
        else { Musicpannel.SetActive(true); SFXSlider.SetActive(false); changeText.text = "Music"; }
        sfxSource.PlayOneShot(click);
    }

    #region Music
    public void Mute()
    {
        UnMuteBtn.SetActive(true);
        MuteBtn.SetActive(false);
        PlayBtn.SetActive(false);
        PauseBtn.SetActive(false);
        PrevBtn.SetActive(false);
        NextBtn.SetActive(false);
        MusicSlider.SetActive(false);
        audioSource.Pause();
        audioSource.mute = true;
    }

    public void UnMute()
    {
        MuteBtn.SetActive(true);
        UnMuteBtn.SetActive(false);
        PlayBtn.SetActive(false);
        PauseBtn.SetActive(true);
        PrevBtn.SetActive(true);
        NextBtn.SetActive(true);
        MusicSlider.SetActive(true);
        audioSource.Play();
        audioSource.mute = false;
    }

    public void Pause()
    {
        PlayBtn.SetActive(true);
        PauseBtn.SetActive(false);
        audioSource.Pause();
        isPlaying = false;
        audioSource.mute = true;
    }

    public void Play()
    {
        PlayBtn.SetActive(false);
        PauseBtn.SetActive(true);
        audioSource.Play();
        isPlaying = true;
        audioSource.mute = false;
    }

    public void Next()
    {
        audioSource.Stop();
        if (current == audioClips.Length - 1) current = 0;
        else current++;
        audioSource.clip = audioClips[current];
        if (isPlaying) { audioSource.mute = false; audioSource.Play(); }
    }

    public void Previous()
    {
        audioSource.Stop();
        if (current == 0) current = audioClips.Length - 1;
        else current--;
        audioSource.clip = audioClips[current];
        if (isPlaying) { audioSource.mute = false; audioSource.Play(); }
    }
    #endregion

    #region SFX
    public void MuteSFX()
    {
        SFXMuteBtn.SetActive(false);
        SFXUnMuteBtn.SetActive(true);
        audioMixer.SetFloat("SFX", Mathf.Log10(0.0001f) * 20);
    }

    public void UnMuteSFX()
    {
        SFXUnMuteBtn.SetActive(false);
        SFXMuteBtn.SetActive(true);
        audioMixer.SetFloat("SFX", SFXValue);
    }
    #endregion
}
