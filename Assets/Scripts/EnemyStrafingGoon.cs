using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Gun))]
public class EnemyStrafingGoon : Enemy
{
    private IEnumerator flightCoroutine;
    void Start()
    {
        flightCoroutine = _pathMover.MoveAlongPath(_paths[0], _pathSpeedModifier, gameObject);
    }

    public override void StartMoving()
    {
        try
        {
            StartCoroutine(CombatActions());
        }
        catch
        {
            Debug.LogWarning($"Coroutine failed to start on {gameObject.name}");
        }
    }

    IEnumerator ShootGuns()
    {
        // Shoot 5 bursts of 3 shots.
        yield return StartCoroutine(_gun.ShootBurstCoroutine(3, 0.2f));
        yield return new WaitForSecondsRealtime(1f);
        yield return StartCoroutine(_gun.ShootBurstCoroutine(3, 0.2f));
        yield return new WaitForSecondsRealtime(1f);
        yield return StartCoroutine(_gun.ShootBurstCoroutine(3, 0.2f));
        yield return new WaitForSecondsRealtime(1f);
        yield return StartCoroutine(_gun.ShootBurstCoroutine(3, 0.2f));
        yield return new WaitForSecondsRealtime(1f);
        yield return StartCoroutine(_gun.ShootBurstCoroutine(3, 0.2f));
        yield return new WaitForSecondsRealtime(1f);
    }

    IEnumerator CombatActions() {
        StartCoroutine(ShootGuns());

        yield return StartCoroutine(flightCoroutine);

        this.Remove();

        Destroy(_parentObject != null ? _parentObject : gameObject);
    }
}
