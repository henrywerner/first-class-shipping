using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : HealthSystem
{
    public override void Kill()
    {
        gameObject.GetComponent<PlayerController>()?.DetachAllGunsWithForce();
        base.Kill();
    }
}
