using UnityEngine;

public class AttackState : BaseState
{
    private GridIndicator gridIndicator;
    private PlayerControls currentControls;
    public AttackState(GridIndicator gridIndicator)
    {
        this.gridIndicator = gridIndicator;
    }

    public override void EnterState()
    {
        Debug.Log("enter attack state");
        gridIndicator.controlHintText.text = "W  s to choose target; Enter key to submit ";
    }

    public override void UpdateState()
    {
        Debug.Log("update attack state");
        InputManager.Instance.SetInputMode(InputManager.InputMode.Gameplay);
    }



    public override void HandleInput(InputManager inputManager) 
    {
    
         Debug.Log("Handle attack State");
         currentControls = inputManager.GetControls();
        if (currentControls != null && gridIndicator != null)
        {
            currentControls.GamePlay.Move.performed += ctx => gridIndicator?.ChooseTargets(ctx.ReadValue<Vector2>());
            currentControls.GamePlay.Submit.performed += ctx => gridIndicator?.SbumitedTarget();
          
    
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
            currentControls.GamePlay.Move.performed -=ctx => gridIndicator?.ChooseTargets(ctx.ReadValue<Vector2>());
            currentControls.GamePlay.Submit.performed -= ctx => gridIndicator?.SbumitedTarget();
        }
        else
        {
            Debug.Log("gridIndicator is null");
        }
    }
}
