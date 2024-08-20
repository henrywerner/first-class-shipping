using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Gun))]
public class EnemyEliteGoon : Enemy
{
    [SerializeField] private GameObject _gunNode;

    private IEnumerator flightEnterCoroutine, flightLeaveCoroutine;
    private PlayerController player;
    void Start()
    {
        flightEnterCoroutine = _pathMover.MoveAlongPath(_paths[0], _pathSpeedModifier, gameObject);
    }

    private void FixedUpdate()
    {
        if (player != null)
        {
            Vector3 direction = player.transform.position - _gunNode.transform.position;
            _gunNode.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
        }
        else
        {
            player = FindObjectOfType<PlayerController>();
        }
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
        yield return StartCoroutine(_gun.ShootBurstCoroutine(5, 0.2f));
    }

    IEnumerator CombatActions() {
        yield return StartCoroutine(flightEnterCoroutine);

        yield return StartCoroutine(ShootGuns());

        yield return new WaitForSecondsRealtime(1f);

        yield return StartCoroutine(ShootGuns());

        yield return new WaitForSecondsRealtime(1f);

        yield return StartCoroutine(ShootGuns());

        yield return new WaitForSecondsRealtime(1f);

        if (_paths.Length > 1) 
        {
            yield return StartCoroutine(_pathMover.MoveAlongPath(_paths[1], _pathSpeedModifier, gameObject));

            this.Remove();

            Destroy(_parentObject != null ? _parentObject : gameObject);
        } 
        else 
        {
            while (true)
            {
                yield return StartCoroutine(ShootGuns());

                yield return new WaitForSecondsRealtime(1f);
            }    
        }
    }
}
