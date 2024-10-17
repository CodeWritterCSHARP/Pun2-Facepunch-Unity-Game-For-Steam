using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Maze : MonoBehaviour
{
    [SerializeField] private MazeCell prefab;
    [SerializeField] private int width;
    [SerializeField] private int heigth;
    private MazeCell[,] mazeGrid;

    private void Start()
    {
        mazeGrid = new MazeCell[width, heigth];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < heigth; y++)
            {
                mazeGrid[x, y] = Instantiate(prefab, new Vector2(x, y), Quaternion.identity);
            }
        }
        mazeGrid[width - 1, 0].ClearWall(0);
        mazeGrid[0, heigth - 1].ClearWall(1);
        GenerateMaze(null, mazeGrid[0, 0]);
    }

    private MazeCell GetNextCell(MazeCell current)
    {
        var unvisited = GetUnvisited(current);
        return unvisited.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private void GenerateMaze(MazeCell previous, MazeCell current)
    {
        current.Visit();
        ClearWalls(previous, current);

        MazeCell nextCell;
        do 
        {
            nextCell = GetNextCell(current);
            if (nextCell != null) GenerateMaze(current, nextCell);
        }
        while (nextCell != null);
    }

    private System.Collections.Generic.IEnumerable<MazeCell> GetUnvisited(MazeCell current)
    {
        int x = (int)current.transform.position.x;
        int y = (int)current.transform.position.y;

        if (x + 1 < width)
        {
            var cellRight = mazeGrid[x + 1, y];
            if (!cellRight.isVisited) yield return cellRight;
        }

        if (x - 1 >= 0)
        {
            var cellLeft = mazeGrid[x - 1, y];
            if (!cellLeft.isVisited) yield return cellLeft;
        }

        if (y + 1 < heigth)
        {
            var cellUp = mazeGrid[x, y + 1];
            if (!cellUp.isVisited) yield return cellUp;
        }

        if (y - 1 >= 0)
        {
            var cellDown = mazeGrid[x, y - 1];
            if (!cellDown.isVisited) yield return cellDown;
        }
    }

    private void ClearWalls(MazeCell previous, MazeCell current)
    {
        if (previous == null) return;

        if (previous.transform.position.x < current.transform.position.x)
        {
            previous.ClearWall(0);
            current.ClearWall(1);
            return;
        }

        if (previous.transform.position.x > current.transform.position.x)
        {
            current.ClearWall(0);
            previous.ClearWall(1);
            return;
        }

        if (previous.transform.position.y < current.transform.position.y)
        {
            previous.ClearWall(2);
            current.ClearWall(3);
            return;
        }

        if (previous.transform.position.y > current.transform.position.y)
        {
            current.ClearWall(2);
            previous.ClearWall(3);
            return;
        }
    }
}
