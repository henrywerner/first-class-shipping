using System.Collections;
using UnityEngine;

public class Level01Wave03 : ILevelState
{
    private FsmLevel01 _gameState;
    public Enemy[] _enemies { get; private set; }
    private int enemiesKilled;

    public Level01Wave03(FsmLevel01 gameStateFsm, Enemy[] enemies) {
        _gameState = gameStateFsm;
        _enemies = enemies;
        enemiesKilled = 0;
    }

    public void Enter()
    {
        Debug.Log("Start Wave 1 - 3");
        EventManager.current.OnEnemyDispatched += EnemyDown;
        _gameState.waveContainers[2].SetActive(true);
        
        try
        {
            _gameState.SpawnEnemyAfterSeconds(_enemies[0], 0f);
            _gameState.SpawnEnemyAfterSeconds(_enemies[1], 0f);
            _gameState.SpawnEnemyAfterSeconds(_enemies[2], 0f);
            _gameState.SpawnEnemyAfterSeconds(_enemies[3], 0f);

            _gameState.SpawnEnemyAfterSeconds(_enemies[4], 7f);
            _gameState.SpawnEnemyAfterSeconds(_enemies[5], 7f);

            _gameState.SpawnEnemyAfterSeconds(_enemies[6], 12f);
            _gameState.SpawnEnemyAfterSeconds(_enemies[7], 12f);
        }
        catch (System.Exception)
        {
            Debug.Log("Wave 3 doesn't have all of its expected enemies");
            throw;
        }
       
    }

    private void EnemyDown()
    {
        enemiesKilled++;
        if (enemiesKilled >= _enemies.Length)
        {
            _gameState.NextWave();
        }
    }

    public void Exit()
    {
        EventManager.current.OnEnemyDispatched -= EnemyDown;
    }

    public void FixedTick()
    {

    }

    public void Tick()
    {

    }

    public int GetNumberOfEnemies()
    {
        return _enemies.Length;
    }
}