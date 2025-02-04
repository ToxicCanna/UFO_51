using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIState : BaseState
{
    public override void EnterState()
    {
        Debug.Log("Enter UI State");
    }

  
    public override void HandleInput(InputManager inputManager)
    {
        Debug.Log("Handle UI State");
        var controls = InputManager.Instance.GetControls();

        controls.UI.Navigate.performed += ctx => UINavManager.Instance.HandleNavigation(ctx.ReadValue<Vector2>());
        controls.UI.Selected.performed += ctx => UINavManager.Instance.HandleButtonsSelection();
        controls.UI.Cancle.performed += ctx => UINavManager.Instance.CancleSelected();
    }

    public override void ExitState()
    {
        var controls = InputManager.Instance.GetControls();
        Debug.Log("Exited UI State");
        controls.UI.Navigate.performed -= ctx => UINavManager.Instance.HandleNavigation(ctx.ReadValue<Vector2>());
        controls.UI.Selected.performed -= ctx => UINavManager.Instance.HandleButtonsSelection();
        controls.UI.Cancle.performed -= ctx => UINavManager.Instance.CancleSelected();

    }


    public override void UpdateState()
    {
        Debug.Log("Update UI state");
        InputManager.Instance.SetInputMode(InputManager.InputMode.UI);
    }
}
