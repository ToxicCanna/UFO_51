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
    private System.Action<InputAction.CallbackContext> pauseAction;
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
        if (gridIndicator == null)
        {
            Debug.LogWarning("GridIndicator is missing! Attempting to reassign.");
            gridIndicator = GameObject.FindAnyObjectByType<GridIndicator>(); 
            if (gridIndicator == null)
            {
                Debug.LogError("GridIndicator is still null, skipping input handling.");
                return; 
            }
        }
       

        // Debug.Log("Handle GamePlay Input");
        var controls = inputManager.GetControls();
        ExitState();

        currentControls = controls;



        if (controls != null && gridIndicator != null)
        {
            if (moveAction == null)
            {
                moveAction = ctx =>
                {
                    if (gridIndicator == null) return;
                    gridIndicator.HandleIndicatorMove(ctx.ReadValue<Vector2>());
                };
            }

            shopAction = ctx => gridIndicator?.ActiveShopMenu();


            currentControls.GamePlay.Move.performed += moveAction;

            currentControls.GamePlay.Shop.performed += shopAction;
            if (submitAction == null)
            {
                submitAction = ctx =>
                {
                    if (gridIndicator == null) return;
                    gridIndicator.HandleHeroSelected();
                };
                currentControls.GamePlay.Submit.performed += submitAction;
            }

            cancelAction = ctx => gridIndicator?.CancleSelected();
            currentControls.GamePlay.Cancle.performed += cancelAction;

            pauseAction = ctx => gridIndicator?.PauseGame();
            currentControls.GamePlay.Pause.performed += pauseAction;
        }
        
    }

    public override void ExitState()
    {
        //Debug.Log("Exited Gameplay State");
        if (currentControls == null)
        {

            currentControls = InputManager.Instance.GetControls();
        }
     

        if (gridIndicator == null)
        {
            Debug.LogWarning("GridIndicator has been destroyed. Skipping event unbinding.");
            return;
        }

        if (moveAction != null) currentControls.GamePlay.Move.performed -= moveAction;
       
        if (shopAction != null) currentControls.GamePlay.Shop.performed -= shopAction;
      


        moveAction = null;

        if (submitAction != null) currentControls.GamePlay.Submit.performed -= submitAction;
        submitAction = null;

        shopAction = null;

        if (cancelAction != null) currentControls.GamePlay.Cancle.performed -= cancelAction;
        cancelAction = null;
    }

    public override void UpdateState()
    {
        //Debug.Log("Update Gameplay State");
        InputManager.Instance.SetInputMode(InputManager.InputMode.Gameplay);
    }
}
