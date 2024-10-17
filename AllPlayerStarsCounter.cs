using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class AllPlayerStarsCounter : MonoBehaviour
{
    [SerializeField] private Text text;
    private PlayerData data;

    void Start() { if (PhotonNetwork.IsMasterClient) Search(); }

    private void FixedUpdate()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (data != null) text.text = data.allStars.ToString(); else Search();
    }

    private void Search() {
        try { data = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerData>(); } catch { }
    }
}
