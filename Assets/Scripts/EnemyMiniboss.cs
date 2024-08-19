using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Gun))]
public class EnemyMiniboss : Enemy
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
            StartCoroutine(CombatActions());
        }
        catch
        {
            Debug.LogWarning($"Coroutine failed to start in Enemy Miniboss.");
        }
    }

    IEnumerator ShootScatterAttack()
    {
        yield return StartCoroutine(_gun.ShootBurstCoroutine(2, 0.01f));
    }

    IEnumerator ShootBeamAttack()
    {
        yield return StartCoroutine(_beamGun.ShootThenWait(3));
    }

    IEnumerator FireAGun() 
    {
        float x = UnityEngine.Random.Range(1f, 4f);

        if (x <= 2.0f) {
            // Beam blast
            yield return StartCoroutine(ShootBeamAttack());
        } else {
            // Scatter attack
            yield return StartCoroutine(ShootScatterAttack());
        }
        
    } 

    IEnumerator CombatActions() {

        yield return StartCoroutine(_pathMover.MoveAlongPath(_paths[0], _pathSpeedModifier, gameObject)); // Move into starting pos

        yield return new WaitForSecondsRealtime(1f);

        yield return StartCoroutine(FireAGun());

        yield return StartCoroutine(_pathMover.MoveAlongPath(_paths[1], _pathSpeedModifier, gameObject)); // Move to top of screen
        
        while (true)
        {
            yield return StartCoroutine(FireAGun());

            yield return new WaitForSecondsRealtime(1f);

            yield return StartCoroutine(_pathMover.MoveAlongPath(_paths[2], _pathSpeedModifier, gameObject)); // Move to bottom of screen

            yield return StartCoroutine(FireAGun());

            yield return new WaitForSecondsRealtime(1f);

            yield return StartCoroutine(_pathMover.MoveAlongPath(_paths[3], _pathSpeedModifier, gameObject)); // Move to top of screen
        }
    }
}
