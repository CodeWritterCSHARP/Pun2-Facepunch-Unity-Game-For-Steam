using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [Header("GroundChecking")]
    [SerializeField] private bool BirdBodyIsGround;
    [SerializeField] private bool BirdWingIsGround;
    [SerializeField] private bool BirdLegIsGround;

    [Header("Radius")]
    [SerializeField] private float radiusForBody = 1.5f;
    [SerializeField] private float radiusForWingAndLeg = 1f;

    [Header("All parametrs")]
    public GameObject Wing;
    private bool Getting;

    public GameObject Leg;

    public bool extraLife = false;
    public float speed = 8.5f;
    public float jump = 8;
    public float moveInput = 1;
    public float forcepower = 25;
    public float time = 0.5f;
    private float speedDecreeseTime = 1.5f;
    private int TimeValue = 2;
    private bool isSpeedDecreese = false;

    public LayerMask Ground;

    [SerializeField] private bool Movement = true;

    private Animator animator;
    private Rigidbody2D rigidbody;

    private PhotonView view;

    public string hashtableName = null;

    [SerializeField] private bool CanPlay = false;

    [SerializeField] private bool CanRestart = true;
    [SerializeField] private bool CanGetDamage = true;

    [SerializeField] private float AFKTimer = 16f;
    public int afkAnim = 0;

    [Header("Effects")]
    [SerializeField] private GameObject dustEffect;
    [SerializeField] private GameObject smokePart;
    [SerializeField] private GameObject bloodPart;
    [SerializeField] private GameObject bloodEffect;
    [SerializeField] private GameObject[] screenEffects;

    public InteractableObjectsList list;

    [Header("TimerOnLVL")]
    private TimerOnLVL timerOnLVL;

    private float extraLifeTimeDuation = 2.5f;
    private bool canChangeExtraLifeTime = false;

    private float curX, curY;

    [Header("Sound")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip defeatSound;
    [SerializeField] private AudioClip InWaterSound;

    private Vector3 remotePlayerPos;

    [SerializeField] private float force;

    [SerializeField] private GetAchivement jumpsAch;
    [SerializeField] private GetAchivement respawnAch;

    [SerializeField] private Vector3 siemenPos;

    private void Start()
    {
        timerOnLVL = FindObjectOfType<TimerOnLVL>().GetComponent<TimerOnLVL>();
        InvokeRepeating("CheckForStanning", 0f, 0.4f);
        for (int i = 0; i < screenEffects.Length; i++)
        {
            screenEffects[i].transform.SetParent(Camera.main.transform);
            screenEffects[i].transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 1);
        }
        siemenPos = GameObject.Find("Siemen").transform.position;
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        view = this.photonView;
        list = GameObject.Find("InteractableList").GetComponent<InteractableObjectsList>();
        if (PhotonNetwork.PlayerList.Length <= 1) hashtableName = "PlayerPos1"; else hashtableName = "PlayerPos2";
        list.ListRefresh();
    }

    void CheckForStanning()
    {
        if (Movement && BirdBodyIsGround && CanPlay)
        {
            if ((Mathf.Abs(moveInput) > 0.7) && (Mathf.Abs(transform.position.x) - Mathf.Abs(curX) <= 0.01) &&
                (Mathf.Abs(transform.position.y) - Mathf.Abs(curY) <= 0.001))
                transform.position = Vector3.Lerp(transform.position, new Vector2(transform.position.x + 0.55f, transform.position.y + 1.05f), 0.25f);
            curX = transform.position.x; curY = transform.position.y;
        }
    }

    void FixedUpdate()
    {
        if (CanPlay == false)
            if ((bool)PhotonNetwork.CurrentRoom.CustomProperties["CanStart"] == true) CanPlay = true;
        if (screenEffects[0].activeSelf) screenEffects[0].transform.localScale = Camera.main.transform.GetChild(2).transform.localScale;
    }

    public void Punch(Vector3 dir, bool state) { view.RPC("Punched", RpcTarget.AllBuffered, dir, state); }
    [PunRPC]
    private void Punched(Vector3 dir, bool state)
    {
        if(PhotonNetwork.IsMasterClient) Debug.Log("PunchIs" + state); else Debug.Log("PunchIsnt" + state);
        if (state == false)
        {
            Vector2 vel = (Vector2.up + (Vector2)dir) * jump;
            if (vel.x > 14) vel.x = 14; if (vel.x < -14) vel.x = -14;
            if (vel.y > 14) vel.y = 14; if (vel.y < -14) vel.y = -14;
            GetComponent<Rigidbody2D>().velocity = vel;
            Debug.Log(GetComponent<Rigidbody2D>().velocity);
        }
    }

    void Update()
    {
        if (!view.IsMine) return;

        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { hashtableName, gameObject.transform.position.x } });
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { hashtableName + "Y", gameObject.transform.position.y } });

        if (CanPlay == true)
        {
            AnimationChanging();
            WingGround();
            BodyGround();

            BirdLegIsGround = Physics2D.OverlapCircle(Leg.transform.position, radiusForWingAndLeg, Ground);

            if (isSpeedDecreese) { speedDecreeseTime -= Time.deltaTime; speed = 5.75f; }
            if (speedDecreeseTime <= 0) { isSpeedDecreese = false; speed = 8.5f; }

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space))
            {
                if (animator.GetBool("Jump") == false)
                {
                    Leg.transform.GetChild(0).GetComponent<LegPunch>().CheckPunch();
                }
            }

            if (Movement == true)
            {
                moveInput = Input.GetAxis("Horizontal");
                rigidbody.velocity = new Vector2(moveInput * speed, rigidbody.velocity.y);
                
                #region extraLifeTime
                if (canChangeExtraLifeTime) extraLifeTimeDuation -= Time.deltaTime;
                if (extraLifeTimeDuation <= 0)
                {
                    view.RPC("ExtraLifeChange", RpcTarget.AllBuffered, false);
                    extraLifeTimeDuation = 2.5f;
                    canChangeExtraLifeTime = false;
                }
                #endregion

                //if (this.moveInput == 0) this.AFKTimer -= Time.deltaTime;
                //else this.AFKTimer = 16f;

                if (time <= 0)
                {
                    TimeValue = 2;
                    time = 0.5f;
                }

                if (TimeValue == 1)
                {
                    time -= Time.deltaTime;
                    if (time <= 0.25f && BirdLegIsGround == true)
                    {                    
                        rigidbody.velocity = Vector2.up * jump;
                        jumpsAch.Count();
                        view.RPC("PlayAudio", RpcTarget.AllBuffered);
                        if (PhotonNetwork.InRoom) PhotonNetwork.Instantiate(dustEffect.name, new Vector2(Leg.transform.position.x, Leg.transform.position.y - 1f), Quaternion.identity);
                    }
                }

                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space))
                {
                    TimeValue = 1;
                    speedDecreeseTime = 1.5f;
                    isSpeedDecreese = true;
                    AFKTimer = 16f;
                }

                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) rigidbody.AddForce(Vector2.left * forcepower);
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) rigidbody.AddForce(Vector2.right * forcepower);

                #region AfkAnimations
                if (moveInput == 0) AFKTimer -= Time.deltaTime; else AFKTimer = 16f;
                if (AFKTimer <= 0)
                {
                    AFKTimer = 16f;
                    afkAnim = UnityEngine.Random.Range(1, 5);
                }
                #endregion
            }
        }
    }

    void AnimationChanging() => view.RPC("RPC_Anim", RpcTarget.AllBuffered);

    void BodyGround()
    {
        BirdBodyIsGround = Physics2D.OverlapCircle(this.gameObject.transform.position, radiusForBody, Ground);
        if (BirdBodyIsGround == true && Movement == true)
        {
            rigidbody.velocity = Vector2.up * 5;
            rigidbody.angularVelocity = rigidbody.angularVelocity + UnityEngine.Random.Range(-5, 5);
        }
    }

    void WingGround()
    {
        BirdWingIsGround = Physics2D.OverlapCircle(Wing.transform.position, radiusForWingAndLeg, Ground);

        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Getting = !Getting;
            if (Getting == true && BirdWingIsGround == true)
            {
              //  Movement = false;
            }
            if (Getting == false)
            {
                Movement = true;
            }
        }
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {

    }

    public void ActivateScreenEffect(int index, bool state) { if (view.IsMine) screenEffects[index].SetActive(state); else return; }

    [PunRPC]
    private void RPC_Anim()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.25f && animator.GetBool("Jump") == false && animator.GetBool("Catch") == true)
            animator.SetBool("Jump", true);

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.3f && animator.GetBool("Catch") == false && animator.GetBool("Jump") == true)
            animator.SetBool("Catch", true);

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)) { animator.SetBool("Jump", false); }

        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Keypad0)) animator.SetBool("Catch", false);

        if (AFKTimer > 0.5f) animator.SetInteger("Afk", 0); else animator.SetInteger("Afk", afkAnim);
    }

    [PunRPC]
    private void InWater() { if (!GetComponent<AudioSource>().isPlaying) GetComponent<AudioSource>().PlayOneShot(InWaterSound); }
    public void PlayingWaterAudio() { view.RPC("InWater", RpcTarget.AllBuffered); }
    [PunRPC] 
    private void PlayAudio(){ if(!GetComponent<AudioSource>().isPlaying) GetComponent<AudioSource>().PlayOneShot(jumpSound); }
    [PunRPC]
    private void PlayDefeatAudio() {Camera.main.GetComponent<AudioSource>().PlayOneShot(defeatSound); }
    public void PlayingDefeatAudio() => view.RPC("PlayDefeatAudio", RpcTarget.AllBuffered);

    [PunRPC]
    private void Dissolve_Anim(bool state) { animator.SetBool("Dissolve", state); }

    private void OnCollisionEnter2D(Collision2D collision) { InCollision(collision); }
    private void OnCollisionStay2D(Collision2D collision) { InCollision(collision); }

    private void InCollision(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && CanGetDamage == true)
        {
            if (extraLife == true) { canChangeExtraLifeTime = true; return; }
            if (extraLife == false)
            {
                CanGetDamage = false;
                if (CanRestart == true)
                {
                    PhotonView[] photonViews = FindObjectsOfType<PhotonView>();
                    for (int i = 0; i < photonViews.Length; i++)
                    {
                        if (photonViews[i].gameObject.tag == "GameController") {
                            photonViews[i].RPC("CR", RpcTarget.AllBuffered, false);
                            photonViews[i].RPC("PlayDefeatAudio", RpcTarget.AllBuffered);
                            photonViews[i].RPC("Dissolve_Anim", RpcTarget.All, false);
                        }
                    }
                    
                    if (collision.gameObject.CompareTag("Damage"))
                    {
                        if (PhotonNetwork.InRoom) PhotonNetwork.Instantiate(bloodPart.name, collision.contacts[0].point, Quaternion.identity);
                        if (PhotonNetwork.InRoom) PhotonNetwork.Instantiate(bloodEffect.name, collision.contacts[0].point, Quaternion.identity);
                    }
 
                    Invoke("InvokeRestart", .25f);
                }
            }
        }
    }

    public void InvokeRestart()
    {
        respawnAch.Count();
        PhotonView[] photonViews = FindObjectsOfType<PhotonView>();
        for (int i = 0; i < photonViews.Length; i++)
        {
            if (photonViews[i].gameObject.tag == "GameController") photonViews[i].RPC("Dissolve_Anim", RpcTarget.All, true);
        }
        //view.RPC("Dissolve_Anim", RpcTarget.AllBuffered, true);
       // list.ListSpawn();
        if (PhotonNetwork.PlayerList.Length > 1) {
            for (int i = 0; i < photonViews.Length; i++)
            {
                if (photonViews[i].gameObject.tag == "GameController")
                {
                    photonViews[i].RPC("Restart", RpcTarget.AllBuffered);
                }
            }
            //view.RPC("Restart", RpcTarget.AllBuffered);
        }
        else
        {
            gameObject.transform.position = new Vector2(UnityEngine.Random.Range(-12, -2), -0.62f);
            list.ListSpawn();
        }
        timerOnLVL.TimerGetSeter = 0;
        CanRestart = true;
        CanGetDamage = true;
        for (int i = 0; i < photonViews.Length; i++)
        {
            if (photonViews[i].gameObject.tag == "GameController") photonViews[i].RPC("CR", RpcTarget.AllBuffered, true);
            //if (photonViews[i].gameObject.tag == "GameController") photonViews[i].RPC("ListChange", RpcTarget.AllBuffered);
        }
       // view.RPC("CR", RpcTarget.AllBuffered, true);
    }

    [PunRPC]
    private void CR (bool change) { CanRestart = change; }

    [PunRPC]
    private void ExtraLifeChange (bool change) { extraLife = change; }

    //public void PlayerListChange() { if (PhotonNetwork.IsMasterClient) view.RPC("ListChange", RpcTarget.AllBuffered); }
    //[PunRPC]
    //private void ListChange() { list.ListSpawn(); list.ListRefresh(); }

    [PunRPC]
    private void Restart()
    {
        GameObject siemen = GameObject.Find("Siemen");
        siemen.transform.position = siemenPos;
        siemenPos = siemen.transform.position;
        Camera.main.transform.position = new Vector3(-14.2f, -2.3f, -1f);
        extraLife = false;
        extraLifeTimeDuation = 2.5f;
        canChangeExtraLifeTime = false;
        CanGetDamage = true;
        CanRestart = true;
        FindObjectOfType<SpawnPlayers>().SpawnPlayer();
        timerOnLVL.TimerGetSeter = 0;
        if (view.IsMine) { try { transform.GetChild(2).GetComponent<Grab>().UnGrabbingDeatf(); } catch { Debug.Log(transform.GetChild(2).name); } }
        try
        {
            GameObject[] crabs = GameObject.FindGameObjectsWithTag("Pos");
            if (crabs.Length > 0) { for (int i = 0; i < crabs.Length; i++) crabs[i].GetComponent<CrabMovement>().StatusRebuild(); }
        }
        catch { }

        GameObject[] birds = GameObject.FindGameObjectsWithTag("GameController");
        if (PhotonNetwork.IsMasterClient)
        {
            if (birds[0] == this.gameObject) { birds[1].GetComponent<PlayerController>().list = this.list; Debug.Log("Main 0"); }
            else { birds[0].GetComponent<PlayerController>().list = this.list; Debug.Log("Main 1"); }
        }
        else
        {
            if (birds[0] == this.gameObject) { this.list = birds[1].GetComponent<PlayerController>().list; Debug.Log("NotMain 0"); }
            else { this.list = birds[0].GetComponent<PlayerController>().list; Debug.Log("NotMain 1"); }
        }
        list.ListSpawn();

        for (int i = 0; i < screenEffects.Length; i++) screenEffects[i].SetActive(false);
    }
}
