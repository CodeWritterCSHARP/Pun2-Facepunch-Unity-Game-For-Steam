using System.Collections.Generic;
using UnityEngine;

public class BaloonRope : MonoBehaviour
{
    [SerializeField] private float ropeSegLength;
    [SerializeField] private float elastic = 0.5f;
    [SerializeField] private int segLength;
    [SerializeField] private Transform startPos;
    [SerializeField] private Vector2 force;
    private LineRenderer lineRenderer;
    private List<RopeSegment> ropeSegment = new List<RopeSegment>();

    private void Awake() => lineRenderer = GetComponent<LineRenderer>();

    private void Start()
    {
        Vector2 startPosV = startPos.position;
        for (int i = 0; i < segLength; i++)
        {
            ropeSegment.Add(new RopeSegment(startPosV));
            startPosV.y -= ropeSegLength;
        }
    }

    private void Update() => DrawRope();

    private void FixedUpdate() => Simulate();

    private void Simulate()
    {
        for (int i = 0; i < segLength; i++)
        {
            RopeSegment firstSegment = ropeSegment[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            firstSegment.posNow += force * Time.deltaTime;
            ropeSegment[i] = firstSegment;
        }

        for (int i = 0; i < 50; i++) ApplyConstraints();
    }

    private void ApplyConstraints()
    {
        Vector2 startPosV = startPos.position;
        RopeSegment[] firstSegments = { ropeSegment[0], ropeSegment[1], ropeSegment[2] };
        for (int i = 0; i < firstSegments.Length; i++)
        {
            firstSegments[i].posNow = startPosV;
            startPosV.y -= segLength;
            ropeSegment[i] = firstSegments[i];
        }

        for (int i = 0; i < segLength - 1; i++)
        {
            RopeSegment first = ropeSegment[i];
            RopeSegment second = ropeSegment[i + 1];

            float dist = (first.posNow - second.posNow).magnitude;
            float error = Mathf.Abs(dist - ropeSegLength);

            Vector2 changeDir = Vector2.zero;
            if (dist > ropeSegLength) changeDir = (first.posNow - second.posNow).normalized;
            else if (dist < ropeSegLength) changeDir = (second.posNow - first.posNow).normalized;

            Vector2 changeAmount = changeDir * error;
            if (i > 2)
            {
                first.posNow -= changeAmount * elastic;
                ropeSegment[i] = first;
                second.posNow += changeAmount * elastic;
                ropeSegment[i + 1] = second;
            }
            else
            {
                second.posNow += changeAmount;
                ropeSegment[i + 1] = second;
            }
        }
    }

    private void DrawRope()
    {
        Vector3[] ropePosition = new Vector3[segLength];
        for (int i = 0; i < segLength; i++) ropePosition[i] = ropeSegment[i].posNow;
        lineRenderer.positionCount = ropePosition.Length;
        lineRenderer.SetPositions(ropePosition);
    }

    public struct RopeSegment
    {
        public Vector2 posNow;
        public Vector2 posOld;
        public RopeSegment(Vector2 pos)
        {
            posNow = pos;
            posOld = pos;
        }
    }
}
