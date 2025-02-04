using UnityEngine;


public class MoveHeroState : BaseState
{
    private GridIndicator gridIndicator;
    private PlayerControls currentControls;

    public MoveHeroState(GridIndicator gridIndicator)
    {
        this.gridIndicator = gridIndicator;
    }

    public override void EnterState()
    {
        Debug.Log("Entered move Hero State");
        gridIndicator.controlHintText.text ="W A S D to move;enter to confirm move to target position";
    }

    public override void HandleInput(InputManager inputManager)
    {
        // Debug.Log("Handle GamePlay State");
        var controls = inputManager.GetControls();
        currentControls = controls;
        if (controls != null && gridIndicator != null)
        {
          
            currentControls.GamePlay.Submit.performed += ctx => gridIndicator?.MoveToTargetIndicator();
      
        }
        else
        {
            Debug.Log("gridIndicator is null");
        }
    }

    public override void ExitState()
    {
        Debug.Log("Exited Gameplay State");
      
        currentControls.GamePlay.Submit.performed -= ctx => gridIndicator?.MoveToTargetIndicator();

    }

    public override void UpdateState()
    {
        //Debug.Log("Update Gameplay State");
        InputManager.Instance.SetInputMode(InputManager.InputMode.Gameplay);
    }
}
