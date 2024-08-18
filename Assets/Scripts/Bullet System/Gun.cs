using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour, IShootable
{
    [SerializeField] protected GameObject _bullet;
    
    [SerializeField] protected GameObject[] _nodes;

    private bool _isOnCooldown = false;

    public virtual void Shoot() {
        foreach (GameObject node in _nodes)
        {
            GameObject bullet = Instantiate(_bullet, node.transform.position, node.transform.rotation);
        }
    }

    public virtual void ShootWithCooldown(float rate)
    {
        if (!_isOnCooldown)
        {
            StartCoroutine(ShootThenWait(1, rate));
            _isOnCooldown = true;
        }
    }

    public virtual void ShootMultipleTimes(int amountOfTimes, float rate) {
        StartCoroutine(ShootThenWait(amountOfTimes, rate));
    }

    public IEnumerator ShootThenWait(int amountOfTimes, float waitTime) {
        for (int i = 0; i < amountOfTimes; i++)
        {
            this.Shoot();
            yield return new WaitForSeconds(waitTime);
            _isOnCooldown = false;
        }
    }
}
