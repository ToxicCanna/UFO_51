using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIState : BaseState
{
    private System.Action<InputAction.CallbackContext> navigateAction;
    private System.Action<InputAction.CallbackContext> selectAction;
    private System.Action<InputAction.CallbackContext> cancelAction;
    private GridIndicator gridIndicator;
    public override void EnterState()
    {
        Debug.Log("Enter UIAction State");
       GameManager.Instance.DisplayInputText("Choose Action\n\nKill hero get point, Reach 10 will win\n\nmove hero in same spot will auto attack");
        UINavManager.Instance.InitSelectorPositionInHeroActionsZone();
    }
    public UIState(GridIndicator gridIndicator)
    {
        this.gridIndicator = gridIndicator;
    }

    public override void HandleInput(InputManager inputManager)
    {
        ExitState();
        Debug.Log("Handle UIAction State");
        var controls = InputManager.Instance.GetControls();

        controls.UI.Navigate.performed += ctx => UINavManager.Instance.HandleNavigationInActionsZone(ctx.ReadValue<Vector2>());
        controls.UI.Selected.performed += ctx => UINavManager.Instance.HandleActionsSelection();
        controls.UI.Cancle.performed += ctx => UINavManager.Instance.CancleSelected();

    }

    public override void ExitState()
    {
        var controls = InputManager.Instance.GetControls();
        Debug.Log("Exited UIAction State");

        if (navigateAction != null)
            controls.UI.Navigate.performed -= navigateAction;
        if (selectAction != null)
            controls.UI.Selected.performed -= selectAction;
        if (cancelAction != null)
            controls.UI.Cancle.performed -= cancelAction;
    }


    public override void UpdateState()
    {
        InputManager.Instance.SetInputMode(InputManager.InputMode.UI);
    }
}
