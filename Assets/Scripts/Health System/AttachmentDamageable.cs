using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AbsAttachable))]
public class AttachmentDamageable : MonoBehaviour, IDamageable
{
    public void Damage(int damage)
    {
        Kill();
    }

    public void Kill()
    {
        GetComponent<AbsAttachable>().FindTail().DetachWithSpeed();
    }
}
