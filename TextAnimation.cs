using UnityEngine;
using TMPro;

public class TextAnimation : MonoBehaviour
{
    [SerializeField] private float countAddValue = -0.05f;
    [SerializeField] private float x = 3.3f;
    [SerializeField] private float y = 2.5f;
    private TMP_Text textMesh;
    private Mesh mesh;
    private Vector3[] vertices;
    private float count = 0;

    void Start() => textMesh = GetComponent<TMP_Text>();

    void Update()
    {
        textMesh.ForceMeshUpdate();
        mesh = textMesh.mesh;
        vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 offset = Wobble(Time.time + count);
            count += countAddValue;
            vertices[i] = vertices[i] + offset;
        }
        count = 0;
        mesh.vertices = vertices;
    }

    Vector2 Wobble(float time) { return new Vector2(Mathf.Sin(time * x), Mathf.Cos(time * y)); }
}
