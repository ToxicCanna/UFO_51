using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UINavManager : MonoBehaviour
{
    public static UINavManager Instance;
    public Button[] buttons; // Assign buttons in order
    public Button[] buttonsHeroActions; // Assign buttons in order
    public RectTransform selector; // Assign the selector GameObject
    private int currentIndex = 0; // Tracks the currently selected button

    private string selectedButtonName;

    [SerializeField] private ShopScript shopScript;

    //xinghua code
    [SerializeField] GameStateMachine gameStateMachine;
    private bool isGamePlayStateActive = false;

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

    private IEnumerator FixSelectorPosition()
    {
        yield return new WaitForEndOfFrame(); // Ensures UI layout is finalized
        UpdateSelectorPosition();
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

        //UpdateSelectorPosition(); // Position the selector on the first button
        StartCoroutine(FixSelectorPosition());
    }



    public void HandleNavigation(Vector2 direction)
    {
        Debug.Log("IsHeroSubmitted" + GameManager.Instance.IsHeroSubmitted);
        if (GameManager.Instance.IsHeroSubmitted)
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
        else
        {
            if (direction.x > 0)
            {
                MoveSelector(1); // Move to the next button

            }
            else if (direction.x < 0)
            {

                MoveSelector(-1); // Move to the previous button
            }
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
    public void HandleButtonsSelection()
    {
        if (GameManager.Instance.IsHeroSubmitted)
        {

            HandleActionsSelection();
        }
        else
        {
            HandleHeroShopSelection();

        }
        
    }

    public void HandleHeroShopSelection()
    {
        string firstTwoLetters = buttons[currentIndex].name.Substring(0, 2);
        selectedButtonName = firstTwoLetters;
        ProcessHeroShopSelected();
        buttons[currentIndex].onClick.Invoke();
    }

    public void HandleActionsSelection()
    {
        string firstTwoLetters = buttonsHeroActions[currentIndex].name.Substring(0, 2);
        selectedButtonName = firstTwoLetters;
        buttonsHeroActions[currentIndex].onClick.Invoke();
        ProcessActionSelected();
    }


    private void ProcessHeroShopSelected()
    {
        if (selectedButtonName == "Ba")//attack button selected
        {
            Debug.Log("spawn Basic hero in the scene ");
        }
        else if (selectedButtonName == "He")//healbutton selected
        {

            Debug.Log("spawn Healer hero in the scene ");
        }
        SwithToGamePlayState();

    }
    private void ProcessActionSelected()
    {
        Debug.Log(" selectedButtonName" + selectedButtonName);
        if (selectedButtonName == "At")//attack button selected
        {
            gameStateMachine.SwitchToAttackState();
        }
        else if (selectedButtonName == "He")//healbutton selected
        {
            // hero begin heal ;before use this function need check selectes is heal or not
            //active button should have 2 state, if selected hero is not heal , the button should disable
            Debug.Log("heal function called");//but function do not write here
            SwithToGamePlayState();
        }
        else
        {
            SwithToGamePlayState();
        }

    }


    internal void SwithToGamePlayState()
    {
        if (isGamePlayStateActive) return;

        if (gameStateMachine != null)
        {
            gameStateMachine.SwitchToGameplayState();
        }

    }

    internal void CancleSelected()
    {
        gameStateMachine.SwitchToGameplayState();
    }
}
