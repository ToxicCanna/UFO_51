using UnityEngine;
using UnityEngine.InputSystem;

public class GamePlayState : BaseState
{
    private GridIndicator gridIndicator;
    private PlayerControls currentControls;
    private System.Action<InputAction.CallbackContext> moveAction;
    private System.Action<InputAction.CallbackContext> submitAction;
    private System.Action<InputAction.CallbackContext> shopAction;
    private System.Action<InputAction.CallbackContext> cancelAction;
    public GamePlayState(GridIndicator gridIndicator)
    {
        this.gridIndicator = gridIndicator;
    }

    public override void EnterState()
    {
        Debug.Log("Entered Gameplay State");
        GameManager.Instance.DisplayInputText("Move Indicator to choose Hero\n \n B_Buy Heros");
    }

    public override void HandleInput(InputManager inputManager)
    {
        Debug.Log("Handle GamePlay Input");
        var controls = inputManager.GetControls();
        ExitState();

        currentControls = controls;


        if (controls != null && gridIndicator != null)
        {

            moveAction = ctx => gridIndicator?.HandleIndicatorMove(ctx.ReadValue<Vector2>());
            submitAction = ctx => gridIndicator?.HandleHeroSelected();
            shopAction = ctx => gridIndicator?.ActiveShopMenu();
            cancelAction = ctx => gridIndicator?.CancleSelected();


            currentControls.GamePlay.Move.performed += moveAction;
            currentControls.GamePlay.Submit.performed += submitAction;
            currentControls.GamePlay.Shop.performed += shopAction;
            currentControls.GamePlay.Cancle.performed += cancelAction;
        }
    }

    public override void ExitState()
    {
        Debug.Log("Exited Gameplay State");
        if (currentControls == null)
        {

            currentControls = InputManager.Instance.GetControls();
        }
        if (moveAction != null) currentControls.GamePlay.Move.performed -= moveAction;
        if (submitAction != null) currentControls.GamePlay.Submit.performed -= submitAction;
        if (shopAction != null) currentControls.GamePlay.Shop.performed -= shopAction;
        if (cancelAction != null) currentControls.GamePlay.Cancle.performed -= cancelAction;


        moveAction = null;
        submitAction = null;
        shopAction = null;
        cancelAction = null;
    }

    public override void UpdateState()
    {
        //Debug.Log("Update Gameplay State");
        InputManager.Instance.SetInputMode(InputManager.InputMode.Gameplay);
    }
}
