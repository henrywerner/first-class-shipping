using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeamGun : BeamGun
{
    public override void Fire()
    {
        // Enemy gun should not be used by the player
        throw new System.NotImplementedException();
    }

    private void OnDrawGizmos() {
        foreach (GameObject node in _nodes)
        {
            Gizmos.color = new Color(1, 0.1f, 0.1f, 0.2f);
            Vector3 newNodePos = node.transform.position;
            newNodePos += node.transform.up * 20;
            Gizmos.DrawLine(node.transform.position, newNodePos);
        }
    }
}
