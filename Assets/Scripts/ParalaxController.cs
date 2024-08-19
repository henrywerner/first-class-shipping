using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxController : MonoBehaviour
{
    [SerializeField] float yRange = 1;
    [SerializeField] Transform endPoint;
    [SerializeField] Transform startPoint;
    [SerializeField] Transform[] paralaxObjects;

    private void Update()
    {
        foreach(var obj in paralaxObjects)
        {
            if(obj.position.x < endPoint.position.x)
            {
                obj.position = new Vector3(startPoint.position.x, obj.position.y + Random.Range(-yRange, yRange));
            }
        }
    }
}
