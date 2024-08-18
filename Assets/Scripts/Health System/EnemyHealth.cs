using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : HealthSystem
{
    public override void Kill()
    {
        Enemy enemy = GetComponent<Enemy>();
        if (enemy != null)
        {
            EventManager.current.ShakeScreen(enemy.ScreenShakeDuration, enemy.ScreenShakeMagnitude);
        }

        EventManager.current.EnemyKilled(); // Send enemy killed event
        this.gameObject.GetComponent<LootContainer>()?.DumpLoot(); // Drop held loot (if any)
        base.Kill();
    }
}
