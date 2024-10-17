using UnityEngine;

public class PathDrawing : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    private Vector2 gizmosPoints;

    private void OnDrawGizmos()
    {
        for (float i = 0; i <= 1; i += 0.05f)
        {
            gizmosPoints = Mathf.Pow(1 - i, 3) * points[0].position + 3 * Mathf.Pow(1 - i, 2) * i * points[1].position +
                3 * (1 - i) * Mathf.Pow(i, 2) * points[2].position + Mathf.Pow(i, 3) * points[3].position;
            Gizmos.DrawSphere(gizmosPoints, 0.25f);
        }

        Gizmos.DrawLine(new Vector2(points[0].position.x, points[0].position.y),
            new Vector2(points[1].position.x, points[1].position.y));

        Gizmos.DrawLine(new Vector2(points[2].position.x, points[2].position.y),
            new Vector2(points[3].position.x, points[3].position.y));
    }
}
