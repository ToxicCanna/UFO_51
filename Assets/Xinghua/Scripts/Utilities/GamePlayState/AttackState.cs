using UnityEngine;
using UnityEngine.InputSystem;

public class AttackState : BaseState
{
    private GridIndicator gridIndicator;
    private PlayerControls currentControls;

    private System.Action<InputAction.CallbackContext> submitAction;
    private System.Action<InputAction.CallbackContext> cancelAction;
    public AttackState(GridIndicator gridIndicator)
    {
        this.gridIndicator = gridIndicator;
    }

    public override void EnterState()
    {
        //Debug.Log("enter attack state");
        GameManager.Instance.DisplayInputText("Check if have enemy with range\nKill hero get point, Reach 10 will win");
        gridIndicator.HandleRangeAttack();
    }

    public override void UpdateState()
    {
        //Debug.Log("update attack state");
        InputManager.Instance.SetInputMode(InputManager.InputMode.Gameplay);
    }



    public override void HandleInput(InputManager inputManager)
    {

        //Debug.Log("Handle attack State");
        currentControls = inputManager.GetControls();
        if (currentControls != null && gridIndicator != null)
        {
            //currentControls.GamePlay.Move.performed += ctx => gridIndicator?.ChooseTargets(ctx.ReadValue<Vector2>());
         

            submitAction = ctx => gridIndicator?.MoveHeroToTargetPosition();
            currentControls.GamePlay.Submit.performed += submitAction;

            cancelAction = ctx => gridIndicator?.CancleSelected();
            currentControls.GamePlay.Cancle.performed += cancelAction;

        }
        else
        {
            Debug.Log("gridIndicator is null");
        }
    }
    public override void ExitState()
    {

        if (currentControls != null && gridIndicator != null)
        {
            //currentControls.GamePlay.Move.performed -=ctx => gridIndicator?.ChooseTargets(ctx.ReadValue<Vector2>());
           

            if (cancelAction != null) currentControls.GamePlay.Cancle.performed -= cancelAction;
            cancelAction = null;

            if (submitAction != null) currentControls.GamePlay.Submit.performed -= submitAction;
            submitAction = null;
        }
        else
        {
            Debug.Log("gridIndicator is null");
        }
    }
}
