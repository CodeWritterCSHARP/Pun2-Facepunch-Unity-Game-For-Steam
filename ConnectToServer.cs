using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject disableObject;
    [SerializeField] private GameObject EnableObject;
    [SerializeField] private InputField PlayerName;

    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip cancel;

    public void TransitionBtwScenes()
    {
        if (!string.IsNullOrEmpty(PlayerName.text))
        {
            GetComponent<AudioSource>().PlayOneShot(click);
            disableObject.SetActive(false);
            EnableObject.SetActive(true);
            PhotonNetwork.NickName = PlayerName.text;
            PhotonNetwork.ConnectUsingSettings();
        } else GetComponent<AudioSource>().PlayOneShot(cancel);
    }

    public void SoloLoad()
    {
        if (!string.IsNullOrEmpty(PlayerName.text))
        {
            GetComponent<AudioSource>().PlayOneShot(click);
            SceneManager.LoadScene("Solo");
        }
        else GetComponent<AudioSource>().PlayOneShot(cancel);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if(!string.IsNullOrEmpty(PlayerName.text)) TransitionBtwScenes();
            else GetComponent<AudioSource>().PlayOneShot(cancel);
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}
