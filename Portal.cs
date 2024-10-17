using UnityEngine;
using Photon.Pun;

public class Portal : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private int level;
    [SerializeField] private float timeForLVL;
    [SerializeField] private int playerAtEnd = 0;
    [SerializeField] private bool isSiemen = false;
    [SerializeField] private GameObject firework;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private AudioClip enterSound;
    [SerializeField] private AudioClip portal1;
    [SerializeField] private AudioClip portal2;
    [SerializeField] private AudioClip VinSound;
    public bool IsVinSound = true;

    float timeWaiting = 0;

    private PhotonView view;

    private void Awake() => view = GetComponent<PhotonView>();

    private void FixedUpdate()
    {
        if(timeWaiting <= 0)
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
        if (playerAtEnd >= 2)
        {
            if (PhotonNetwork.IsMasterClient) FindObjectOfType<SpawnPlayers>().LevelsMenuChangeThenLevelIsEnd();
            PlayerData data = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerData>();

            string _stars = "1";
            float timerOnLVL = FindObjectOfType<TimerOnLVL>().TimerGetSeter;

            if (isSiemen) _stars += "1"; else _stars += "0";

            if (data.times[level - 1] < timeForLVL && data.times[level - 1] != 0) _stars += "1";
            else { if (timerOnLVL < timeForLVL) _stars += "1"; else _stars += "0"; }

            data.star[level - 1] = _stars;
            if (data.times[level - 1] != 0)
            {
                if (data.times[level - 1] > timerOnLVL) data.times[level - 1] = timerOnLVL;
            }
            else data.times[level - 1] = timerOnLVL;

            data.Recount();

            Transform content = FindObjectOfType<ButtonsSpawn>().transform;
            for (int i = 0; i < content.childCount; i++) content.GetChild(i).GetComponent<ButtonsBehaviour>().TextUpdate();
            
            FindObjectOfType<SavingAndLoadSystem>().Save();

            var buttonsArray = FindObjectsOfType<ButtonsBehaviour>();
            for (int i = 0; i < buttonsArray.Length; i++) buttonsArray[i].TextUpdate();
            playerAtEnd = 0;
            isSiemen = false;
            if (IsVinSound == true) {
                view.RPC("PlayVinSound", RpcTarget.AllBuffered);
                PhotonNetwork.Instantiate(firework.name, spawnPoint.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
                view.RPC("ChangeVin", RpcTarget.AllBuffered);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) { stream.SendNext(playerAtEnd); stream.SendNext(isSiemen); }
        else { if (stream.IsReading) { playerAtEnd = (int)stream.ReceiveNext(); isSiemen = (bool)stream.ReceiveNext(); } }
    }

    private void OnTriggerEnter2D(Collider2D collision) { ChangeValue(collision, true); }

    private void OnTriggerExit2D(Collider2D collision) { ChangeValue(collision, false); }

    [PunRPC]
    private void PlayEnterSound() { GetComponentInChildren<AudioSource>().PlayOneShot(enterSound); }

    [PunRPC]
    private void PlayVinSound() { GetComponentInChildren<AudioSource>().PlayOneShot(VinSound); }

    [PunRPC]
    private void ChangeVin() { IsVinSound = false; }

    void ChangeValue(Collider2D collision, bool add)
    {
        if (PhotonNetwork.PlayerList.Length > 1)
        {
            if (collision.gameObject.CompareTag("GameController"))
            {
                view.RPC("PlayEnterSound", RpcTarget.AllBuffered);
                if (add) playerAtEnd++; else playerAtEnd--;
            }
            if (collision.gameObject.name == "Siemen")
            {
                view.RPC("PlayEnterSound", RpcTarget.AllBuffered);
                if (add) isSiemen = true; else isSiemen = false;
            }
        }
        else
        {
            if (collision.gameObject.CompareTag("GameController") || collision.gameObject.name == "Siemen") view.RPC("PlayEnterSound", RpcTarget.AllBuffered);
        }
    }
}
