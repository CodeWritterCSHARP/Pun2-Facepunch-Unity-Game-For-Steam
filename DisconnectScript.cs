using Photon.Pun;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisconnectScript : MonoBehaviour
{
    private string path = Path.GetFullPath("./") + "\\Networking.exe";

    public void DisconnectPlayer() => StartCoroutine(DisconnectAndLoad());

    private IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected) yield return null;

        if (!Application.isEditor)
        {
            try
            {
                Process process = Process.Start(path);
                Application.Quit();
               // process.Kill();
            }
            catch { SceneManager.LoadScene("Lobby"); }
        }
        else SceneManager.LoadScene("Loading");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) StartCoroutine(DisconnectAndLoad());
    }
}
