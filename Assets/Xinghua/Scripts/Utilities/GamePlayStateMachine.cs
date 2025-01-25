using UnityEngine;

public class GamePlayStateMachine : BaseStateMachine
{
    // Keep track of all the states
    private  MovingHero _heroSelectState;
    private GridIndicator _gridIndicatorState;

    public MovingHero HeroSelect => _heroSelectState;
    public GridIndicator GridIndicator => _gridIndicatorState;
    private void Awake()
    {
        //_heroSelectState = new MovingHero();
        _heroSelectState =GetComponent<MovingHero>();
        _gridIndicatorState = new GridIndicator();
    }

    private void Start()
    {
        // Switch to the default state of patrolling
        SetState(_heroSelectState);
    }
}
