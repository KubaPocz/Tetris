using UnityEngine;

public class InputReader : MonoBehaviour
{
    private InputActionSystem controls;

    void Awake()
    {
        controls = new InputActionSystem();
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();

        controls.Gameplay.Player1.performed += ctx =>
        {
            float dir = ctx.ReadValue<float>();
            Events.SendMoveInput(1, dir);
        };

        controls.Gameplay.Player2.performed += ctx =>
        {
            float dir = ctx.ReadValue<float>();
            Events.SendMoveInput(2, dir);
        };
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

}
