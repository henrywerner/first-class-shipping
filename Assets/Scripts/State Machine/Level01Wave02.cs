using UnityEngine;

public class Level01Wave02 : IState
{
    private FsmLevel01 _gameState;

    public Level01Wave02(FsmLevel01 gameStateFsm) {
        _gameState = gameStateFsm;
    }

    public void Enter()
    {
        Debug.Log("Start Wave 1 - 2");
    }

    public void Exit()
    {

    }

    public void FixedTick()
    {

    }

    public void Tick()
    {

    }
}