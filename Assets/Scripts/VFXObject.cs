using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class VFXObject : MonoBehaviour
{
    ParticleSystem partSys;

    void Start()
    {
        partSys = GetComponent<ParticleSystem>();
        Destroy(gameObject, partSys.main.startLifetime.constant);
    }

}
