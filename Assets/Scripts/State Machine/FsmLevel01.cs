using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FsmLevel01 : StateMachine
{
    public Level01Wave01 wave01 { get; private set; }
    public Level01Wave02 wave02 { get; private set; }
    public Level01Wave03 wave03 { get; private set; }
    public Level01Wave04 wave04 { get; private set; }
    public Level01Wave05 wave05 { get; private set; }
    public Level01Miniboss1 miniboss1 { get; private set; }
    public Level01Miniboss2 miniboss2 { get; private set; }
    public Level01LevelEnd levelEnd { get; private set; }

    private Queue<ILevelState> waves = new Queue<ILevelState>();

    [SerializeField] public GameObject[] waveContainers;


    private Enemy[] wave01Enemies, wave02Enemies, wave03Enemies, wave04Enemies, wave05Enemies, wave06Enemies, wave07Enemies, wave08Enemies, wave09Enemies;
    private Enemy[] miniboss01Enemies, miniboss02Enemies;

    void Awake()
    {
        wave01Enemies = waveContainers[0].GetComponentsInChildren<Enemy>();
        wave02Enemies = waveContainers[1].GetComponentsInChildren<Enemy>();
        wave03Enemies = waveContainers[2].GetComponentsInChildren<Enemy>();
        miniboss01Enemies = waveContainers[3].GetComponentsInChildren<Enemy>();
        wave04Enemies = waveContainers[4].GetComponentsInChildren<Enemy>();
        wave05Enemies = waveContainers[5].GetComponentsInChildren<Enemy>();
        miniboss02Enemies = waveContainers[6].GetComponentsInChildren<Enemy>();

        wave01 = new Level01Wave01(this, wave01Enemies);
        wave02 = new Level01Wave02(this, wave02Enemies);
        wave03 = new Level01Wave03(this, wave03Enemies);
        miniboss1 = new Level01Miniboss1(this, miniboss01Enemies);
        wave04 = new Level01Wave04(this, wave04Enemies);
        wave05 = new Level01Wave05(this, wave05Enemies);
        miniboss2 = new Level01Miniboss2(this, miniboss02Enemies);
        levelEnd = new Level01LevelEnd(this);

        waves.Enqueue(wave01);
        waves.Enqueue(wave02);
        waves.Enqueue(wave03);
        waves.Enqueue(miniboss1);
        waves.Enqueue(wave04);
        waves.Enqueue(wave05);
        waves.Enqueue(miniboss2);
        waves.Enqueue(levelEnd);
    }

    void Start()
    {
        NextWave();
    }

    public void NextWave()
    {
        while (waves.Peek() != levelEnd && waves.Peek().GetNumberOfEnemies() == 0)
        {
            waves.Dequeue();
        }
        Debug.Log("Next wave called: " + waves.Peek());
        ChangeState(waves.Dequeue());
    }

    public void StartWaveAfterSeconds(float delay)
    {
        StartCoroutine(StartWaveAfterSecondsCoroutine(delay));
    }

    protected IEnumerator StartWaveAfterSecondsCoroutine(float delay)
    {
        // NOTE: THIS WILL BREAK IF WE ADD FUNCTIONALITY FOR ENDING THE WAVE WHEN ALL ARE DEAD
        yield return new WaitForSecondsRealtime(delay);
        NextWave();
    }

    public void SpawnEnemyAfterSeconds(Enemy enemy, float delay)
    {
        StartCoroutine(SpawnEnemyAfterSecondsCoroutine(enemy, delay));
    }

    protected IEnumerator SpawnEnemyAfterSecondsCoroutine(Enemy enemy, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        enemy.StartMoving();
        yield return null;
    }
}
