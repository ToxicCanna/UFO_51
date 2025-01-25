using UnityEngine;

public class HeroStateMachine : BaseStateMachine
{
    private HeroSelect _heroSlecectState;
    public HeroSelect HeroSlecectState => _heroSlecectState;
    private void Awake()
    {
        _heroSlecectState = GetComponent<HeroSelect>();
    }

}
