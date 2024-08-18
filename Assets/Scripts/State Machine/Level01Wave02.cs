using UnityEngine;

public class Level01Wave02 : IState
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

        EventManager.current.OnEnemyKilled += EnemyDown;

        _gameState.waveContainers[1].SetActive(true);

        _gameState.SpawnEnemyAfterSeconds(_enemies[0], 0f);

        // _gameState.StartWaveAfterSeconds(11f);
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