using Photon.Pun;
using UnityEngine;

public class LegPunch : MonoBehaviourPun
{
    [SerializeField] private float radius;
    private PhotonView view;

    private void Awake() => view = GetComponent<PhotonView>();

    public void CheckPunch() { ColliderCheck(); }

    private void ColliderCheck()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        int count = 0; GameObject gb = null;
        foreach (var col in colliders)
        {
            if (col.gameObject.GetComponent<PlayerController>() != null)
            {
                if (count >= 1)
                {
                    GetComponent<GetAchivement>().enabled = true;
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        Vector2 dir = Vector2.zero;
                        dir = col.transform.position - transform.position;
                        if (col.gameObject == this.gameObject.transform.parent) gameObject.transform.parent.GetComponent<PlayerController>().Punch(dir, true);
                        else gb.GetComponent<PlayerController>().Punch(dir, false);
                        break;
                    }
                    else
                    {
                        Vector2 dir = Vector2.zero;
                        dir = col.transform.position - transform.position;
                        if (col.gameObject != this.gameObject.transform.parent) gb.GetComponent<PlayerController>().Punch(dir, false);
                        else gameObject.transform.parent.GetComponent<PlayerController>().Punch(dir, true);
                        break;
                    }
                }
                else gb = col.gameObject; 
                count++;
            }
        }
    }
}
