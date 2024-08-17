using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestGun : Gun
{
    public override void Shoot() {
        foreach (GameObject node in _nodes)
        {
            GameObject bulletObj = Instantiate(_bullet, node.transform.position, node.transform.rotation);
            Bullet bulletScript = bulletObj.GetComponent<Bullet>();
            bulletScript.isEnemy = false;
        }
    }
}
