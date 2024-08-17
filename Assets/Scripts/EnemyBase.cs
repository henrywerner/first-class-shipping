using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected Transform[] _paths;
    [SerializeField] protected float _pathSpeedModifier = 0.5f;

    protected BezierMoveAlongPath _pathMover;

    protected void Awake() {
        _pathMover = FindFirstObjectByType<BezierMoveAlongPath>();
    }
    
}
