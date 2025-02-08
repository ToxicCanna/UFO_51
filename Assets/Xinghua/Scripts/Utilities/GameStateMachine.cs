using UnityEngine;

public class GameStateMachine : BaseStateMachine
{
    // Keep track of all the states of gamePlay
    private MovingHero _heroSelectState;
    [SerializeField] private GridIndicator _gridIndicator;

    public MovingHero HeroSelect => _heroSelectState;
    public GridIndicator GridIndicator => _gridIndicator;

    private GamePlayState gameplayState;
    private UIState uiState;
    private AttackState attackState;
    private MoveHeroState moveState;
    private ShopHeroState shopState;

    public string currentState;
    private void Awake()
    {
        uiState = new UIState(_gridIndicator);

        if (_gridIndicator == null)
        {
            Debug.LogError("GridIndicator not assigned in the Inspector!");
            return;
        }

        gameplayState = new GamePlayState(_gridIndicator);
        attackState = new AttackState(_gridIndicator);
        moveState = new MoveHeroState(_gridIndicator);
        shopState = new ShopHeroState(_gridIndicator);
    }
    private void Start()
    {
        // Ensure a default state is set
        if (gameplayState != null)
        {
            SetState(gameplayState); // Ensure the default state is set here
        }
        else
        {
            Debug.LogError("GameplayState is null! Ensure it is initialized.");
        }
    }
    public void SetGameState(BaseState value)
    {
        SetState(value);
    }

    public void SwitchToUIState()
    {

        SetState(uiState);
    }
    public void SwitchToShopState()
    {

        SetState(shopState);
    }


    public void SwitchToGameplayState()
    {
        SetState(gameplayState);
        currentState = "gameplay";
    }
    public void SwitchToAttackState()
    {
        SetState(attackState);
    }
    public void SwitchToMoveHeroState()
    {
        SetState(moveState);
        currentState = "MoveHero";
    }
}

