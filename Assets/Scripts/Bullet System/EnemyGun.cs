using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : Gun
{
    public override void Fire()
    {
        // Enemy gun should not be used by the player
        throw new System.NotImplementedException();
    }
}
