using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloList : MonoBehaviour
{
    private List<Tuple<GameObject, Vector3, Quaternion>> list = new List<Tuple<GameObject, Vector3, Quaternion>>();
    [SerializeField] private string[] tags = new string[8] { "Siemen", "Gas", "Finish", "FallGround", "Pos", "Bee", "Wheel", "Animated" };

    public void ListRefresh()
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
                            case "Finish":
                                list.Add(Tuple.Create(gObject[j], gObject[j].transform.position, gObject[j].transform.rotation));
                                gObject[j].SetActive(false);
                                break;

                            case "FallGround":
                                list.Add(Tuple.Create(gObject[j], gObject[j].transform.position, gObject[j].transform.rotation));
                                foreach (Transform child in gObject[j].transform)
                                    list.Add(Tuple.Create(child.gameObject, child.position, child.rotation));
                                break;

                            case "Gas":
                                list.Add(Tuple.Create(gObject[j], gObject[j].transform.position, gObject[j].transform.rotation));
                                gObject[j].GetComponent<SoloMushroom>().ChangeOff(true);
                                break;

                            default: list.Add(Tuple.Create(gObject[j], gObject[j].transform.position, gObject[j].transform.rotation)); break;
                        }
                    }
                }
            }
        }
        catch { }
    }

    public void ListSpawn()
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
                        case "Finish": list[i].Item1.SetActive(false); break;

                        case "Wheel":
                            list[i].Item1.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                            list[i].Item1.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
                            break;

                        case "Animated": list[i].Item1.GetComponent<Animator>().Update(0f); list[i].Item1.GetComponent<Animator>().enabled = false; break;

                        case "FallGround": list[i].Item1.GetComponent<BoxCollider2D>().enabled = true; break;

                        case "FallGroundChild": list[i].Item1.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static; break;

                        case "Bee":
                            list[i].Item1.GetComponent<BeeAttackSolo>().enabled = true;
                            list[i].Item1.GetComponentInChildren<CapsuleCollider2D>().enabled = true;
                            list[i].Item1.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                            break;

                        case "Gas":
                            list[i].Item1.GetComponent<SoloMushroom>().ChangeOff(true);
                            break;

                        case "Siemen":
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
        }
        catch { }
    }
}
