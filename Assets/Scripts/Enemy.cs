using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Pathing")]
    [SerializeField] protected Transform[] _paths;
    [SerializeField] protected float _pathSpeedModifier = 0.5f;

    [Header("Gun")]
    [SerializeField] protected Gun _gun;

    [Header("Parent")]
    [SerializeField] protected GameObject _parentObject;


    protected BezierMoveAlongPath _pathMover;

    protected void Awake() {
        _pathMover = FindFirstObjectByType<BezierMoveAlongPath>();
    }

    public virtual void StartMoving() {
        _pathMover.StartMoving(_paths, _pathSpeedModifier, gameObject);
    }

    // Remove without killing, as killing drops loot
    protected virtual void Remove()
    {
        gameObject.SetActive(false);
    }
    
}
