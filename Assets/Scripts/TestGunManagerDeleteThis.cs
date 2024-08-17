using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGunManagerDeleteThis : MonoBehaviour
{
    
    [SerializeField] private PlayerTestGun _playerGun;

    [SerializeField] private EnemyGun _enemyGun;


    void Update()
    {
        if (Input.GetButtonDown("Fire1")) {
            _playerGun.Shoot();
        }

        if (Input.GetButtonDown("Fire2")) {
            _enemyGun.ShootMultipleTimes(5, 0.1f);
        }
    }
}
