using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class SpawnPlayers : MonoBehaviourPunCallbacks, IPunObservable
{
    [Header("StartSettings")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float YPos = -0.62f;

    [SerializeField] private GameObject player;
    Vector2 spawnpoint = Vector2.zero;

    [Header("PlayerList")]
    [SerializeField] private GameObject playerListPrefab;
    [SerializeField] private Transform playerListContent;
    [SerializeField] private Text roomname;

    [Header("Timer")]
    [SerializeField] private GameObject timer;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject StartBt;
    [SerializeField] private float currCountdownValue = 4f;
    private float Timer;
    private bool canchange = false;

    [Header("LevelsPanel")]
    [SerializeField] private GameObject LevelsPanel;
    private bool levelsPanelOpen = true;

    [Header("Audio")]
    [SerializeField] private AudioClip cancel;
    [SerializeField] private AudioClip click;

    private PhotonView view;

    private void Awake() => view = GetComponent<PhotonView>();

    private void Start() {
        Timer = currCountdownValue;
        if (!PhotonNetwork.IsMasterClient) StartBt.SetActive(false);
        SpawnPlayer();
        ListUpdate();
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            LevelsPanel.SetActive(false);
            if ((bool)PhotonNetwork.CurrentRoom.CustomProperties["CanStart"] == true) { LevelsPanel.SetActive(false); startPanel.SetActive(false); }
            return;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                levelsPanelOpen = !levelsPanelOpen;
                LevelsPanel.SetActive(levelsPanelOpen);
            }
        }

        if (canchange == true)
        {
            Timer -= Time.deltaTime;
            if (Timer <= 0)
            {
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "CanStart", true } });
                if (Timer < -0.5f)
                //{
                //    PhotonView[] photonViews = FindObjectsOfType<PhotonView>();
                //    for (int i = 0; i < photonViews.Length; i++)
                //    {
                //        if (photonViews[i].gameObject.tag == "GameController") photonViews[i].RPC("TurnOff", RpcTarget.AllBuffered);
                //    }
                //}
                view.RPC("TurnOff", RpcTarget.All);
            }
            else timer.GetComponent<Text>().text = Timer.ToString("F1");
        } else if(PhotonNetwork.IsMasterClient && StartBt.activeSelf == false) StartBt.SetActive(true);
    }

    public void SpawnPlayer()
    {
        Player[] players = PhotonNetwork.PlayerList;
        spawnpoint = new Vector2(Random.Range(minX, maxX), YPos);

        if (players.Length <= 1) 
        {
            if (PhotonNetwork.InRoom) {
                GameObject pl = PhotonNetwork.Instantiate(player.name, spawnpoint, Quaternion.identity);
                pl.GetComponent<PlayerController>().enabled = true;
            }
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Position", spawnpoint.x} });
        }
        else
        {
            GameObject curPlayer = GameObject.FindGameObjectWithTag("GameController");
            if (curPlayer == null)
            {
                Vector2 vector = new Vector2((float)PhotonNetwork.CurrentRoom.CustomProperties["Position"], YPos);
                if (Vector2.Distance(vector, spawnpoint) > 2f && PhotonNetwork.InRoom)
                {
                    GameObject pl = PhotonNetwork.Instantiate(player.name, spawnpoint, Quaternion.identity);
                    pl.GetComponent<PlayerController>().enabled = true;
                }
                else SpawnPlayer();
            }
            else
            {
                try
                {
                    float playerpos1 = (float)PhotonNetwork.CurrentRoom.CustomProperties["PlayerPos1"];
                    float playerpos2 = (float)PhotonNetwork.CurrentRoom.CustomProperties["PlayerPos2"];

                    if ((Mathf.Abs(Mathf.Abs(spawnpoint.x) - Mathf.Abs(playerpos1)) > 2f) &&
                        (Mathf.Abs(Mathf.Abs(spawnpoint.x) - Mathf.Abs(playerpos2)) > 2f)) curPlayer.transform.position = spawnpoint;
                    else SpawnPlayer();
                }
                catch { return; }
            }
        }
    }

    public void LevelsMenuChangeThenLevelIsEnd()
    {
        levelsPanelOpen = true;
        LevelsPanel.SetActive(levelsPanelOpen);
    }

    private void ListUpdate()
    {
        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
           Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerList>().SetUp(players[i]);
        roomname.text = PhotonNetwork.CurrentRoom.Name;
    }

    public void StartBTN()
    {
        if (PhotonNetwork.PlayerList.Length >= 2)
        {
            view.RPC("Starting", RpcTarget.AllBuffered);
            GetComponent<AudioSource>().PlayOneShot(click);
        }
        else GetComponent<AudioSource>().PlayOneShot(cancel);
    }

    [PunRPC]
    private void Starting()
    {
        timer.SetActive(true);
        startPanel.SetActive(false);
        LevelsPanel.SetActive(false);
        canchange = true;
    }

    [PunRPC]
    private void TurnOff()
    {
        timer.SetActive(false);
        canchange = false;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(PhotonNetwork.InRoom) Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerList>().SetUp(newPlayer);
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && PhotonNetwork.IsMasterClient) stream.SendNext(Timer);
        else if (stream.IsReading)
        {
            Timer = (float)stream.ReceiveNext();
            if (Timer >= 0 && canchange == true) timer.GetComponent<Text>().text = Timer.ToString("F1");
        }
    }
}
