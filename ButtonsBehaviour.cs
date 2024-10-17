using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsBehaviour : MonoBehaviour
{
    [SerializeField] private AudioClip cancel;
    [SerializeField] private AudioClip click;
    [SerializeField] private GameObject locker;
    public GameObject curLocation;
    public int startsToLoad;

    private void Start()
    {
        Vector2 Localscale = transform.localScale;
        float multiplier = Screen.width / (float)1225;
        transform.localScale = new Vector2(Localscale.x *= multiplier, Localscale.y *= multiplier);
        TextUpdate();
    }

    public void LoadLocation()
    {
        if(Camera.main.GetComponent<ButtonsDelay>().waiting == false)
        {
            Camera.main.GetComponent<ButtonsDelay>().waiting = true;
            try
            {
                if (GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerData>().allStars >= startsToLoad && (bool)PhotonNetwork.CurrentRoom.CustomProperties["CanStart"] == true)
                {
                    GetComponent<AudioSource>().PlayOneShot(click);
                    try { GameObject.FindGameObjectWithTag("Location").GetComponent<DestroyInNetwork>().enabled = true; } catch { Debug.Log("Location Not Founded"); }
                    Camera.main.GetComponent<CameraManager>().LoadingScreen();
                    PhotonNetwork.Instantiate(curLocation.name, new Vector3(-5, 0.2f, -2), Quaternion.identity);
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerController>().Invoke("InvokeRestart", .5f);
                    try
                    {
                        GameObject[] blood = GameObject.FindGameObjectsWithTag("Blood");
                        for (int i = 0; i < blood.Length; i++) blood[i].GetComponent<DestroyInNetwork>().enabled = true;
                    }
                    catch { Debug.Log("Blood wasnt delete"); }
                }
                else GetComponent<AudioSource>().PlayOneShot(cancel);
            }
            catch { Debug.Log("Fatal Error"); FindObjectOfType<DisconnectScript>().DisconnectPlayer(); }
        }
    }

    public void TextUpdate()
    {
        Text btntext = gameObject.transform.GetChild(1).GetComponent<Text>();
        int index = Convert.ToInt32(gameObject.transform.GetChild(0).GetComponent<Text>().text) - 1;
        PlayerData data = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerData>();
        if (data.allStars < startsToLoad) locker.SetActive(true); else locker.SetActive(false);
        btntext.text = "";
        btntext.text += " " + data.star[index];
        btntext.text += $"{Environment.NewLine}({data.times[index]} sec)";
    }
}
