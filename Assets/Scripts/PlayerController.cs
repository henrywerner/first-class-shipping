using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject[] ConnectionNodes;

    [SerializeField] float _moveSpeed = .25f;
    public float MoveSpeed
    {
        get => _moveSpeed;
        set => _moveSpeed = value;
    }

    private void FixedUpdate()
    {
        Move();
        if (Input.GetAxis("Fire1") != 0)
        {
            FireGuns();
        }

        if (Input.GetAxis("Debug") != 0)
        {
            Debug.Log("detaching");
            foreach (var node in ConnectionNodes)
            {
                node.gameObject.GetComponent<RootAttachable>().Detach();
            }
        }
    }

    public void Move()
    {
        // calculate movement amount
        float verticalMoveThisFrame = Input.GetAxis("Vertical") * _moveSpeed;
        float horizontalMoveThisFrame = Input.GetAxis("Horizontal") * _moveSpeed;
        // move player
        this.transform.position = this.transform.position + (transform.up * verticalMoveThisFrame) + (transform.right * horizontalMoveThisFrame);
    }

    public void FireGuns()
    {
        // TODO
        Debug.Log("Fire player guns");
    }

    public void AttachComponent(AbsAttachable argComponent)
    {
        if (argComponent.ParrentAttachment == null)
        {
            AbsAttachable attachPoint = GetClosestRootNode(argComponent.gameObject).FindTail();
            argComponent.AttachTo(attachPoint);
            argComponent.transform.parent = this.transform;
        }
    }

    private RootAttachable GetClosestRootNode(GameObject Obj)
    {
        float minDistance = 9999;
        float distance = 0;
        GameObject nearest = null;
        foreach (GameObject node in ConnectionNodes)
        {
            distance = Vector3.Distance(node.transform.position, Obj.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = node;
            }
        }

        if (nearest != null)
        {
            return nearest.GetComponent<RootAttachable>();
        }
        else
        {
            Debug.LogWarning("Could not find nearest root.");
            return null;
        }
    }
}
