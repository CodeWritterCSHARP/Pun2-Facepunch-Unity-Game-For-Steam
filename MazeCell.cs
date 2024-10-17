using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    public List<GameObject> walls;

    public void ClearWall(int index) => walls[index].SetActive(false);

    public bool isVisited { get; set; }

    public void Visit() { isVisited = true; }
}
