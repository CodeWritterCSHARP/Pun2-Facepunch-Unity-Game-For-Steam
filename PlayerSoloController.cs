using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSoloController : MonoBehaviour
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

    private Animator animator;
    private Rigidbody2D rigidbody;

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

    public SoloList list;

    private float extraLifeTimeDuation = 2.5f;
    private bool canChangeExtraLifeTime = false;

    private float curX, curY;

    [Header("Sound")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip defeatSound;
    [SerializeField] private AudioClip InWaterSound;

    private Vector3 remotePlayerPos;

    [SerializeField] private float force;

    private float timer;
    [SerializeField] private Text text;

    [SerializeField] private GetAchivement jumpsAch;
    [SerializeField] private GetAchivement respawnAch;

    private void Start()
    {
        InvokeRepeating("CheckForStanning", 0f, 0.4f);
        for (int i = 0; i < screenEffects.Length; i++)
        {
            screenEffects[i].transform.SetParent(Camera.main.transform);
            screenEffects[i].transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 1);
        }
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        list = GameObject.Find("InteractableList").GetComponent<SoloList>();
        list.ListRefresh();
        text = GameObject.Find("TimerOnLVL").GetComponent<Text>();
    }

    void CheckForStanning()
    {
        if (BirdBodyIsGround)
        {
            if ((Mathf.Abs(moveInput) > 0.7) && (Mathf.Abs(transform.position.x) - Mathf.Abs(curX) <= 0.01) &&
                (Mathf.Abs(transform.position.y) - Mathf.Abs(curY) <= 0.001))
                transform.position = Vector3.Lerp(transform.position, new Vector2(transform.position.x + 0.55f, transform.position.y + 1.05f), 0.25f);
            curX = transform.position.x; curY = transform.position.y;
        }
    }

    void FixedUpdate()
    {
        if (screenEffects[0].activeSelf) screenEffects[0].transform.localScale = Camera.main.transform.GetChild(2).transform.localScale;
    }

    void Update()
    {
        timer += Time.deltaTime;
        text.text = timer.ToString("F1");

        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);

        RPC_Anim();
        WingGround();
        BodyGround();

        BirdLegIsGround = Physics2D.OverlapCircle(Leg.transform.position, radiusForWingAndLeg, Ground);

        if (isSpeedDecreese) { speedDecreeseTime -= Time.deltaTime; speed = 5.75f; }
        if (speedDecreeseTime <= 0) { isSpeedDecreese = false; speed = 8.5f; }

        moveInput = Input.GetAxis("Horizontal");
        rigidbody.velocity = new Vector2(moveInput * speed, rigidbody.velocity.y);

        #region extraLifeTime
        if (canChangeExtraLifeTime) extraLifeTimeDuation -= Time.deltaTime;
        if (extraLifeTimeDuation <= 0)
        {
            extraLife = false;
            extraLifeTimeDuation = 2.5f;
            canChangeExtraLifeTime = false;
        }
        #endregion

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
                PlayAudio();
                Instantiate(dustEffect, new Vector2(Leg.transform.position.x, Leg.transform.position.y - 1f), Quaternion.identity);
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
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow)) rigidbody.AddForce(Vector2.right * forcepower);

        #region AfkAnimations
        if (moveInput == 0) AFKTimer -= Time.deltaTime; else AFKTimer = 16f;
        if (AFKTimer <= 0)
        {
            AFKTimer = 16f;
            afkAnim = UnityEngine.Random.Range(1, 5);
        }
        #endregion

    }

    void Respawn() => transform.position = new Vector2(Random.Range(-12f, -4f), 0.1f);

    void BodyGround()
    {
        BirdBodyIsGround = Physics2D.OverlapCircle(this.gameObject.transform.position, radiusForBody, Ground);
        if (BirdBodyIsGround == true)
        {
            rigidbody.velocity = Vector2.up * 5;
            rigidbody.angularVelocity = rigidbody.angularVelocity + UnityEngine.Random.Range(-5, 5);
        }
    }

    void WingGround()
    {
        BirdWingIsGround = Physics2D.OverlapCircle(Wing.transform.position, radiusForWingAndLeg, Ground);
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Keypad0)) Getting = !Getting;
    }

    public void ActivateScreenEffect(int index, bool state) { screenEffects[index].SetActive(state);}

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

    public void PlayingWaterAudio() { if (!GetComponent<AudioSource>().isPlaying) GetComponent<AudioSource>().PlayOneShot(InWaterSound); }

    private void PlayAudio() { if (!GetComponent<AudioSource>().isPlaying) GetComponent<AudioSource>().PlayOneShot(jumpSound); }

    public void PlayDefeatAudio() => Camera.main.GetComponent<AudioSource>().PlayOneShot(defeatSound);

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

                    CR(false);
                    PlayDefeatAudio();
                    Dissolve_Anim(false);

                    if (collision.gameObject.CompareTag("Damage"))
                    {
                        Instantiate(bloodPart, collision.contacts[0].point, Quaternion.identity);
                        Instantiate(bloodEffect, collision.contacts[0].point, Quaternion.identity);
                    }
                    Invoke("InvokeRestart", .25f);
                }
            }
        }
    }

    public void InvokeRestart()
    {
        Dissolve_Anim(true);
        Restart();
        CR(true);
    }

    private void CR(bool change) { CanRestart = change; }

    private void Restart()
    {
        extraLife = false;
        extraLifeTimeDuation = 2.5f;
        canChangeExtraLifeTime = false;
        CanGetDamage = true;
        CanRestart = true;
        timer = 0;
        respawnAch.Count();
        Respawn();
        { try { transform.GetChild(2).GetComponent<SoloGrab>().UnGrabbingDeatf(); } catch { Debug.Log(transform.GetChild(2).name); } }
        try
        {
            GameObject[] crabs = GameObject.FindGameObjectsWithTag("Pos");
            if (crabs.Length > 0) { for (int i = 0; i < crabs.Length; i++) crabs[i].GetComponent<CrabMovement>().StatusRebuild(); }
        }
        catch { }

        list.ListSpawn();
        for (int i = 0; i < screenEffects.Length; i++) screenEffects[i].SetActive(false);
    }
}

