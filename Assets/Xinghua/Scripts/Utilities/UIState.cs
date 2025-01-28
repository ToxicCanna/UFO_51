using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
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

        // band UI action
        controls.UI.Navigate.performed += ctx => UINavManager.Instance.HandleNavigation(ctx.ReadValue<Vector2>());
       // controls.UI.Navigate.performed += OnNavigate;

    }

    public override void ExitState()
    {
        var controls = InputManager.Instance.GetControls();
        Debug.Log("Exited UI State");
        controls.UI.Navigate.performed -= ctx => UINavManager.Instance.HandleNavigation(ctx.ReadValue<Vector2>());
    }
    //private void OnNavigate(InputAction.CallbackContext context)
    //{
    //    Debug.Log("UI Navigation");
    //}


    public override void UpdateState()
    {
        Debug.Log("Update UI state");
        InputManager.Instance.SetInputMode(InputManager.InputMode.UI);
    }
}
