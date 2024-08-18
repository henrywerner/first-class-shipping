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

    [Header("SFX")]
    [SerializeField] GameObject _hitSFX;
    [SerializeField] GameObject _deathSFX;

    public void Awake()
    {
        currentHp = maxHp;
        _canBeHurt = true;
    }

    public virtual void Kill()
    {
        AudioController.controller.PlaySFX(_deathSFX, transform.position);
        gameObject.SetActive(false);
    }

    public virtual void Damage(int damage)
    {
        if (_canBeHurt)
        {
            currentHp -= damage;
            AudioController.controller.PlaySFX(_hitSFX, transform.position);

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