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
        var controls = inputManager.GetControls();

        // band UI action
        controls.UI.Navigate.performed -= OnNavigate;
        controls.UI.Navigate.performed += OnNavigate;

    }

    public override void ExitState()
    {
        Debug.Log("Exited UI State");
    }
    private void OnNavigate(InputAction.CallbackContext context)
    {
        Debug.Log("UI Navigation");
    }


    public override void UpdateState()
    {
        Debug.Log("Update UI state");
        InputManager.Instance.SetInputMode(InputManager.InputMode.UI);
    }
}
