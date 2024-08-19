using UnityEngine;

public class Level01Wave02 : ILevelState
{
    private FsmLevel01 _gameState;
    public Enemy[] _enemies { get; private set; }
    private int enemiesKilled;

    public Level01Wave02(FsmLevel01 gameStateFsm, Enemy[] enemies) {
        _gameState = gameStateFsm;
        _enemies = enemies;
        enemiesKilled = 0;
    }

    public void Enter()
    {
        Debug.Log("Start Wave 1 - 2");
        EventManager.current.OnEnemyDispatched += EnemyDown;
        _gameState.waveContainers[1].SetActive(true);

        try
        {
            _gameState.SpawnEnemyAfterSeconds(_enemies[0], 0f);
            _gameState.SpawnEnemyAfterSeconds(_enemies[1], 3f);
            _gameState.SpawnEnemyAfterSeconds(_enemies[2], 8f);
        }
        catch (System.Exception)
        {
            Debug.Log("Wave 2 doesn't have all of its expected enemies");
            throw;
        }
    }

    private void EnemyDown()
    {
        enemiesKilled++;

        if (enemiesKilled == 1) {
            // _gameState.SpawnEnemyAfterSeconds(_enemies[1], 0f);
        }

        if (enemiesKilled == _enemies.Length)
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