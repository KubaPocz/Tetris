using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int playerID; // Przypisanie do gracza 1 lub 2
    public static int width = 10;
    public static int height = 20;
    public GameObject[,] grid = new GameObject[8, 12];

    public void AddToGrid(GameObject block, int x , int y)
    {
        Debug.Log("x:"+x +"y:"+ y+"go:"+block.name);
        grid[x, y] = block;
        block.transform.localPosition = new Vector3((int)x, (int)y, 0);
    }

    public bool CanMoveTo(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x);
        int y = Mathf.RoundToInt(position.y);

        if (x < 0 || x >= width || y < 0 || y >= height) return false;
        return grid[x, y] == null; // Prawda, jeœli kratka jest wolna
    }
}
