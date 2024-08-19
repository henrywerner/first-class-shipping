using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour, IShootable
{
    [SerializeField] protected GameObject _bullet;
    
    [SerializeField] protected GameObject[] _nodes;
    [SerializeField] protected GameObject shootSFXObj;

    protected bool _isOnCooldown = false;

    // Fire should be used by the player to actually fire the weapons
    public abstract void Fire();

    public virtual void Shoot() {
        foreach (GameObject node in _nodes)
        {
            GameObject bullet = Instantiate(_bullet, node.transform.position, node.transform.rotation);
        }
        AudioController.controller.PlaySFX(shootSFXObj, transform.position);
    }

    public virtual void ShootWithCooldown(float rate)
    {
        if (!_isOnCooldown)
        {
            StartCoroutine(ShootBurstCoroutine(1, rate));
            _isOnCooldown = true;
        }
    }

    public virtual void ShootBurst(int amountOfTimes, float rate) {
        StartCoroutine(ShootBurstCoroutine(amountOfTimes, rate));
    }

    public IEnumerator ShootBurstCoroutine(int amountOfTimes, float waitTime) {
        for (int i = 0; i < amountOfTimes; i++)
        {
            this.Shoot();
            yield return new WaitForSeconds(waitTime);
            _isOnCooldown = false;
        }
    }
}
