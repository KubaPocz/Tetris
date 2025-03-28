using UnityEngine;

public class InputReader : MonoBehaviour
{
    private InputActionSystem _controls;

    void Awake()
    {
        // Create a new input control instance
        _controls = new InputActionSystem();
    }

    private void OnEnable()
    {
        // Enable gameplay controls
        _controls.Gameplay.Enable();

        // Handle Player 1 input
        _controls.Gameplay.Player1.performed += ctx =>
        {
            float dir = ctx.ReadValue<float>();
            Events.SendMoveInput(1, dir);
        };

        // Handle Player 2 input
        _controls.Gameplay.Player2.performed += ctx =>
        {
            float dir = ctx.ReadValue<float>();
            Events.SendMoveInput(2, dir);
        };
    }

    private void OnDisable()
    {
        // Disable gameplay controls
        _controls.Gameplay.Disable();
    }
}
