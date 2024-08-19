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

    [Header("VFX")]
    [SerializeField] GameObject _hitSprite;
    [SerializeField] float flashSeconds = 0.1f;
    [SerializeField] GameObject _destroyVFX;

    public void Awake()
    {
        currentHp = maxHp;
        _canBeHurt = true;
    }

    public virtual void Kill()
    {
        _canBeHurt = false;
        AudioController.controller.PlaySFX(_deathSFX, transform.position);

        if (_destroyVFX != null)
        {
            Instantiate(_destroyVFX, transform.position, transform.rotation);
        }

        var destoryObj = transform.parent != null && transform.parent.gameObject.tag == "Enemy" ? transform.parent.gameObject : gameObject;
        Destroy(destoryObj);
    }

    public virtual void Damage(int damage)
    {
        if (_canBeHurt)
        {
            currentHp -= damage;
            AudioController.controller.PlaySFX(_hitSFX, transform.position);
            FlashHitEffect();

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

    public void FlashHitEffect()
    {
        if (_hitSprite != null)
        {
            try
            {
                StartCoroutine(FlashAndWaitCoroutine());
            }
            catch
            {
                //We are just going to ignore in case the object is killed (disabled)
                Debug.LogWarning($"Flash Effect Coroutine failed to start on {gameObject.name}");
            }
        }
    }

    IEnumerator FlashAndWaitCoroutine()
    {
        _hitSprite.SetActive(true);
        yield return new WaitForSeconds(flashSeconds);
        _hitSprite.SetActive(false);
    }
}