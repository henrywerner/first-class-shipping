using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : HealthSystem
{
    public override void Kill()
    {
        EventManager.current.EnemyKilled(); // Send enemy killed event
        base.Kill();
    }
}
