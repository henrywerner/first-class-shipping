using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FsmLevel01 : StateMachine
{
    public Level01Wave01 wave01 { get; private set; }
    public Level01Wave02 wave02 { get; private set; }

    private Queue<IState> waves = new Queue<IState>();

    [SerializeField] private Enemy[] wave01Enemies;
    [SerializeField] private Enemy[] wave02Enemies;
    [SerializeField] private Enemy[] wave03Enemies;

    void Awake()
    {
        wave01 = new Level01Wave01(this, wave01Enemies);
        wave02 = new Level01Wave02(this);
        waves.Enqueue(wave01);
        waves.Enqueue(wave02);
    }

    void Start()
    {
        ChangeState(wave01);
    }

    public void StartWaveAfterSeconds(float delay) {
        StartCoroutine(StartWaveAfterSecondsCoroutine(delay));
    }

    protected IEnumerator StartWaveAfterSecondsCoroutine(float delay) {
        // NOTE: THIS WILL BREAK IF WE ADD FUNCTIONALITY FOR ENDING THE WAVE WHEN ALL ARE DEAD
        yield return new WaitForSecondsRealtime(delay);
        waves.Dequeue();
        ChangeState(waves.Peek());
    }

    public void SpawnEnemyAfterSeconds(Enemy enemy, float delay) {
        StartCoroutine(SpawnEnemyAfterSecondsCoroutine(enemy, delay));
    }

    protected IEnumerator SpawnEnemyAfterSecondsCoroutine(Enemy enemy, float delay) {
        yield return new WaitForSecondsRealtime(delay);
        enemy.StartMoving();
        yield return null;
    }
}
