using UnityEngine;
using UnityEngine.InputSystem;


public class MoveHeroState : BaseState
{
    private GridIndicator gridIndicator;
    private PlayerControls currentControls;
    private System.Action<InputAction.CallbackContext> submitAction;
    private System.Action<InputAction.CallbackContext> moveAction;
    public MoveHeroState(GridIndicator gridIndicator)
    {
        this.gridIndicator = gridIndicator;
    }

    public override void EnterState()
    {
        Debug.Log("Entered move Hero State");
        GameManager.Instance.DisplayInputText("Choose target positon ");
    }

    public override void HandleInput(InputManager inputManager)
    {
        // Debug.Log("Handle GamePlay State");
        var controls = inputManager.GetControls();
      
        ExitState();
        currentControls = controls;
     
        if (controls != null && gridIndicator != null)
        {
            moveAction = ctx => gridIndicator?.HandleIndicatorMove(ctx.ReadValue<Vector2>());
            currentControls.GamePlay.Move.performed += moveAction;

            submitAction = ctx => gridIndicator?.MoveHeroToTargetPosition();
            currentControls.GamePlay.Submit.performed += submitAction;
        }
        else
        {
            Debug.LogWarning("MoveHeroState: gridIndicator or controls is null!");
        }
    }

    public override void ExitState()
    {
        Debug.Log("Exited move Hero State");
        if (currentControls == null)
        {
            currentControls = InputManager.Instance.GetControls(); 
        }
        if (moveAction != null)
        {
            currentControls.GamePlay.Move.performed -= moveAction;
            moveAction = null;
        }

        if (submitAction != null)
        {
            currentControls.GamePlay.Submit.performed -= submitAction;
            submitAction = null;
        }

    }

    public override void UpdateState()
    {
        //Debug.Log("Update Gameplay State");
        InputManager.Instance.SetInputMode(InputManager.InputMode.Gameplay);
    }
}
