using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField Join;
    [SerializeField] private InputField Create;
    [SerializeField] private AudioClip sound;

    private List<string> rooms = new List<string>();

    public void GenerateRoomName()
    {
        int k = UnityEngine.Random.Range(7, 10);
        Create.text = Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString())).Remove(k);
        if (rooms.Contains(Create.text)) GenerateRoomName();
        else GetComponent<AudioSource>().PlayOneShot(sound);
    }

    public void CreateRoom()
    {
        GetComponent<AudioSource>().PlayOneShot(sound);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        Hashtable RoomCustomProperties = new Hashtable();

        RoomCustomProperties.Add("PlayerPos1", 0f);
        RoomCustomProperties.Add("PlayerPos2", 0f);
        RoomCustomProperties.Add("PlayerPos1Y", 0f);
        RoomCustomProperties.Add("PlayerPos2Y", 0f);

        RoomCustomProperties.Add("Position", 0);

        RoomCustomProperties.Add("CanStart", false);

        roomOptions.CustomRoomProperties = RoomCustomProperties;

        PhotonNetwork.CreateRoom(Create.text, roomOptions);
    }
    public void JoinRoom() { GetComponent<AudioSource>().PlayOneShot(sound); PhotonNetwork.JoinRoom(Join.text); }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("SampleScene");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Create.text = "already exists";
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
            {
                int index = rooms.FindIndex(x => x == info.Name);
                if (index != -1) rooms.RemoveAt(index);
            }
            else rooms.Add(info.Name);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (!string.IsNullOrEmpty(Join.text) && string.IsNullOrEmpty(Create.text)) JoinRoom();
            if (!string.IsNullOrEmpty(Create.text) && string.IsNullOrEmpty(Join.text)) CreateRoom();
        }
    }
}
