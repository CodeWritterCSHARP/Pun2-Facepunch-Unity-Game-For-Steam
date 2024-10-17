using System.Collections;
using UnityEngine;
using Pathfinding;

public class MultiplyPaths : MonoBehaviour
{
    [SerializeField] private Transform[] trails;
    private int currentTrail = 0;
    private float trailParametr = 0f;
    private Vector2 playerPos;
    [SerializeField] private float speed = 0.5f;
    private bool courutine = true;

    private void Update()
    {
        if (GetComponent<AIDestinationSetter>() != null) {
            if(GetComponent<AIDestinationSetter>().target == null)
            {
                 if (courutine) StartCoroutine(MoveByTrail(currentTrail));
            }
        }
        else if (courutine) StartCoroutine(MoveByTrail(currentTrail));
    }

    public Vector2 PlayerPosGetter { get => playerPos; }

    private IEnumerator MoveByTrail(int trailNumber)
    {
        courutine = false;
        while(trailParametr < 1)
        {
            trailParametr += Time.deltaTime * speed;

            playerPos = Mathf.Pow(1 - trailParametr, 3) * trails[trailNumber].GetChild(0).position +
                3 * Mathf.Pow(1 - trailParametr, 2) * trailParametr * trails[trailNumber].GetChild(1).position +
                3 * (1 - trailParametr) * Mathf.Pow(trailParametr, 2) * trails[trailNumber].GetChild(2).position +
                Mathf.Pow(trailParametr, 3) * trails[trailNumber].GetChild(3).position;

            if (playerPos.x > transform.position.x) transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
            else transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);

            transform.position = playerPos;
            yield return new WaitForEndOfFrame();
        }
        trailParametr = 0;
        currentTrail++;
        if (currentTrail > trails.Length - 1) currentTrail = 0;
        courutine = true;
    }
}
