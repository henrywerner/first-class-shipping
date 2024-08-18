using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AbsAttachable))]
public class AttachmentDamageable : MonoBehaviour, IDamageable
{
    [SerializeField] GameObject hitSFX;

    public void Damage(int damage)
    {
        AudioController.controller.PlaySFX(hitSFX, transform.position);
        Kill();
    }

    public void Kill()
    {
        GetComponent<AbsAttachable>().FindTail().DetachWithSpeed();
    }
}
