using UnityEngine;

public class GamePlayState : BaseState
{
    private GridIndicator gridIndicator;

    public GamePlayState(GridIndicator gridIndicator)
    {
        this.gridIndicator = gridIndicator;
    }

    public override void EnterState()
    {
        Debug.Log("Entered Gameplay State");
    }

    public override void HandleInput(InputManager inputManager)
    {
        Debug.Log("Handle GamePlay State");
        var controls = inputManager.GetControls();
        if (controls != null && gridIndicator != null)
        {
            controls.GamePlay.Move.performed += ctx => gridIndicator?.HandleIndicatorMove(ctx.ReadValue<Vector2>());
            controls.GamePlay.Switch.performed += ctx => gridIndicator?.HandleSelectHero();
            controls.GamePlay.Confirm.performed += ctx => gridIndicator?.MoveToTargetIndicator();
        }
        else
        {
            Debug.Log("gridIndicator is null");
        }
    }

    public override void ExitState()
    {
        Debug.Log("Exited Gameplay State");
    }

    public override void UpdateState()
    {
        Debug.Log("Update Gameplay State");
        InputManager.Instance.SetInputMode(InputManager.InputMode.Gameplay);
    }
}
