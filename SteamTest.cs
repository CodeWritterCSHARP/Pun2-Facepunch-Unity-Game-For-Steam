using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class SteamTest : MonoBehaviour
{
    [SerializeField] private InputField text;
    void Start()
    {
        if (!SteamManager.Initialized) { Debug.Log("NO"); return; }
        text.text = SteamFriends.GetPersonaName();
    }
}
