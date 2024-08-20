using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AbsAttachable))]
public class AttachmentDamageable : MonoBehaviour, IDamageable
{
    [SerializeField] GameObject hitSFX;
    [SerializeField] float DetachForce = 100f;
    bool _canBeHurt = true;
    [SerializeField] GameObject _damageOverlay;
    [SerializeField] GameObject _damageVFX;

    public void Damage(int damage)
    {
        if (_canBeHurt)
        {
            AudioController.controller.PlaySFX(hitSFX, transform.position);
            if (_damageVFX != null)
            {
                Instantiate(_damageVFX, transform.position, transform.rotation);
            }
            if (_damageOverlay != null)
            {
                StartCoroutine(DamageIndicator());
            }
            Kill();
        }
    }

    public void Kill()
    {
        GetComponent<AbsAttachable>().FindTail().DetachWithForce(DetachForce);
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

    private IEnumerator DamageIndicator()
    {
        _damageOverlay.SetActive(true);
        yield return StartCoroutine(LerpColor(0, 0.5f, 10));
        yield return StartCoroutine(LerpColor(0.5f, 0, 20));
        _damageOverlay.SetActive(false);
    }

    private IEnumerator LerpColor(float alphaStart, float alphaEnd, int frames)
    {
        Color startColor = _damageOverlay.GetComponent<SpriteRenderer>().material.color;
        startColor.a = alphaStart;
        _damageOverlay.GetComponent<SpriteRenderer>().material.color = startColor;
        Color colorGoal = startColor;
        colorGoal.a = alphaEnd;

        int framesElapsed = 0;

        while (_damageOverlay.GetComponent<SpriteRenderer>().material.color.a != alphaEnd)
        {
            Debug.Log("alpha value = " + _damageOverlay.GetComponent<SpriteRenderer>().material.color.a);
            _damageOverlay.GetComponent<SpriteRenderer>().material.color = Color.Lerp(startColor, colorGoal, (float) framesElapsed / frames);
            yield return new WaitForFixedUpdate();
            framesElapsed++;
        }
    }
}
