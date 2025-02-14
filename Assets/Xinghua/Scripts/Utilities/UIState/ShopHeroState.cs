using UnityEngine;
using UnityEngine.InputSystem;

public class ShopHeroState : BaseState
{
    private System.Action<InputAction.CallbackContext> navigateAction;
    private System.Action<InputAction.CallbackContext> selectAction;
    private System.Action<InputAction.CallbackContext> cancelAction;
    private GridIndicator gridIndicator;
    public override void EnterState()
    {
        Debug.Log("Enter shop State");
        GameManager.Instance.DisplayInputText("W and D to select hero to buy, Enter key to submit");
        UINavManager.Instance.InitSelectorPositionInHeroShopZone();
        //UINavManager.Instance.UpdateShopButtons();
    }
    public ShopHeroState(GridIndicator gridIndicator)
    {
        this.gridIndicator = gridIndicator;
    }

    public override void HandleInput(InputManager inputManager)
    {
        Debug.Log("Handle shop State");
        var controls = InputManager.Instance.GetControls();

        navigateAction = ctx => UINavManager.Instance.HandleNavigationInShopZone(ctx.ReadValue<Vector2>());
        selectAction = ctx => UINavManager.Instance.HandleHeroShopSelection();
        cancelAction = ctx => UINavManager.Instance.CancleSelected();

        controls.UI.Navigate.performed += navigateAction;
        controls.UI.Selected.performed += selectAction;
        controls.UI.Cancle.performed += cancelAction;

    }

    public override void ExitState()
    {
        var controls = InputManager.Instance.GetControls();
        Debug.Log("Exited shop State");

        if (controls?.UI == null)
        {
            Debug.LogWarning("UI Input controls are null, cannot unbind events.");
            return;
        }

        if (navigateAction != null)
            controls.UI.Navigate.performed -= navigateAction;
        if (selectAction != null)
            controls.UI.Selected.performed -= selectAction;
        if (cancelAction != null)
            controls.UI.Cancle.performed -= cancelAction;


        navigateAction = null;
        selectAction = null;
        cancelAction = null;

    }


    public override void UpdateState()
    {
        //Debug.Log("Update shop state");
        InputManager.Instance.SetInputMode(InputManager.InputMode.UI);
    }
}
