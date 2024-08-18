using UnityEngine;

public class Level01Wave02 : IState
{
    private FsmLevel01 _gameState;

    public Enemy[] _enemies { get; private set; }

    //TODO: add event listener system for tracking when all the enemies are dead so that you can progress to the next wave.

    public Level01Wave02(FsmLevel01 gameStateFsm, Enemy[] enemies) {
        _gameState = gameStateFsm;
        _enemies = enemies;
    }

    public void Enter()
    {
        Debug.Log("Start Wave 1 - 2");

        _gameState.waveContainers[1].SetActive(true);

        _gameState.SpawnEnemyAfterSeconds(_enemies[0], 0f);

        _gameState.StartWaveAfterSeconds(11f);
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