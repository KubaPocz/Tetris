using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TetrisController : MonoBehaviour
{
    private InputActionSystem controls;
    private GameObject grid;
    private GridManager gridManager; // Każdy klocek ma przypisany swój GridManager
    public string gridTag;

    public int playerID; // 1 = Gracz 1, 2 = Gracz 2
    public float gridSize = 1f; // Wielkość kratki (ruch poziomy)

    private Rigidbody2D rb;
    private bool canMove = true;

    private void Awake()
    {
        controls = new InputActionSystem();
        rb = GetComponent<Rigidbody2D>();

        grid = GameObject.FindGameObjectWithTag(gridTag);
        gridManager = grid.GetComponent<GridManager>();
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();

        if (playerID == 1)
        {
            controls.Gameplay.Player1.performed += ctx => Move(ctx);
        }
        else if (playerID == 2)
        {
            controls.Gameplay.Player2.performed += ctx => Move(ctx);
        }
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    private void Move(InputAction.CallbackContext context)
    {
        if (!canMove) return;

        float direction = context.ReadValue<float>();
        Vector3 targetPosition = transform.position + new Vector3(direction * gridSize, 0, 0);

        // Każdy klocek sprawdza ruch w swoim GridManager
        if (gridManager.CanMoveTo(targetPosition))
        {
            transform.position = targetPosition;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Block"))
        {
            List<Transform> quadsToDetach = new List<Transform>(); // Tymczasowa lista Quads

            // Dodajemy wszystkie `Quad`-y do listy przed zmianą rodzica
            foreach (Transform child in transform)
            {
                quadsToDetach.Add(child);
            }

            // Teraz oddzielamy `Quad`-y od rodzica
            foreach (Transform quad in quadsToDetach)
            {
                quad.SetParent(gridManager.transform); // Przypisanie do GridManager

                gridManager.AddToGrid(quad.gameObject,(int)quad.transform.localPosition.x, (int)quad.transform.localPosition.y);

                // Jeśli `Quad` nie ma `Rigidbody2D`, dodajemy go dynamicznie
                Rigidbody2D rb = quad.GetComponent<Rigidbody2D>();
                if (rb == null)
                {
                    rb = quad.gameObject.AddComponent<Rigidbody2D>();
                }
                rb.bodyType = RigidbodyType2D.Static; // Blok nie powinien się już ruszać
            }

            Destroy(gameObject); // Dopiero teraz usuwamy pustego rodzica
        }
    }
}