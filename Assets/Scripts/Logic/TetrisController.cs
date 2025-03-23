using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TetrisController : MonoBehaviour
{
    private GameObject grid;
    private GridManager gridManager;
    public string gridTag;
    private float targetPositionX;
    public float gravity;

    public int playerId;
    public float gridSize = 1f;

    private Rigidbody2D rb;
    private bool isActive = true;
    private BlockSpawner blockSpawner;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPositionX = rb.position.x;
    }
    private void Start()
    {
        grid = GameObject.FindGameObjectWithTag(gridTag);
        gridManager = grid.GetComponent<GridManager>();
        blockSpawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<BlockSpawner>();

        if (playerId == 1)
            Events.OnPlayer1MoveInput += Move;
        else if (playerId == 2)
            Events.OnPlayer2MoveInput += Move;
    }
    private void FixedUpdate()
    {
        rb.position = new Vector2(Mathf.Lerp(rb.position.x, targetPositionX, 40f*Time.deltaTime), rb.position.y-(gravity * Time.deltaTime));
    }


    private void OnDisable()
    {
        if (playerId == 1)
            Events.OnPlayer1MoveInput -= Move;
        else if (playerId == 2)
            Events.OnPlayer2MoveInput -= Move;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Block"))
        {
            List<Transform> quadsToDetach = new List<Transform>(); // Tymczasowa lista Quads

            foreach (Transform child in transform)
            {
                quadsToDetach.Add(child);
            }

            foreach (Transform quad in quadsToDetach)
            {
                quad.SetParent(gridManager.transform);
                quad.GetComponent<BoxCollider2D>().enabled = true;
                gridManager.AddToGrid(quad.gameObject, Mathf.RoundToInt(quad.transform.localPosition.x), Mathf.RoundToInt(quad.transform.localPosition.y));
            }
            if (isActive)
            {
                blockSpawner.SpawnBlock(playerId);
                isActive = false;
            }
            gridManager.ClearFullRows();
            Destroy(gameObject);
        }
    }
    private void Move(float direction)
    {
        if (IsBlockedByWall(direction))
        {
            Debug.Log("Ruch zablokowany przez ścianę!");
            return;
        }
        targetPositionX = rb.position.x + direction;
    }


    
    private bool IsBlockedByWall(float direction)
    {
        float checkDistance = gridSize*0.99f ;
        Vector2 checkDirection = new Vector2(direction, 0);
        Vector2 rayOrigin = (Vector2)rb.position + new Vector2(direction * (gridSize / 2f), 0);

        Debug.DrawRay(rayOrigin, checkDirection * checkDistance, Color.red, 0.2f);

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, checkDirection, checkDistance, 1 << 6);

        if (hit.collider != null)
        {
            Debug.Log("Raycast trafił w: " + hit.collider.name);
            return true; // Trafił w ścianę (bo patrzymy tylko na warstwę Wall)
        }
        if (hit.collider != null)
        {
            Debug.Log("Raycast trafił w: " + hit.collider.name);
            return true; // Trafił w ścianę (bo patrzymy tylko na warstwę Wall)
        }

        return false;
    }

    public void Initialize(int playerId)
    {
        this.playerId = playerId;
        gravity = 3f;

        switch (playerId)
        {
            case 1:
                gridTag = "Grid1";
                break;
            case 2:
                gridTag = "Grid2";
                break;
        }

        enabled = true;
    }
}