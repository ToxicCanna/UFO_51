using UnityEngine;

public  class BaseStateMachine : MonoBehaviour
{
    protected BaseState CurrentState;

    public virtual void SetState(BaseState newState)
    {

        CurrentState?.ExitState();

        CurrentState = newState;

        CurrentState.EnterState();
    }

    private void Update()
    {
        Debug.Log("base state update");
        CurrentState?.UpdateState();
        Debug.Log("current state" + CurrentState);
    }
}