using UnityEngine;

public class TMTReplace : MonoBehaviour
{
    [SerializeField] private GameObject newText;
    void Start()
    {
        int type = Languages.instance.type;
        if (type == 5 || type == 7 || type == 8 || type == 10 || type == 11 || type == 12)
        {
            gameObject.SetActive(false); newText.SetActive(true);
        }
    }
}
