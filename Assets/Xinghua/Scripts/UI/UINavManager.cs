using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UINavManager : MonoBehaviour
{
    public static UINavManager Instance;
    public Button[] buttons;
    public Button[] buttonsHeroActions;
    public RectTransform selector; // Assign the selector GameObject
  
    private int shopIndex = 0; 
    private int actionIndex = 0; 


    private string selectedButtonName;

    [SerializeField] GameStateMachine gameStateMachine;
    [SerializeField] SpawnHero spawnHero;


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
        UpdateSelectorPositionShop();
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



    public void HandleNavigationInActionsZone(Vector2 direction)
    {
        MoveSelectorWithHeroActions((int)direction.x);
    }

    public void HandleNavigationInShopZone(Vector2 direction)
    {

        MoveSelectorInShop((int)direction.x);
    }

    public void MoveSelectorInShop(int direction)
    {
        if (buttons.Length == 0) return;
        Debug.Log("buttons.Length"+ buttons.Length);

        shopIndex = (shopIndex + direction + buttons.Length) % buttons.Length; // Wrap around
        Debug.Log($"After move: currentIndex = {shopIndex}");
        UpdateSelectorPositionShop();
    }

    public void MoveSelectorWithHeroActions(int direction)
    {
        if (buttonsHeroActions.Length == 0) return;

        actionIndex = (actionIndex + direction + buttonsHeroActions.Length) % buttonsHeroActions.Length; // Wrap around
        UpdateSelectorPositionAction();
    }

    private void UpdateSelectorPositionInHeroActionsZone()
    {
        if (selector == null)
        {
            Debug.LogWarning("[UISelector] Selector is missing!");
            return;
        }

      /*  if (buttonsHeroActions == null || buttonsHeroActions.Length == 0)
        {
            Debug.LogWarning("[UISelector] buttonsHeroActions array is empty or null!");
            return;
        }

        if (currentIndex < 0 || currentIndex >= buttonsHeroActions.Length)
        {
            Debug.LogWarning($"[UISelector] currentIndex ({currentIndex}) is out of range! Valid range: 0 - {buttonsHeroActions.Length - 1}");
            return;
        }
*/
        if (buttons.Length > 0)
        {
            selector.position = buttonsHeroActions[actionIndex].transform.position;
            // Debug.Log("[UISelector] Selector moved to: " + selector.position);
        }
    }

    public void UpdateSelectorPositionShop()
    {
        if (selector == null)
        {
            Debug.LogWarning("[UISelector] Selector is missing!");
            return;
        }

        if (buttons.Length > 0)
        {
            selector.position = buttons[shopIndex].transform.position;
            // Debug.Log("[UISelector] Selector moved to: " + selector.position);
        }
    }

    public void UpdateSelectorPositionAction()
    {
        if (selector == null)
        {
            Debug.LogWarning("[UISelector] Selector is missing!");
            return;
        }

        if (buttonsHeroActions.Length > 0)
        {
            selector.position = buttonsHeroActions[actionIndex].transform.position;
            // Debug.Log("[UISelector] Selector moved to: " + selector.position);
        }
    }
    public void HandleHeroShopSelection()
    {
        string firstTwoLetters = buttons[shopIndex].name.Substring(0, 2);
        selectedButtonName = firstTwoLetters;
        ProcessHeroShopSelected();
        buttons[shopIndex].onClick.Invoke();
    }

    public void HandleActionsSelection()
    {
        string firstTwoLetters = buttonsHeroActions[actionIndex].name.Substring(0, 2);
        selectedButtonName = firstTwoLetters;
        ProcessActionSelected();
        buttonsHeroActions[actionIndex].onClick.Invoke();
    }


    private void ProcessHeroShopSelected()
    {
        Debug.Log("spawn hero");
        spawnHero.SpawnNew(selectedButtonName);

        SwithToGamePlayState();
    }

    private void ProcessActionSelected()
    {
        Debug.Log(" selectedButtonName" + selectedButtonName);
        if (selectedButtonName == "At")//attack button selected
        {
            gameStateMachine.SwitchToAttackState();
        }
        else if (selectedButtonName == "Mo")//healbutton selected
        {
            gameStateMachine.SwitchToMoveHeroState();
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
