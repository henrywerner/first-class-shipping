using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour, IShootable
{
    [SerializeField] protected GameObject _bullet;
    
    [SerializeField] protected GameObject[] _nodes;

    public virtual void Shoot() {
        foreach (GameObject node in _nodes)
        {
            GameObject bullet = Instantiate(_bullet, node.transform.position, node.transform.rotation);
        }
    }

    public virtual void ShootMultipleTimes(int amountOfTimes, float rate) {
        StartCoroutine(ShootThenWait(amountOfTimes, rate));
    }

    IEnumerator ShootThenWait(int amountOfTimes, float waitTime) {
        for (int i = 0; i < amountOfTimes; i++)
        {
            this.Shoot();
            yield return new WaitForSecondsRealtime(waitTime);
        }
    }
}
