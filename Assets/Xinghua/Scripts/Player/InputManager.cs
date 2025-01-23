using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerControls controls;
    [SerializeField] GridIndicator indicatorMove;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.GamePlay.Move.performed += ctx => indicatorMove.HandleInput(ctx.ReadValue<Vector2>());
        controls.GamePlay.Confirm.performed += ctx => indicatorMove.ConfirmMovePosition();
    }

    private void OnEnable()
    {
        controls.GamePlay.Enable();
    }

    private void OnDisable()
    {
        controls.GamePlay.Disable();
    }
}
