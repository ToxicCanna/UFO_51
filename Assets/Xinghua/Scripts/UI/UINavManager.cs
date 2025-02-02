using UnityEngine;
using UnityEngine.UI;

public class UINavManager : MonoBehaviour
{
    public static UINavManager Instance;
    public Button[] buttons; // Assign buttons in order
    public Button[] buttonsHeroActions; // Assign buttons in order
    public RectTransform selector; // Assign the selector GameObject
    private int currentIndex = 0; // Tracks the currently selected button

    [SerializeField] private ShopScript shopScript;

    //xinghua code
    [SerializeField] GameStateMachine gameStateMachine;
    //end
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Multiple instances of InputManager detected. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (buttons.Length == 0)
        {
            Debug.LogWarning("[UISelector] No buttons assigned!");
            return;
        }

        if (selector == null)
        {
            Debug.LogWarning("[UISelector] Selector GameObject is not assigned!");
            return;
        }

        UpdateSelectorPosition(); // Position the selector on the first button
       
    }

  

    public void HandleNavigation(Vector2 direction)
    {
        UpdateSelectorPositionInHeroActionsZone();
        if (direction.x > 0)
        {
            //MoveSelector(1); // Move to the next button
            MoveSelectorWithHeroActions(1);
        }
        else if (direction.x < 0)
        {
            MoveSelectorWithHeroActions(-1);
            // MoveSelector(-1); // Move to the previous button
        }
    
    }

    public void MoveSelector(int direction)
    {
        if (buttons.Length == 0) return;

        currentIndex = (currentIndex + direction + buttons.Length) % buttons.Length; // Wrap around
        // Debug.Log("[UISelector] Moved to button index: " + currentIndex + " (" + buttons[currentIndex].name + ")");
        UpdateSelectorPosition();
    }

    public void MoveSelectorWithHeroActions(int direction)
    {
        if (buttonsHeroActions.Length == 0) return;

        currentIndex = (currentIndex + direction + buttonsHeroActions.Length) % buttonsHeroActions.Length; // Wrap around
        Debug.Log("UpdateSelectorPositionInHeroActionsZone");
        UpdateSelectorPositionInHeroActionsZone();
    }
    private void UpdateSelectorPositionInHeroActionsZone()
    {
        if (selector == null)
        {
            Debug.LogWarning("[UISelector] Selector is missing!");
            return;
        }

        if (buttons.Length > 0)
        {
            selector.position = buttonsHeroActions[currentIndex].transform.position;
            // Debug.Log("[UISelector] Selector moved to: " + selector.position);
        }
    }

    private void UpdateSelectorPosition()
    {
        if (selector == null)
        {
            Debug.LogWarning("[UISelector] Selector is missing!");
            return;
        }

        if (buttons.Length > 0)
        {
            selector.position = buttons[currentIndex].transform.position;
            // Debug.Log("[UISelector] Selector moved to: " + selector.position);
        }
    }

    public void HandleSelection()
    {

        Debug.Log("[UISelector] Selected button: " + buttons[currentIndex].name);
        //buttons[currentIndex].onClick.Invoke();

    }
    public void HandleActionsSelection()
    {

        Debug.Log("[UISelector] Selected button: " + buttons[currentIndex].name);
        buttonsHeroActions[currentIndex].onClick.Invoke();
    }

    //xinghua code for exit UI Input
    internal void SwithToGamePlayState()
    {
        if (gameStateMachine != null)
        {
            gameStateMachine.SwitchToGameplayState();
        }
    }
    //xinghua code end
}
