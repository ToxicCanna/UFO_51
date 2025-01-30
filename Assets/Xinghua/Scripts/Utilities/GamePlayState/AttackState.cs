using UnityEngine;

public class AttackState : BaseState
{
    public override void EnterState()
    {

    }

    public override void UpdateState()
    {
        Debug.Log("update attack state");
    }

    public override void ExitState()
    {

    }

    public override void HandleInput(InputManager inputManager) { }
}