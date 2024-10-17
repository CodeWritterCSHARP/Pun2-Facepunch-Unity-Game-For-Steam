using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class TimerOnLVL : MonoBehaviourPunCallbacks, IPunObservable
{
    private float timer;

    public float TimerGetSeter { get => timer; set => timer = value; }
    
    void Update()
    {
        if ((bool)PhotonNetwork.CurrentRoom.CustomProperties["CanStart"] == true) {
            timer += Time.deltaTime;
            GetComponent<Text>().text = timer.ToString("F1");
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if ((bool)PhotonNetwork.CurrentRoom.CustomProperties["CanStart"] == true) {
            if (stream.IsWriting && PhotonNetwork.IsMasterClient) stream.SendNext(timer);
            else if (stream.IsReading) timer = (float)stream.ReceiveNext();
        }
    }
}
