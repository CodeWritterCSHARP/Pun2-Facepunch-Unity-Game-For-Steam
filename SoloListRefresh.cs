using UnityEngine;

public class SoloListRefresh : MonoBehaviour
{
    private void Start() => FindObjectOfType<SoloList>().ListRefresh();
}
