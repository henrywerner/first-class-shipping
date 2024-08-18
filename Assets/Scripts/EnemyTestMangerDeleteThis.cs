using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTestMangerDeleteThis : Enemy
{
    void Start()
    {
        _pathMover.StartMoving(_paths, _pathSpeedModifier, gameObject);
    }
}
