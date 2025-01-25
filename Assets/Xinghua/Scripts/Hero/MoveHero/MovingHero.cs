using Unity.VisualScripting;
using UnityEngine;

public class MovingHero : BaseState
{
    private HeroStateMachine  _stateMachine;
    public MovingHero(HeroStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }
    public override void EnterState()
    {
        //show highlight path
    }

    public override void ExitState()
    {
        //confirm target
    }



    public override void UpdateState()
    {
        Debug.Log("base state update game state");
      // HandleInput();
    //move to target
}

  
}
