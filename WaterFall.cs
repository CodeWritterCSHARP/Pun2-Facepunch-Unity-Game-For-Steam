using UnityEngine;

public class WaterFall : MonoBehaviour
{
    [SerializeField] private float maxLength;
    [SerializeField] private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.material.SetFloat("_Length", maxLength);
    }
}
