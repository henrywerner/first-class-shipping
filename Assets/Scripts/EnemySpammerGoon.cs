using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Gun))]
public class EnemySpammerGoon : Enemy
{
    public override void StartMoving()
    {
        try
        {
            StartCoroutine(CombatActions());
        }
        catch
        {
            Debug.LogWarning($"Coroutine failed to start in EnemySpammerGoon.");
        }
    }

    IEnumerator CombatActions() {
        yield return StartCoroutine(_pathMover.MoveAlongPath(_paths[0], _pathSpeedModifier, gameObject));

        while (true)
        {
            _gun.Shoot();
            yield return new WaitForSecondsRealtime(0.25f);
        }
    }
}
