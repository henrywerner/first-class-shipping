using System.Collections;
using UnityEngine;

public class Level01Wave01 : IState
{
    private FsmLevel01 _gameState;

    public Enemy[] _enemies { get; private set; }

    public Level01Wave01(FsmLevel01 gameStateFsm, Enemy[] enemies) {
        _gameState = gameStateFsm;
        _enemies = enemies;
    }

    public void Enter()
    {
        Debug.Log("Start Wave 1 - 1");

        _gameState.SpawnEnemyAfterSeconds(_enemies[0], 0f);
        _gameState.SpawnEnemyAfterSeconds(_enemies[1], 0f);
        _gameState.SpawnEnemyAfterSeconds(_enemies[2], 7.5f);
        _gameState.SpawnEnemyAfterSeconds(_enemies[3], 7.5f);
        _gameState.SpawnEnemyAfterSeconds(_enemies[4], 7.5f);

        _gameState.StartWaveAfterSeconds(20f);
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