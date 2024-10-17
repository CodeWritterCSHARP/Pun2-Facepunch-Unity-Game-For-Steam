using UnityEngine.SceneManagement;
using UnityEngine;

public class SoloDisconnect : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene("Loading");
    }
}
