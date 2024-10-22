using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : HealthSystem
{
    [SerializeField] bool _useHealthScaling = true;

    public override void Awake()
    {
        base.Awake();
        if (_useHealthScaling )
        {
            currentHp = ((FindObjectOfType<PlayerController>().GetGunCount() / 4) + 1) * maxHp;
        }
    }

    public override void Kill()
    {
        Enemy enemy = GetComponent<Enemy>();
        if (enemy != null)
        {
            EventManager.current.ShakeScreen(enemy.ScreenShakeDuration, enemy.ScreenShakeMagnitude);
        }

        EventManager.current.EnemyDispatched(); // Send enemy killed event
        this.gameObject.GetComponent<LootContainer>()?.DumpLoot(); // Drop held loot (if any)
        base.Kill();
    }
}
