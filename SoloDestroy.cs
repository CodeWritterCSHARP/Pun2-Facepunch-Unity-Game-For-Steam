using UnityEngine;

public class SoloDestroy : MonoBehaviour
{
    public int type = 0;
    [SerializeField] private float time;

    public float TimeGetSet { get => time; set => time = value; }

    void Start()
    {
        if (type == 0) Destroing();
        if (type == 1) Disable();
    }

    private void Destroing() => Destroy(gameObject, time);

    private void Disable()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        gameObject.GetComponent<Collider2D>().enabled = false;
    }
}
