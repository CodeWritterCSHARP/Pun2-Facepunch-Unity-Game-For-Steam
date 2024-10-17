using UnityEngine;

public class ActivateSolo : MonoBehaviour
{
    [SerializeField] private GameObject gameObj;
    [SerializeField] private int type = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GameController"))
        {
            switch (type)
            {
                case 0: Activate(); break;
                case 1: ActivateWheel(); break;
                case 2: ActivateAnimated(); break;
                default: break;
            }
        }
    }

    void ActivateAnimated()
    {
        try { gameObj.GetComponent<Animator>().enabled = true; } catch { }
    }

    void ActivateWheel()
    {
        gameObj.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        gameObj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    void Activate()
    {
        gameObj.SetActive(true);
    }
}
