using System.Collections;
using System.Collections.Generic;
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

    private Dictionary<string, List<string>> heroActionsMapping = new Dictionary<string, List<string>>
    {
        { "Basic", new List<string> { "Move" } },
        { "Knight", new List<string> {  "Move", "Attack"} },
        { "Thieft", new List<string> { "Move", "Attack"} },
        { "Range", new List<string> { "Move", "Attack"} },
        { "Healer", new List<string> { "Move", "Attack","Heal"} }
    };
    private Dictionary<string, int> heroCostMapping = new Dictionary<string, int>
    {
        { "Ba", 1 },
        { "Kn", 4 },
        { "Th", 3 },
        { "Ra", 2 },
        { "He", 3 }
    };


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

    public void UpdateHeroActions(HeroPath heroPath)
    {
        if (heroPath == null) return;

        string heroType = heroPath.heroType;
        if (!heroActionsMapping.TryGetValue(heroType, out List<string> availableActions))
        {
            Debug.LogWarning($"Hero type {heroType} not found in action mapping.");
            return;
        }
        Debug.Log("availableActions" + availableActions.Count);
        for (int i = 0; i < buttonsHeroActions.Length; i++)
        {
            buttonsHeroActions[i].gameObject.SetActive(true); 
            buttonsHeroActions[i].interactable = false;       
            SetButtonColor(buttonsHeroActions[i], false);   
        }


        for (int i = 0; i < buttonsHeroActions.Length; i++)
        {
            string actionName = buttonsHeroActions[i].name;

            if (availableActions.Contains(actionName))
            {
                buttonsHeroActions[i].interactable = true;
                SetButtonColor(buttonsHeroActions[i], true);
            }
            else
            {
                buttonsHeroActions[i].interactable = false;
                SetButtonColor(buttonsHeroActions[i], false);
            }
        }

    }
    private void SetButtonColor(Button button, bool isActive)
    {
        Color targetColor = isActive ? Color.white : Color.gray;

        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.color = targetColor;
        }
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
        Debug.Log("buttons.Length" + buttons.Length);

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
    public void InitSelectorPositionInHeroActionsZone()
    {

        if (buttons.Length > 0)
        {
            selector.position = buttonsHeroActions[0].transform.position;

        }
    }

    public void InitSelectorPositionInHeroShopZone()
    {
        if (buttons.Length > 0)
        {
            selector.position = buttons[0].transform.position;

        }
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
            selector.position = buttonsHeroActions[actionIndex].transform.position;

        }
    }

    private void UpdateSelectorPositionShop()
    {
        if (selector == null)
        {
            Debug.LogWarning("[UISelector] Selector is missing!");
            return;
        }

        if (buttons.Length > 0)
        {
            selector.position = buttons[shopIndex].transform.position;
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
        ProcessHeroShopSelected(selectedButtonName);
        buttons[shopIndex].onClick.Invoke();
    }

    public void HandleActionsSelection()
    {
        Button selectedButton = buttonsHeroActions[actionIndex];
        if (!selectedButton.interactable)
        {
            Debug.Log("Cannot select this action, button is disabled.");
            return; 
        }

        string firstTwoLetters = buttonsHeroActions[actionIndex].name.Substring(0, 2);
        selectedButtonName = firstTwoLetters;
        ProcessActionSelected();
        buttonsHeroActions[actionIndex].onClick.Invoke();
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
            gameStateMachine.SwitchToHealState();
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

    int cost;
    private void ProcessHeroShopSelected(string button)
    {
        //check the coin number
        Debug.Log("ProcessHeroShopSelected nnnnnname" +button);
        Debug.Log("ProcessHeroShopSelected nnnnnname" + button);
        Debug.Log("Bufffff" + CanPurchaseHero(button));

        if (CanPurchaseHero(button))//0 should be the hero cost ,here shoul check whith button
        {
           
            spawnHero.SpawnNew(selectedButtonName,cost);
        }
     
        SwithToGamePlayState();
    }
   
    public bool CanPurchaseHero(string heroName)
    {
        if (!heroCostMapping.TryGetValue(heroName, out int heroCost))
        {

            Debug.LogWarning($"Hero {heroName} not found in cost mapping.");
            return false;
        }
        cost = heroCost;
        Debug.Log("ProcessHeroShopSelected costtttttt" + heroCost);
        Debug.Log("my money" + GameManager.Instance.GetCurrentTurnCoin());
        if (GameManager.Instance.GetCurrentTurnCoin()< heroCost)
        {
            Debug.Log("Not enough coins to purchase " + heroName);
            return false;
        }

        return true;
    }

}
