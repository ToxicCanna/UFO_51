using UnityEngine;

public class GamePlayState : BaseState
{
    private GridIndicator gridIndicator;
    private PlayerControls currentControls;

    public GamePlayState(GridIndicator gridIndicator)
    {
        this.gridIndicator = gridIndicator;
    }

    public override void EnterState()
    {
        Debug.Log("Entered Gameplay State");
    }

    public override void HandleInput(InputManager inputManager)
    {
       // Debug.Log("Handle GamePlay State");
        var controls = inputManager.GetControls();
        currentControls = controls;
        if (controls != null && gridIndicator != null)
        {
            currentControls.GamePlay.Move.performed += ctx => gridIndicator?.HandleIndicatorMoveNew(ctx.ReadValue<Vector2>());
           // currentControls.GamePlay.Switch.performed += ctx => gridIndicator?.HandleSelectHero();
            currentControls.GamePlay.Submit.performed += ctx => gridIndicator?.HandleSubmitHeroSelected();

            currentControls.GamePlay.Submit.performed += ctx => gridIndicator?.ChooseHeroAction();
            currentControls.GamePlay.Shop.performed += ctx => gridIndicator?.ActiveShopMenu();
            //currentControls.GamePlay.Cancle.performed += ctx => gridIndicator?.CancleSelected();
        }
        else
        {
            Debug.Log("gridIndicator is null");
        }
    }

    public override void ExitState()
    {
        Debug.Log("Exited Gameplay State");
        currentControls.GamePlay.Move.performed -= ctx => gridIndicator?.HandleIndicatorMoveNew(ctx.ReadValue<Vector2>());
        //currentControls.GamePlay.Switch.performed -= ctx => gridIndicator?.HandleSelectHero();
        currentControls.GamePlay.Submit.performed -= ctx => gridIndicator?.HandleSubmitHeroSelected();

        currentControls.GamePlay.Submit.performed -= ctx => gridIndicator?.ChooseHeroAction();
        currentControls.GamePlay.Confirm.performed -= ctx => gridIndicator?.MoveToTargetIndicator();
    }

    public override void UpdateState()
    {
        //Debug.Log("Update Gameplay State");
        InputManager.Instance.SetInputMode(InputManager.InputMode.Gameplay);
    }
}
