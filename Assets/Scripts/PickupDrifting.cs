using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupDrifting : MonoBehaviour
{
    [SerializeField] float DriftVelocity = 0.5f;

    private void FixedUpdate()
    {
        this.transform.position = transform.position - (transform.right * DriftVelocity);
    }
}
