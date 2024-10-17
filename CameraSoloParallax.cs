using UnityEngine;

public class CameraSoloParallax : MonoBehaviour
{
    [SerializeField] private GameObject backGround;
    [SerializeField] private Vector2 parallaxEffect;
    [SerializeField] private GameObject backGroundSpawn;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject LevelsPanel;
    private float textureUnitSize;
    private Camera camera;
    private Vector3 lastCamposition;
    private bool levelsPanelOpen = false;
    private bool CameraValueState = false;

    private void Start()
    {
        camera = GetComponent<Camera>();
        Sprite sprite = backGroundSpawn.GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSize = texture.width / sprite.pixelsPerUnit;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            levelsPanelOpen = !levelsPanelOpen;
            LevelsPanel.SetActive(levelsPanelOpen);
        }
        if (camera.orthographicSize > 13.7) backGround.transform.localScale = new Vector2(backGround.transform.localScale.x, camera.orthographicSize / 13.7f);
    }

    public void CameraChangevalue()
    {
        CameraValueState =! CameraValueState;
        if(CameraValueState == true) camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 20f, 2f);
        else camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 9f, 1f);
    }

    private void FixedUpdate()
    {
        float size = camera.orthographicSize;
        loadingScreen.transform.localScale = new Vector2(size / 2.71f, size / 1.71f);
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = camera.transform.position - lastCamposition;
        backGround.transform.position += new Vector3(deltaMovement.x * parallaxEffect.x, deltaMovement.y * parallaxEffect.y);
        lastCamposition = camera.transform.position;

        if (Mathf.Abs(camera.transform.position.x - backGround.transform.position.x) >= textureUnitSize)
        {
            float offsetPos = (camera.transform.position.x - backGround.transform.position.x) % textureUnitSize;
            backGround.transform.position = new Vector3(camera.transform.position.x + offsetPos, backGround.transform.position.y);
        }
    }

    public void LoadScreen() { loadingScreen.GetComponent<Animator>().Play("loadscreen"); }
}
