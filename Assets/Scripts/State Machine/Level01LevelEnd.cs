using System.Collections;
using UnityEngine;

public class Level01LevelEnd : IState
{
    private FsmLevel01 _gameState;

    public Level01LevelEnd(FsmLevel01 gameStateFsm) {
        _gameState = gameStateFsm;
    }

    public void Enter()
    {
        Debug.Log("End Level 1");
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