using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicGun : Gun
{
    [SerializeField] float CooldownLength = 0.5f; // in seconds
    public override void Fire()
    {
        ShootWithCooldown(CooldownLength);
    }
}
