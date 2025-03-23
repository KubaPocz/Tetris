using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Loading;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class GridManager : MonoBehaviour
{
    public int playerId;
    public int playerScore = 0;
    public GameObject[,] grid = new GameObject[8, 12];
    [NonSerialized] public GameObject nextBlock;


    public void AddToGrid(GameObject block, int x , int y)
    {
        if(x<=8 && y <= 10)
        {
            grid[x, y] = block;
        }
        else
        {
            Destroy(GameObject.FindGameObjectWithTag("Spawner"));
            Events.EndGame(playerId);
        }
        block.transform.localPosition = new Vector2(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
        block.tag = "Block";
        playerScore += 20;
        Events.UpdateScore(playerId,playerScore);
    }


    #region ClearingRows
    public void ClearFullRows()
    {
        for (int row = 0; row < grid.GetLength(1); row++)
        {
            if (isRowFull(row))
            {
                for (int col = 0; col < grid.GetLength(0); col++)
                {
                    grid[col, row].GetComponent<Animator>().SetTrigger("Destroy");
                    grid[col, row] = null;
                }
                playerScore += 2000;
            }
        }
        ReassembleGrid();
    }
    public bool isRowFull(int row)
    {
        for (int col=0; col < grid.GetLength(0); col++)
        {
            if(grid[col, row] == null) return false;
        }
        return true;
    }
    #endregion

    #region MovingRowsDown
    public bool IsRowEmpty(int row)
    {
        for (int col = 0; col < grid.GetLength(0); col++)
        {
            if (grid[col, row] != null) return false;
        }
        return true;
    }

    public void ReassembleGrid()
    {
        for (int row = 0; row < grid.GetLength(1); row++)
        {
            if (IsRowEmpty(row))
            {
                bool movedSomething = false;

                for (int y = row + 1; y < grid.GetLength(1); y++)
                {
                    for (int x = 0; x < grid.GetLength(0); x++)
                    {
                        if (grid[x, y] != null)
                        {
                            grid[x, y - 1] = grid[x, y];
                            grid[x, y] = null;

                            StartCoroutine(MoveBlockDown(grid[x, y - 1]));
                            movedSomething = true;
                        }
                    }
                }

                if (movedSomething)
                    row--;
            }
        }
        Events.UpdateScore(playerId, playerScore);
    }
    public IEnumerator MoveBlockDown(GameObject gm)
    {
        yield return new WaitForSeconds(0.1f);
        gm.transform.position += Vector3.down;
    }

    #endregion

}
