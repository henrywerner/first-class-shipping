using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject[] ConnectionNodes;

    [SerializeField] float _moveSpeed = .1f;
    public float MoveSpeed
    {
        get => _moveSpeed;
        set => _moveSpeed = value;
    }

    Vector3 _moveDirection;

    private void FixedUpdate()
    {
        Move();
        ProcessInputs();
    }

    private void ProcessInputs()
    {
        // calculate movement amount
        _moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetAxis("Fire1") >= 0)
        {
            FireGuns();
        }

        if (Input.GetAxis("Dodge") >= 0)
        {
            DodgeRoll();
        }

        if (Input.GetAxis("Debug") != 0)
        {
            Debug.Log("detaching all");
            foreach (var node in ConnectionNodes)
            {
                node.gameObject.GetComponent<RootAttachable>().Detach();
            }
        }

        // TEMP - noah
        if (Input.GetAxis("Jump") != 0)
        {
            Debug.Log("Detaching single gun");
            AbsAttachable throwingGun = GetRandAttachedTail();
            if (throwingGun != null)
            {
                throwingGun.DetachWithSpeed();
            }
        }
    }

    public void Move()
    {
        transform.Translate(_moveDirection.normalized * _moveSpeed);
    }

    public void FireGuns()
    {
        // TODO
        Debug.Log("Fire player guns");
    }

    private void DodgeRoll()
    {
        // Set invul
        // Show dodge visuals and play sfx
        // Start Coroutine to count frames/time till dodge roll end. Pass in frames
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

    private AbsAttachable GetRandAttachedTail()
    {
        List< AbsAttachable> roots = new List<AbsAttachable> ();
        foreach (GameObject node in ConnectionNodes)
        {
            if (node.GetComponent<RootAttachable>().NextAttachment != null)
            {
                roots.Add(node.GetComponent<RootAttachable>().FindTail());
            }
        }
        if (roots.Count > 0)
        {
            return roots[Random.Range(0, roots.Count)];
        }
        else
        {
            return null;
        }
    }
}
