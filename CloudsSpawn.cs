using Photon.Pun;
using UnityEngine;

public class CloudsSpawn : MonoBehaviourPun
{
    private Camera camera;

    private BoxCollider2D boxCollider;

    [Header("Timer")]
    [SerializeField] private float timerMin;
    [SerializeField] private float timerMax;
    private float timer;

    [Header("Speed")]
    [SerializeField] private float speedMin;
    [SerializeField] private float speedMax;

    [Header("CloudLifeTime")]
    [SerializeField] private float liteTimeMin;
    [SerializeField] private float liteTimeMax;

    [SerializeField] private GameObject[] clouds;

    private void Start()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        Vector2 bound = camera.ViewportToWorldPoint(new Vector2(0, 1));
        transform.position = bound;
        transform.localScale = new Vector3(camera.orthographicSize, camera.orthographicSize * .75f, 1);

        timer -= Time.fixedDeltaTime;

        if (timer <= 0)
        {
            timer = Random.Range(timerMin, timerMax);

            float boundMaxX = boxCollider.bounds.max.x;
            float boundMinX = boxCollider.bounds.center.x;
            float boundMaxY = boxCollider.bounds.max.y;
            float boundMinY = boxCollider.bounds.min.y;
            Vector2 spawnVector = new Vector2(Random.Range(boundMinX, boundMaxX), Random.Range(boundMinY, boundMaxY));

            int i = Random.Range(0, clouds.Length);
            if (PhotonNetwork.InRoom)
            {
                GameObject cloud = PhotonNetwork.Instantiate(clouds[i].name, spawnVector, Quaternion.identity);
                float scale = Random.Range(2, 4.5f);
                cloud.transform.localScale = new Vector3(scale, scale, 1);
                cloud.GetComponent<CloudMovement>().SpeedGetSet = Random.Range(speedMin, speedMax);
                cloud.GetComponent<DestroyInNetwork>().TimeGetSet = Random.Range(liteTimeMin, liteTimeMax);
            }
        }
    }
}
