using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTestMangerDeleteThis : MonoBehaviour
{
    [SerializeField] private BezierMoveAlongPath movementScript;

    void Start()
    {
        movementScript.StartMoving();
    }
}
