using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpawner : MonoBehaviour
{
    [SerializeField] private int minWind = 6;
    [SerializeField] private int maxWind = 8;
    [SerializeField] private float offset;
    [SerializeField] private GameObject wind;
    private BoxCollider2D boxcollider;
    private Camera camera;


    private void Awake() => boxcollider = GetComponent<BoxCollider2D>();

    private void Start()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        InvokeRepeating("Spawn", 2f, 5f);
    }

    private void FixedUpdate()
    { 
        Vector2 bound = camera.ViewportToWorldPoint(new Vector2(1, 0.5f)); 
        transform.position = bound;
        transform.localScale = new Vector3(camera.orthographicSize, camera.orthographicSize * 2, 1);
    }

    private void Spawn()
    {
        int minWindNew = minWind;
        if (maxWind < camera.orthographicSize / 2) minWindNew += (int)Mathf.Round(camera.orthographicSize / 2) - maxWind;
        int number = Random.Range(minWindNew, maxWind + 1);
        float yPos = boxcollider.bounds.min.y;
        float currentOffset = 0.2f;
        offset = camera.orthographicSize / number / 2f;
        for (int i = 0; i < number; i++)
        {
            var xPos = Random.Range(boxcollider.bounds.min.x, boxcollider.bounds.max.x);
            if (i != number - 1) yPos = Random.Range(yPos + currentOffset, yPos + currentOffset + offset);
            else yPos = Random.Range(boxcollider.bounds.max.y - offset - 0.2f, boxcollider.bounds.max.y);
            Vector2 spawnPos = new Vector2(xPos, yPos);
            Instantiate(wind, spawnPos, Quaternion.identity);
            currentOffset += offset;
        }
    }
}
