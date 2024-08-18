using System.Collections;
using UnityEngine;

public class Level01Wave01 : IState
{
    private FsmLevel01 _gameState;
    public Enemy[] _enemies { get; private set; }
    private int enemiesKilled;

    public Level01Wave01(FsmLevel01 gameStateFsm, Enemy[] enemies) {
        _gameState = gameStateFsm;
        _enemies = enemies;
        enemiesKilled = 0;
    }

    public void Enter()
    {
        Debug.Log("Start Wave 1 - 1");

        EventManager.current.OnEnemyKilled += EnemyDown;

        _gameState.waveContainers[0].SetActive(true);

        _gameState.SpawnEnemyAfterSeconds(_enemies[0], 0f);
        _gameState.SpawnEnemyAfterSeconds(_enemies[1], 0f);

        _gameState.SpawnEnemyAfterSeconds(_enemies[2], 7.5f);
        _gameState.SpawnEnemyAfterSeconds(_enemies[3], 7.5f);
        _gameState.SpawnEnemyAfterSeconds(_enemies[4], 7.5f);

        _gameState.SpawnEnemyAfterSeconds(_enemies[5], 14f);
        _gameState.SpawnEnemyAfterSeconds(_enemies[6], 14f);
        _gameState.SpawnEnemyAfterSeconds(_enemies[7], 14f);
        _gameState.SpawnEnemyAfterSeconds(_enemies[8], 14f);

        // _gameState.StartWaveAfterSeconds(20f);
    }

    private void EnemyDown()
    {
        enemiesKilled++;
        if (enemiesKilled == _enemies.Length)
        {
            _gameState.NextWave();
        }
    }

    public void Exit()
    {
        EventManager.current.OnEnemyKilled -= EnemyDown;
    }

    public void FixedTick()
    {
        
    }

    public void Tick()
    {

    }

}