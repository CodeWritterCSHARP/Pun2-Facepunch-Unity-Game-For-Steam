using System.Collections;
using UnityEngine;
using Photon.Pun;

public class CameraManager : MonoBehaviourPunCallbacks
{
    private float playerpos1;
    private float playerpos2;
    private float playerpos1Y;
    private float playerpos2Y;

    private float olddist;
    private float olddistY = 0;

    private Camera camera;

    [SerializeField] private GameObject backGround;
    [SerializeField] private GameObject backGroundSpawn;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Vector2 parallaxEffect;
    private Stack stack = new Stack();
    private Vector3 lastCamposition;
    private float textureUnitSize;
    private float backGroundPosition = 54.15f;
    private float cameraSize = 0f;

    private PhotonView view;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        camera = GetComponent<Camera>();
        lastCamposition = camera.transform.position;
        Sprite sprite = backGroundSpawn.GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSize = texture.width / sprite.pixelsPerUnit;
    }

    void FixedUpdate()
    {
        Debug.Log(PhotonNetwork.SendRate); Debug.Log(PhotonNetwork.SerializationRate);
        #region LoadingScreen
        float size = camera.orthographicSize;
        loadingScreen.transform.localScale = new Vector2(size / 2.71f, size / 1.71f);
        #endregion

            #region BackGroundUpdate
        if (size > 13.7) backGround.transform.localScale = new Vector2(backGround.transform.localScale.x, size / 13.7f);

        if (size > cameraSize + 13.7)
        {
            cameraSize += 13.7f;
            AddInStack(true);
            AddInStack(false);
            backGroundPosition += 54.15f;
        }

        if (size <= cameraSize - 13.7 || size < 13.7)
        {
            cameraSize -= 13.7f;
            backGroundPosition -= 54.15f;
            Destroy((GameObject)stack.Pop());
            Destroy((GameObject)stack.Pop());
        }
        #endregion

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            transform.position = new Vector3(GameObject.FindGameObjectWithTag("GameController").transform.position.x, GameObject.FindGameObjectWithTag("GameController").transform.position.y, transform.position.z);
            camera.orthographicSize = 9f;
        }

    }

    private void Update() => CameraPosChange();

    #region BackGroundParallax
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
    #endregion

    [PunRPC]
    void CameraPosChange()
    {
        try {
            playerpos1 = (float)PhotonNetwork.CurrentRoom.CustomProperties["PlayerPos1"];
            playerpos2 = (float)PhotonNetwork.CurrentRoom.CustomProperties["PlayerPos2"];
            playerpos1Y = (float)PhotonNetwork.CurrentRoom.CustomProperties["PlayerPos1Y"];
            playerpos2Y = (float)PhotonNetwork.CurrentRoom.CustomProperties["PlayerPos2Y"];
        }
        catch { return; }

        if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            float middleposX = playerpos1 + (playerpos2 - playerpos1) / 2;
            float middleposY = playerpos1Y + (playerpos2Y - playerpos1Y) / 2;

            float distanceY = 0;
            if ((playerpos1Y > 0 && playerpos2Y > 0) || (playerpos1Y < 0 && playerpos2Y < 0)) distanceY = Mathf.Abs(playerpos1Y - playerpos2Y);
            if ((playerpos1Y <= 0 && playerpos2Y >= 0) || (playerpos2Y <= 0 && playerpos1Y >= 0)) distanceY = Mathf.Abs(playerpos1Y) + Mathf.Abs(playerpos2Y);
            float distanceX = Vector2.Distance(new Vector2(playerpos1, transform.position.y), new Vector2(playerpos2, transform.position.y));


            if (distanceY > 12 && distanceX <= distanceY)
            {
                camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, distanceY / 1.7f, 0.2f);
                if (Mathf.Abs(distanceY - olddistY) > 3) olddistY = distanceY;
            }
            else olddistY = 0;

            if (distanceX > 16 && olddistY == 0)
            {
                camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, distanceX / 2, 0.2f);
                if (Mathf.Abs(distanceX - olddist) > 4) olddist = distanceX;
            }
            else olddist = 0;

            if (distanceX <= 16 && distanceY <= 12) camera.orthographicSize = 10;
            if (distanceY < 6) transform.position = Vector3.Lerp(transform.position, new Vector3(middleposX, transform.position.y, -1), .25f);
            else transform.position = Vector3.Lerp(transform.position, new Vector3(middleposX, middleposY, -1), .25f);
        }
    }

    #region LoadingScreen
    public void LoadingScreen(){ view.RPC("loadScreen", RpcTarget.AllBuffered); }
    [PunRPC]
    void loadScreen(){ loadingScreen.GetComponent<Animator>().Play("loadscreen");}
    #endregion

    private void AddInStack(bool direction)
    {
        GameObject curBackGround = Instantiate(backGroundSpawn, Vector3.zero, Quaternion.identity);
        curBackGround.transform.SetParent(backGround.transform);
        curBackGround.transform.localScale = backGround.transform.GetChild(0).transform.localScale;
        if (direction) curBackGround.transform.position = new Vector2(backGround.transform.GetChild(0).transform.position.x + backGroundPosition, backGround.transform.GetChild(0).transform.position.y);
        else curBackGround.transform.position = new Vector2(backGround.transform.GetChild(0).transform.position.x - backGroundPosition, backGround.transform.GetChild(0).transform.position.y);
        stack.Push(curBackGround);
    }
}
