using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AbsAttachable))]
public class AttachmentDamageable : MonoBehaviour, IDamageable
{
    [SerializeField] GameObject hitSFX;
    [SerializeField] float DetachForce = 100f;
    bool _canBeHurt = true;

    public void Damage(int damage)
    {
        if (_canBeHurt)
        {
            AudioController.controller.PlaySFX(hitSFX, transform.position);
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
}
