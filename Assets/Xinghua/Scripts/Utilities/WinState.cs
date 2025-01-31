using UnityEngine;

public class WinState : BaseState
{
    private GridIndicator gridIndicator;
    public WinState(GridIndicator gridIndicator)
    {
        this.gridIndicator = gridIndicator;
    }

    public override void EnterState()
    {
        throw new System.NotImplementedException();
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        Debug.Log("Player"+ gridIndicator.currentTurn +"win");
    }


}
