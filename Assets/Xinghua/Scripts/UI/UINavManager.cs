using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINavManager : MonoBehaviour
{
    public static UINavManager Instance;
    public Button[] buttons;//those buttons for hero shop
    public Button[] buttonsHeroActions;

    public List<Sprite> activeSprites;
    public List<Sprite> inactiveSprites;

    public List<Sprite> activeShopSprites;
    public List<Sprite> inactiveShopSprites;

    public RectTransform selector; // Assign the selector GameObject

    private int shopIndex = 0;
    private int actionIndex = 0;


    private string selectedButtonName;

    [SerializeField] GameStateMachine gameStateMachine;
    [SerializeField] SpawnHero spawnHero;

    private Dictionary<string, List<string>> heroActionsMapping = new Dictionary<string, List<string>>
    {
        { "Basic", new List<string> { "Move" } },
        { "Knight", new List<string> {  "Move"} },
        { "Thief", new List<string> { "Move" } },
        { "Range", new List<string> { "Move", "Attack"} },
        { "Healer", new List<string> { "Move", "Heal"} }
    };
    private Dictionary<string, int> heroCostMapping = new Dictionary<string, int>
    {
        { "Basic", 1 },
        { "Knight", 4 },
        { "Thief", 3 },
        { "Ranged", 2 },
        { "Healer", 3 }
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


    private void FixedUpdate()
    {
        UpdateShopButtons();
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

        Debug.Log("availableActions count: " + availableActions.Count);

        for (int i = 0; i < buttonsHeroActions.Length; i++)
        {
            string actionName = buttonsHeroActions[i].name;
            bool isActive = availableActions.Contains(actionName);

            buttonsHeroActions[i].gameObject.SetActive(true);
            buttonsHeroActions[i].interactable = isActive;

            if (i < activeSprites.Count && i < inactiveSprites.Count)
            {
                SetButtonSprite(buttonsHeroActions[i], isActive, activeSprites[i], inactiveSprites[i]);
            }
        }
    }


    private void SetButtonSprite(Button button, bool isActive, Sprite activeSprite, Sprite inactiveSprite)
    {
        if (button == null) return;

        Image buttonImage = button.GetComponentInChildren<Image>();
        if (buttonImage != null)
        {
            buttonImage.sprite = isActive ? activeSprite : inactiveSprite;
            Debug.Log($"Setting sprite for {button.name}: {(isActive ? "Active" : "Inactive")}");
        }
        else
        {
            Debug.LogWarning($"No Image component found for button {button.name}");
        }
    }



    private IEnumerator FixSelectorPosition()
    {
        yield return new WaitForEndOfFrame(); // Ensures UI layout is finalized
        UpdateSelectorPositionShop();
    }

    private void Start()
    {

        UpdateShopButtons();
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
        UpdateShopButtons();
    }



    public void HandleNavigationInActionsZone(Vector2 direction)
    {
        MoveSelectorWithHeroActions((int)direction.x);
    }

    public void HandleNavigationInShopZone(Vector2 direction)
    {

        if (buttons.Length == 0) return;
        Debug.Log("buttons.Length" + buttons.Length);

        shopIndex = (shopIndex + (int)direction.x + buttons.Length) % buttons.Length; // Wrap around
        Debug.Log($"After move: currentIndex = {shopIndex}");
        UpdateSelectorPositionShop();

        AudioManager.Instance.Play("SelectorMove");
    }

    public void MoveSelectorWithHeroActions(int direction)
    {
        if (buttonsHeroActions.Length == 0) return;

        actionIndex = (actionIndex + direction + buttonsHeroActions.Length) % buttonsHeroActions.Length; // Wrap around
        UpdateSelectorPositionAction();

        // Play the selection sound
        AudioManager.Instance.Play("SelectorMove");
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

    public bool CanPurchaseHero(string heroName)
    {
        if (!heroCostMapping.TryGetValue(heroName, out int heroCost))
        {

            Debug.LogWarning($"Hero {heroName} not found in cost mapping.");
            return false;
        }
        cost = heroCost;
        if (GameManager.Instance.GetCurrentTurnCoin() < heroCost)
        {
            GameManager.Instance.DisplayErrorText("Not enough coins to purchase");
            return false;
        }


        if (GridManager.Instance.IsSpawnOccupied())//spawn location was occupied
        {
            GameManager.Instance.DisplayErrorText("Move the hero in spawn location first");
            return false;
        }

        return true;
    }

    public void UpdateShopButtons()
    {
        int playerCoin = GameManager.Instance.GetCurrentTurnCoin();

        Debug.Log("UpdateShopButtons" + playerCoin);
        for (int i = 0; i < buttons.Length; i++)
        {
            string heroName = buttons[i].name;

            if (heroCostMapping.TryGetValue(heroName, out int heroCost))
            {
                bool canAfford = false;

                if (playerCoin >= 3)
                {
                    canAfford = true;

                }
                else if (playerCoin >= 2 && (heroName == "Ranged" || heroName == "Basic"))
                {
                    canAfford = true;
                }
                else if (playerCoin == 1 && heroName == "Basic")
                {
                    canAfford = true;
                }
                else if (playerCoin == 0)
                {
                    canAfford = false;
                }

                buttons[i].interactable = canAfford;


                if (i < activeShopSprites.Count && i < inactiveShopSprites.Count)
                {
                    SetButtonSprite(buttons[i], canAfford, activeShopSprites[i], inactiveShopSprites[i]);
                }
            }
            else
            {
                Debug.LogWarning($"Hero {heroName} not found in cost mapping.");
            }
        }

    }

    public void HandleHeroShopSelection()
    {

        var button = buttons[shopIndex];

        buttons[shopIndex].onClick.Invoke();

        if (!button.interactable)
        {

            GameManager.Instance.DisplayErrorText("Cannot select disabled button");
            return;
        }


        if (CanPurchaseHero(button.name))//0 should be the hero cost ,here shoul check whith button
        {

            spawnHero.SpawnNew(button.name, cost);
            //UpdateShopButtons();

        }

        SwithToGamePlayState();
    }


}
