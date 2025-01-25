using Unity.VisualScripting;

public abstract class BaseState
{
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public virtual void HandleInput(InputManager inputManager) { }
}
