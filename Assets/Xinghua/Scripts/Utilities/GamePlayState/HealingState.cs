using UnityEngine;


public class HealingState : BaseState
{
    private GridIndicator gridIndicator;
    private PlayerControls currentControls;
    public HealingState(GridIndicator gridIndicator)
    {
        this.gridIndicator = gridIndicator;
    }

    public override void EnterState()
    {
        Debug.Log("enter Heal state");
        gridIndicator.controlHintText.text = "Check if have enemy with range";
        gridIndicator.CheckHealRange();
    }

    public override void UpdateState()
    {
        Debug.Log("update Heal state");
       
    }

    public override void HandleInput(InputManager inputManager)
    {

        Debug.Log("Handle Heal State");
      
    }
    public override void ExitState()
    {
        Debug.Log("Exit Heal State");
      
    }
}
