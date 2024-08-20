using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Gun))]
public class EnemyMiniboss2 : Enemy
{
    [Header("Miniboss Specific")]
    [SerializeField] private BeamGun _beamGun;

    void Start()
    {

    }

    public override void StartMoving()
    {
        try
        {
            gameObject.GetComponent<EnemyHealth>().CanBeHurt = false;
            MoveToPosition1();
        }
        catch
        {
            Debug.LogWarning($"Coroutine failed to start in Enemy Miniboss.");
        }
    }

    public void MoveToPosition1()
    {
        StartCoroutine(MoveToPositionOneAndThenShootBeam());
    }

    public void MoveToPosition2()
    {
        StartCoroutine(MoveFromPositionOneToPositionTwoAndThenShootBeam());
    }

    public void MoveToFinalPosition()
    {
        StartCoroutine(MoveFromPositionTwoToCenterOfScreenAndThenSpamWaveShots());
    }

    IEnumerator ShootScatterAttack()
    {
        yield return StartCoroutine(_gun.ShootBurstCoroutine(2, 0.01f));
    }

    IEnumerator ShootBeamAttack()
    {
        yield return StartCoroutine(_beamGun.ShootThenWait(3));
    }

    IEnumerator MoveToPositionOneAndThenShootBeam()
    {
        yield return StartCoroutine(_pathMover.MoveAlongPath(_paths[0], _pathSpeedModifier, gameObject)); // Move into pos 1
        yield return StartCoroutine(_beamGun.WarningBeams());
        _beamGun.Shoot();
    }

    IEnumerator MoveFromPositionOneToPositionTwoAndThenShootBeam()
    {
        _beamGun.DeactivateAllBeams();
        yield return new WaitForSecondsRealtime(0.3f);

        Transform[] p = {_paths[1], _paths[2]};

        yield return StartCoroutine(_pathMover.MoveAlongAllPaths(p, _pathSpeedModifier, gameObject)); // Exit pos 1, then enter pos 2

        yield return StartCoroutine(_beamGun.WarningBeams());

        _beamGun.Shoot();
    }

    IEnumerator MoveFromPositionTwoToCenterOfScreenAndThenSpamWaveShots()
    {
        _beamGun.DeactivateAllBeams();
        yield return new WaitForSecondsRealtime(0.3f);
        
        gameObject.GetComponent<EnemyHealth>().CanBeHurt = true; // make the boss able to take damage

        Transform[] p = {_paths[3], _paths[4]};

        yield return StartCoroutine(_pathMover.MoveAlongAllPaths(p, _pathSpeedModifier, gameObject)); // Exit pos 2, then enter center screen

        while (true)
        {
            yield return StartCoroutine(ShootScatterAttack());
            yield return new WaitForSecondsRealtime(2f);
        }
    }
}
