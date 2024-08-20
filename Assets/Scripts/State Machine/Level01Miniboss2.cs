using System.Collections;
using UnityEngine;

public class Level01Miniboss2 : ILevelState
{
    private FsmLevel01 _gameState;
    public Enemy[] _enemies { get; private set; }
    private int enemiesKilled;

    public Level01Miniboss2(FsmLevel01 gameStateFsm, Enemy[] enemies) {
        _gameState = gameStateFsm;
        _enemies = enemies;
        enemiesKilled = 0;
    }

    public void Enter()
    {
        Debug.Log("Start Wave Miniboss");
        EventManager.current.OnEnemyDispatched += EnemyDown;
        _gameState.waveContainers[6].SetActive(true);
        
        try
        {
            _gameState.SpawnEnemyAfterSeconds(_enemies[0], 0f);
            _gameState.SpawnEnemyAfterSeconds(_enemies[1], 3f);
        }
        catch (System.Exception)
        {
            throw;
        }
       
    }

    private void EnemyDown()
    {
        enemiesKilled++;

        Debug.Log("ENEMIES KILLED: " + enemiesKilled);

        if (enemiesKilled == 1)
        {
            // Spawn next enemy
            _gameState.SpawnEnemyAfterSeconds(_enemies[2], 0f);
            _gameState.SpawnEnemyAfterSeconds(_enemies[3], 0f);
        }
        else if (enemiesKilled == 3)
        {
            // Spawn next enemy
            _gameState.SpawnEnemyAfterSeconds(_enemies[4], 0f);
            _gameState.SpawnEnemyAfterSeconds(_enemies[5], 0f);
        }
        else if (enemiesKilled == 5)
        {
            // After 3 enemies, move to pos 2
            _enemies[0].gameObject.GetComponent<EnemyMiniboss2>().MoveToPosition2();
            _gameState.SpawnEnemyAfterSeconds(_enemies[6], 3f);
            _gameState.SpawnEnemyAfterSeconds(_enemies[7], 3f);
        }
        else if (enemiesKilled == 7)
        {
            // Spawn next enemy
            _gameState.SpawnEnemyAfterSeconds(_enemies[8], 0f);
            _gameState.SpawnEnemyAfterSeconds(_enemies[9], 2f);
            _gameState.SpawnEnemyAfterSeconds(_enemies[10], 2f);
            _gameState.SpawnEnemyAfterSeconds(_enemies[11], 2f);
            _gameState.SpawnEnemyAfterSeconds(_enemies[12], 4f);
            _gameState.SpawnEnemyAfterSeconds(_enemies[13], 4f);
            _gameState.SpawnEnemyAfterSeconds(_enemies[14], 4f);
            _gameState.SpawnEnemyAfterSeconds(_enemies[15], 6f);
            _gameState.SpawnEnemyAfterSeconds(_enemies[16], 6f);
            _gameState.SpawnEnemyAfterSeconds(_enemies[17], 6f);
        }
        else if (enemiesKilled == 17)
        {
            // After another 3 enemies, move to the center position
            _enemies[0].gameObject.GetComponent<EnemyMiniboss2>().MoveToFinalPosition();
            _gameState.SpawnEnemyAfterSeconds(_enemies[18], 6f);
            _gameState.SpawnEnemyAfterSeconds(_enemies[19], 6f);
            _gameState.SpawnEnemyAfterSeconds(_enemies[20], 6f);
            _gameState.SpawnEnemyAfterSeconds(_enemies[21], 6.2f);
            _gameState.SpawnEnemyAfterSeconds(_enemies[22], 6.2f);
            _gameState.SpawnEnemyAfterSeconds(_enemies[23], 6.2f);
        }
        
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
        if (_enemies[0] == null) {
            _gameState.NextWave();
        }
    }

    public void Tick()
    {

    }

    public int GetNumberOfEnemies()
    {
        return _enemies.Length;
    }
}