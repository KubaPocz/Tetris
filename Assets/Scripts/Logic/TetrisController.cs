using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TetrisController : MonoBehaviour
{
    private GameObject _grid;
    private GridManager _gridManager;
    private Rigidbody2D _rb;
    private BlockSpawner _blockSpawner;

    private float _targetPositionX;
    private bool _isActive = true;

    public string gridTag;
    public float gravity;
    public int playerId;
    public float gridSize = 1f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _targetPositionX = _rb.position.x;
    }

    private void Start()
    {
        // Assign references
        _grid = GameObject.FindGameObjectWithTag(gridTag);
        _gridManager = _grid.GetComponent<GridManager>();
        _blockSpawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<BlockSpawner>();

        // Subscribe to movement input
        if (playerId == 1)
            Events.OnPlayer1MoveInput += Move;
        else if (playerId == 2)
            Events.OnPlayer2MoveInput += Move;
    }

    private void FixedUpdate()
    {
        // Move block towards target position and apply gravity
        _rb.position = new Vector2(
            Mathf.Lerp(_rb.position.x, _targetPositionX, 40f * Time.deltaTime),
            _rb.position.y - (gravity * Time.deltaTime)
        );
    }

    private void OnDisable()
    {
        // Unsubscribe from input events
        if (playerId == 1)
            Events.OnPlayer1MoveInput -= Move;
        else if (playerId == 2)
            Events.OnPlayer2MoveInput -= Move;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the block hits ground or another block
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Block"))
        {
            List<Transform> quadsToDetach = new List<Transform>();

            // Store child blocks to detach
            foreach (Transform child in transform)
            {
                quadsToDetach.Add(child);
            }

            // Detach and add each block to the grid
            foreach (Transform quad in quadsToDetach)
            {
                quad.SetParent(_gridManager.transform);
                quad.GetComponent<BoxCollider2D>().enabled = true;
                _gridManager.AddToGrid(
                    quad.gameObject,
                    Mathf.RoundToInt(quad.transform.localPosition.x),
                    Mathf.RoundToInt(quad.transform.localPosition.y)
                );
            }

            if (_isActive)
            {
                _blockSpawner.SpawnBlock(playerId);
                _isActive = false;
            }

            _gridManager.ClearFullRows();
            Destroy(gameObject);
        }
    }

    private void Move(float direction)
    {
        // Check for wall collision
        if (IsBlockedByWall(direction))
        {
            Debug.Log("Blocked by wall");
            return;
        }

        // Set target position for smooth movement
        _targetPositionX = _rb.position.x + direction;
    }

    private bool IsBlockedByWall(float direction)
    {
        float checkDistance = gridSize * 0.99f;

        // 1. Find the furthest block (child) in movement direction
        Transform furthestChild = null;
        float extremeX = direction > 0 ? float.NegativeInfinity : float.PositiveInfinity;

        foreach (Transform child in transform)
        {
            float childX = child.position.x;
            if ((direction > 0 && childX > extremeX) || (direction < 0 && childX < extremeX))
            {
                extremeX = childX;
                furthestChild = child;
            }
        }

        if (furthestChild == null)
            return false; // safety check

        // 2. Setup raycast direction and origin
        Vector2 rayOrigin = furthestChild.position;
        Vector2 rayDirection = new Vector2(direction, 0);

        Debug.DrawRay(rayOrigin, rayDirection * checkDistance, Color.red, 0.2f);

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, checkDistance, 1 << 6);

        if (hit.collider != null)
        {
            Debug.Log("Raycast hit: " + hit.collider.name);
            return true;
        }

        return false;
    }


    public void Initialize(int playerId)
    {
        this.playerId = playerId;
        gravity = 3f;

        // Assign grid tag based on player
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
