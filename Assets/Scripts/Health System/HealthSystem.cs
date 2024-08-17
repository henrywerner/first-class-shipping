using System;
using UnityEngine;

public abstract class HealthSystem : MonoBehaviour, IDamageable
{
    public int maxHp;
    public int currentHp;

    public void Awake()
    {
        currentHp = maxHp;
    }

    public virtual void Kill()
    {
        gameObject.SetActive(false);
    }

    public virtual void Damage(int damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
            Kill();
    }
}