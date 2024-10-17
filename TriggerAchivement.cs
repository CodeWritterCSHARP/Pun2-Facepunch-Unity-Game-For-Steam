using UnityEngine;

public class TriggerAchivement : MonoBehaviour
{
    [SerializeField] private GetAchivement achivement;
    [SerializeField] private string TriggerName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TriggerName)) achivement.enabled = true;
    }
}
