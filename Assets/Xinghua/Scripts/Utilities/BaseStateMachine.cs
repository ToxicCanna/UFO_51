using UnityEngine;

public  class BaseStateMachine : MonoBehaviour
{
    protected BaseState CurrentState;

    public virtual void SetState(BaseState newState)
    {

        CurrentState?.ExitState();

        CurrentState = newState;

        CurrentState.EnterState();
        CurrentState.HandleInput(InputManager.Instance);
    }

    private void Update()
    {
       // Debug.Log("base state update");
        CurrentState?.UpdateState();
    }
}