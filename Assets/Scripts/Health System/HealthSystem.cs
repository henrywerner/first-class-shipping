using System;
using System.Collections;
using UnityEngine;

public abstract class HealthSystem : MonoBehaviour, IDamageable
{
    [SerializeField] int maxHp;
    [SerializeField] int currentHp;
    [SerializeField] float hurtBufferTime = 0f;
    public bool CanBeHurt { get => _canBeHurt; set => _canBeHurt = value; }
    private bool _canBeHurt;

    public void Awake()
    {
        currentHp = maxHp;
        _canBeHurt = true;
    }

    public virtual void Kill()
    {
        gameObject.SetActive(false);
    }

    public virtual void Damage(int damage)
    {
        if (_canBeHurt)
        {
            currentHp -= damage;

            if (currentHp <= 0)
            {
                Kill();
            }
            else if(hurtBufferTime > 0)
            {
                SetTempInvul(hurtBufferTime);
            }
        }
    }

    public void SetTempInvul(float invulSec)
    {
        StartCoroutine(InvunerableCoroutine(invulSec));
    }

    private IEnumerator InvunerableCoroutine(float invulSec)
    {
        _canBeHurt = false;
        yield return new WaitForSeconds(invulSec);
        _canBeHurt = true;
    }
}