using System.Collections;
using UnityEngine;

public class Level01Miniboss1 : ILevelState
{
    private FsmLevel01 _gameState;
    public Enemy[] _enemies { get; private set; }
    private int enemiesKilled;

    public Level01Miniboss1(FsmLevel01 gameStateFsm, Enemy[] enemies) {
        _gameState = gameStateFsm;
        _enemies = enemies;
        enemiesKilled = 0;
    }

    public void Enter()
    {
        Debug.Log("Start Wave Miniboss");
        EventManager.current.OnEnemyDispatched += EnemyDown;
        _gameState.waveContainers[3].SetActive(true);
        
        try
        {
            _gameState.SpawnEnemyAfterSeconds(_enemies[0], 0f);
        }
        catch (System.Exception)
        {
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