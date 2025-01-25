public class GamePlayStateMachine : BaseStateMachine
{
    // Keep track of all the states of gamePlay
    private MovingHero _heroSelectState;
    private GridIndicator _gridIndicatorState;

    public MovingHero HeroSelect => _heroSelectState;
    public GridIndicator GridIndicator => _gridIndicatorState;
    private void Awake()
    {
        _heroSelectState = new MovingHero(this);
       // _heroSelectState = GetComponent<MovingHero>();
    }

    private void Start()
    {
        // Switch to the default state of patrolling
        SetState(_heroSelectState);
    }
}
