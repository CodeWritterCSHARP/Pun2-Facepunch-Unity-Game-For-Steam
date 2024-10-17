using UnityEngine;

public class LVLLoader : MonoBehaviour
{
    public GameObject curLocation;
    [SerializeField] private AudioClip click;

    private void Start()
    {
        Vector2 Localscale = transform.localScale;
        float multiplier = Screen.width / (float)1225;
        transform.localScale = new Vector2(Localscale.x *= multiplier, Localscale.y *= multiplier);
    }

    public void LoadLocation()
    {
        if (Camera.main.GetComponent<ButtonsDelay>().waiting == false)
        {
            Camera.main.GetComponent<ButtonsDelay>().waiting = true;
            GetComponent<AudioSource>().PlayOneShot(click);
            Destroy(GameObject.FindGameObjectWithTag("Location"));
            Camera.main.GetComponent<CameraSoloParallax>().LoadScreen();
            Instantiate(curLocation, new Vector3(-5, 0.2f, -2), Quaternion.identity);

            GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerSoloController>().Invoke("InvokeRestart", .5f);
            try
            {
                GameObject[] blood = GameObject.FindGameObjectsWithTag("Blood");
                for (int i = 0; i < blood.Length; i++) blood[i].GetComponent<SoloDestroy>().enabled = true;
            }
            catch { Debug.Log("Blood wasnt delete"); }
        }
    }
}
