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
    private WinState winState;

    private void Awake()
    {
        uiState = new UIState();

        if (_gridIndicator == null)
        {
            Debug.LogError("GridIndicator not assigned in the Inspector!");
            return;
        }

        gameplayState = new GamePlayState(_gridIndicator);
        attackState = new AttackState(_gridIndicator);
        winState = new WinState(_gridIndicator);
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
    private void OnEnable()
    {
        if (_gridIndicator != null)
        {
            _gridIndicator.activeShop += SwitchToUIState;

        }
    }
    private void OnDisable()
    {
        if (_gridIndicator != null)
        {
            _gridIndicator.activeShop -= SwitchToUIState;

        }
    }


    public void SwitchToUIState()
    {

        SetState(uiState);
    }

    public void SwitchToGameplayState()
    {
        SetState(gameplayState);
    }
    public void SwitchToAttackState()
    {
        SetState(attackState);
    }
    public void SwitchToWinState()
    {
        SetState(winState);
    }
}

