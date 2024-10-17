using Photon.Realtime;
using Photon.Pun;
using UnityEngine;

public class RoomChecking : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
#if UNITY_EDITOR
    void Update() { Debug.Log(PhotonNetwork.PlayerList.Length); }
#endif

    public DisconnectScript disconnectScript;

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        disconnectScript.DisconnectPlayer();
        base.OnPlayerLeftRoom(otherPlayer);
    }
}
