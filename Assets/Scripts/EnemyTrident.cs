using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Gun))]
public class EnemyTrident : Enemy
{
    private IEnumerator flightEnterCoroutine, flightLeaveCoroutine;

    public override void StartMoving() {
        StartCoroutine(CombatActions());
    }

    IEnumerator ShootGuns() {
        yield return StartCoroutine(_gun.ShootBurstCoroutine(5, 0.2f));
        yield return new WaitForSecondsRealtime(0.6f);
        yield return StartCoroutine(_gun.ShootBurstCoroutine(5, 0.2f));
        yield return new WaitForSecondsRealtime(0.6f);
        yield return StartCoroutine(_gun.ShootBurstCoroutine(5, 0.2f));
        yield return new WaitForSecondsRealtime(0.6f);
    }

    IEnumerator CombatActions() {

        yield return StartCoroutine(_pathMover.MoveAlongPath(_paths[0], _pathSpeedModifier, gameObject));

        yield return StartCoroutine(ShootGuns());

        if (_paths.Length > 1) {
            yield return StartCoroutine(_pathMover.MoveAlongPath(_paths[1], _pathSpeedModifier, gameObject));

            this.Remove();

            Destroy(_parentObject != null ? _parentObject : gameObject);
        }
        else 
        {
            while (true)
            {
                yield return StartCoroutine(ShootGuns());
            }
        }
    }
}
