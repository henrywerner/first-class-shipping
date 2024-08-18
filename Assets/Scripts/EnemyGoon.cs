using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Gun))]
public class EnemyGoon : Enemy
{
    private IEnumerator flightEnterCoroutine, flightLeaveCoroutine;
    void Start()
    {
        flightEnterCoroutine = _pathMover.MoveAlongPath(_paths[0], _pathSpeedModifier, gameObject);
        flightLeaveCoroutine = _pathMover.MoveAlongPath(_paths[1], _pathSpeedModifier, gameObject);
    }

    public override void StartMoving() {
        StartCoroutine(CombatActions());
    }

    IEnumerator ShootGuns() {
        yield return StartCoroutine(_gun.ShootThenWait(3, 0.2f));
    }

    IEnumerator CombatActions() {
        yield return StartCoroutine(flightEnterCoroutine);

        yield return StartCoroutine(ShootGuns());

        yield return new WaitForSecondsRealtime(1f);

        yield return StartCoroutine(ShootGuns());

        yield return new WaitForSecondsRealtime(1f);

        yield return StartCoroutine(ShootGuns());

        yield return new WaitForSecondsRealtime(1f);

        yield return StartCoroutine(flightLeaveCoroutine);

        this.Remove();

        Destroy(_parentObject != null ? _parentObject : gameObject);
    }
}
