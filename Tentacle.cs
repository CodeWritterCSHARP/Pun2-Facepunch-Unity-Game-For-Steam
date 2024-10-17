using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    [SerializeField] private int length;
    [SerializeField] private Vector3[] segments;
    [SerializeField] private Transform targetDir;
    [SerializeField] private Transform wiggleDir;

    [SerializeField] private float targetDist;
    [SerializeField] private float smoothSpeed;
    [SerializeField] private float wiggleMagnitude;
    [SerializeField] private float wiggleSpeed;

    [SerializeField] private float animationTime;
    private float constTime;

    private LineRenderer lineRenderer;
    private Vector3[] segmentsRef;

    private void Awake() => lineRenderer = GetComponent<LineRenderer>();

    private void Start()
    {
        constTime = animationTime;
        lineRenderer.positionCount = length;
        segments = new Vector3[length];
        segmentsRef = new Vector3[length];
        ResetPos();
    }

    void Update()
    {
        wiggleDir.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * wiggleSpeed) * wiggleMagnitude);
        segments[0] = targetDir.position;

        List<Vector2> edges = new List<Vector2>();

        for (int i = 1; i < segments.Length; i++)
        {
            Vector3 targetPos = segments[i - 1] + (segments[i] - segments[i - 1]).normalized * targetDist;
            segments[i] = Vector3.SmoothDamp(segments[i], targetPos, ref segmentsRef[i], smoothSpeed);
        }
        lineRenderer.SetPositions(segments);

        animationTime -= Time.deltaTime;
        if (animationTime <= 0) { animationTime = constTime; ResetPos(); }
    }

    private void ResetPos()
    {
        segments[0] = targetDir.position;
        for (int i = 1; i < length; i++) segments[i] = segments[i - 1] + targetDir.right * targetDist;
        lineRenderer.SetPositions(segments);
    }
}
