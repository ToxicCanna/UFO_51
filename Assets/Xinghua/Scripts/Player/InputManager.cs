using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerControls controls;
    [SerializeField] GridIndicator indicatorMove;
    [SerializeField] HeroSelect heroSelect;
   

    private void Awake()
    {
        controls = new PlayerControls();

        controls.GamePlay.Move.performed += ctx => indicatorMove.HandleInput(ctx.ReadValue<Vector2>());
        controls.GamePlay.Switch.performed += ctx => heroSelect.SwitchHero();
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
