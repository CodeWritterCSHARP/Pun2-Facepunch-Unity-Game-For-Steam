using UnityEngine;

public class Languages : MonoBehaviour
{
    public static Languages instance { get; private set; }
    public int type { get; set; }

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(this);
        else instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
