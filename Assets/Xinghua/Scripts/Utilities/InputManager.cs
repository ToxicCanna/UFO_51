using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public enum InputMode { Gameplay, UI,UINav }
    public InputMode CurrentMode { get; private set; }
    private PlayerControls controls;
    [SerializeField] GridIndicator indicatorMove;
    [SerializeField] HeroSelect heroSelect;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Multiple instances of InputManager detected. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        controls = new PlayerControls();
    

    }
    public void SetInputMode(InputMode mode)
    {
        CurrentMode = mode;

        if (mode == InputMode.Gameplay)
        {
            controls.GamePlay.Enable();
            controls.UI.Disable();
        }
        else if (mode == InputMode.UI)
        {
            controls.GamePlay.Disable();
            controls.UI.Enable();
        }
      

        //Debug.Log($"Input mode switched to: {mode}");
    }

    private void HandleUINavigation(Vector2 vector2)
    {
        Debug.Log("WASD for UI nav now");
    }
    public PlayerControls GetControls()
    {
        return controls;
    }

}
