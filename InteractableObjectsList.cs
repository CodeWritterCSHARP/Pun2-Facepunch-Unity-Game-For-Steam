using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjectsList : MonoBehaviourPun
{
    private List<Tuple<GameObject, Vector3, Quaternion>> list = new List<Tuple<GameObject, Vector3, Quaternion>>();
    [SerializeField] private string[] tags = new string[8] { "Siemen", "Gas", "Finish", "FallGround", "Pos", "Bee", "Wheel", "Animated" };
    [SerializeField] private DisconnectScript disconnectScript;

    public void ListRefresh() { /*if (PhotonNetwork.IsMasterClient)*/ GetComponent<PhotonView>().RPC("ListRefreshh", RpcTarget.AllBuffered); }
    public void ListSpawn() { /*if (PhotonNetwork.IsMasterClient)*/ GetComponent<PhotonView>().RPC("ListSpawnn", RpcTarget.AllBuffered); }

    [PunRPC]
    public void ListRefreshh()
    {
        try
        {
            list.Clear();

            for (int i = 0; i < tags.Length; i++)
            {
                GameObject[] gObject = GameObject.FindGameObjectsWithTag(tags[i]);
                if (gObject.Length > 0)
                {
                    for (int j = 0; j < gObject.Length; j++)
                    {
                        switch (tags[i])
                        {
                            //case "Finish":
                            //    list.Add(Tuple.Create(gObject[j], gObject[j].transform.position, gObject[j].transform.rotation));
                            //    foreach (Transform child in gObject[j].transform) child.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
                            //    gObject[j].GetComponent<MovementToPoint>().enabled = false;
                            //    break;

                            case "FallGround":
                                list.Add(Tuple.Create(gObject[j], gObject[j].transform.position, gObject[j].transform.rotation));
                                foreach (Transform child in gObject[j].transform)
                                    list.Add(Tuple.Create(child.gameObject, child.position, child.rotation));
                                break;

                            case "Gas":
                                list.Add(Tuple.Create(gObject[j], gObject[j].transform.position, gObject[j].transform.rotation));
                                gObject[j].GetComponent<Mushroom>().ChangeOff(true);
                                break;

                            default: list.Add(Tuple.Create(gObject[j], gObject[j].transform.position, gObject[j].transform.rotation)); break;
                        }
                    }
                }
            }
        } catch (Exception ex) { Debug.Log("WARNING" + ex); /*disconnectScript.DisconnectPlayer();*/ }
    }

    [PunRPC]
    public void ListSpawnn()
    {
        try
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Item1 != null)
                {
                    list[i].Item1.transform.position = list[i].Item2;
                    list[i].Item1.transform.rotation = list[i].Item3;

                    switch (list[i].Item1.tag)
                    {
                        case "Finish": list[i].Item1.GetComponent<MovementToPoint>().enabled = false; break;

                        case "Wheel":
                            list[i].Item1.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                            list[i].Item1.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
                            break;

                        case "Animated": list[i].Item1.GetComponent<Animator>().Update(0f); list[i].Item1.GetComponent<Animator>().enabled = false; break;

                        case "FallGround": list[i].Item1.GetComponent<BoxCollider2D>().enabled = true; break;

                        case "FallGroundChild": list[i].Item1.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static; break;

                        case "Bee":
                            list[i].Item1.GetComponent<BeeAttack>().enabled = true;
                            list[i].Item1.GetComponent<BeeAttack>().startChasing = false;
                            list[i].Item1.GetComponentInChildren<CapsuleCollider2D>().enabled = true;
                            if (list[i].Item1.GetComponent<Rigidbody2D>() == null) list[i].Item1.AddComponent<Rigidbody2D>();
                            //list[i].Item1.GetComponent<Rigidbody2D>().gravityScale = 0;
                            list[i].Item1.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                            break;

                        case "Gas":
                            list[i].Item1.GetComponent<Mushroom>().ChangeOff(true);
                            break;

                        case "Siemen":
                            list[i].Item1.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                            list[i].Item1.transform.GetChild(0).transform.localPosition = new Vector3(1.742364f, 0.007967987f, 0);
                            list[i].Item1.transform.GetChild(1).transform.localPosition = new Vector3(1.037868f, 0.002805176f, 0);
                            list[i].Item1.transform.GetChild(2).transform.localPosition = new Vector3(0.4880788f, -0.02524536f, 0);
                            list[i].Item1.transform.GetChild(3).transform.localPosition = new Vector3(-0.1122018f, -0.03085541f, 0);
                            list[i].Item1.transform.GetChild(4).transform.localPosition = new Vector3(-0.7124825f, -0.02524536f, 0);
                            list[i].Item1.transform.GetChild(5).transform.localPosition = new Vector3(-1.234222f, -0.01402512f, 0);
                            break;
                    }
                }
            }
        } catch(Exception ex) { Debug.Log("WARNING" + ex); /*disconnectScript.DisconnectPlayer();*/ }
    }
}
