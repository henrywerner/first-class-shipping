using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class BezierGizmoDisplay : MonoBehaviour
{
    // Code borrowed from https://www.youtube.com/watch?v=11ofnLOE8pw
    [SerializeField] private Transform[] controlPoints;

    private UnityEngine.Vector2 gizmosPos;

    private void OnDrawGizmos() {
        if (controlPoints == null) { return; }

        if (controlPoints.Length < 4) { return; }

        for (float t = 0; t <= 1; t += 0.05f) {
            gizmosPos = Mathf.Pow(1 - t, 3) * controlPoints[0].position +
            3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1].position +
            3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2].position + 
            Mathf.Pow(t, 3) * controlPoints[3].position;

            Gizmos.color = new Color(1f, 1f, 1f, 0.4f);
            Gizmos.DrawSphere(gizmosPos, 0.15f);
        }

        Gizmos.DrawLine(new UnityEngine.Vector2(controlPoints[0].position.x, controlPoints[0].position.y),
            new UnityEngine.Vector2(controlPoints[1].position.x, controlPoints[1].position.y));

        Gizmos.DrawLine(new UnityEngine.Vector2(controlPoints[2].position.x, controlPoints[2].position.y),
            new UnityEngine.Vector2(controlPoints[3].position.x, controlPoints[3].position.y));
    }
}
