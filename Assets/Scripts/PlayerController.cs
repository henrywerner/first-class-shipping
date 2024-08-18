using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject[] ConnectionNodes;
    private List<PlayerBasicGun> _gunList = new List<PlayerBasicGun>();

    [SerializeField] float _moveSpeed = .1f;
    public float MoveSpeed
    {
        get => _moveSpeed;
        set => _moveSpeed = value;
    }

    // Dodge
    [SerializeField] float _dodgeInvulTime = .2f;
    [SerializeField] float _dodgeBoostTime = .4f;
    [SerializeField] float _dodgeCoolDown = 1f;
    [SerializeField] float _dodgeSpeed = .5f;
    [SerializeField] GameObject _dodgeSprite;

    Vector3 _moveDirection;
    Vector3 _dodgeDirection;
    HealthSystem _healthSystem;
    float _lastDodgeTime;
    bool _isDodging;

    private void Awake()
    {
        _moveDirection = Vector3.zero;
        _healthSystem = GetComponent<HealthSystem>();
        _isDodging = false;
    }

    private void FixedUpdate()
    {
        ProcessInputs();
        Move();
    }

    private void ProcessInputs()
    {
        // calculate movement amount
        _moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetAxis("Fire1") > 0)
        {
            FireGuns();
        }

        if (Input.GetAxis("Dodge") > 0)
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

            UpdateGunList();
        }
    }

    public void Move()
    {
        if (_isDodging)
        {
            transform.Translate(_dodgeDirection.normalized * _dodgeSpeed);
        }
        else // default to regular movement
        {
            transform.Translate(_moveDirection.normalized * _moveSpeed);
        }
    }

    public void FireGuns()
    {
        // TODO
        Debug.Log("Fire player guns");
        foreach (PlayerBasicGun gun in _gunList)
        {
            gun.ShootWithCooldown(0.2f);
        }
    }

    private void UpdateGunList()
    {
        _gunList.Clear();

        foreach (GameObject node in ConnectionNodes)
        {
            AbsAttachable currentNode = node.GetComponent<RootAttachable>().NextAttachment;
            while (currentNode != null) 
            {
                if (currentNode.gameObject.GetComponent<GunAttachable>() != null)
                {
                    _gunList.Add(currentNode.GetComponent<PlayerBasicGun>());
                }
                currentNode = currentNode.NextAttachment;
            }
        }
    }

    public void AttachComponent(AbsAttachable argComponent)
    {
        if (argComponent.ParrentAttachment == null)
        {
            AbsAttachable attachPoint = GetClosestRootNode(argComponent.gameObject).FindTail();
            argComponent.AttachTo(attachPoint);
            argComponent.transform.parent = this.transform;
        }

        UpdateGunList();
    }

    private void DodgeRoll()
    {
        // Check not on cool down
        if (!_isDodging && Time.time >= _lastDodgeTime + _dodgeCoolDown)
        {
            // Require a part to use dodge
            if (DetatchSingle()) //Later might need to be more or none.
            {
                Debug.Log("Dodge this!");
                // Set invul
                _healthSystem.SetTempInvul(_dodgeInvulTime);
                // Show dodge visuals and play sfx
                _dodgeSprite?.SetActive(true);
                // Start Coroutine to count frames/time till dodge roll end. Pass in frames
                StartCoroutine(DodgeSpeedCoroutine(_dodgeBoostTime));
            }
        }
    }

    private IEnumerator DodgeSpeedCoroutine(float seconds)
    {
        _isDodging = true;
        _dodgeDirection = _moveDirection;
        yield return new WaitForSeconds(seconds);
        _lastDodgeTime = Time.time;
        _isDodging = false;
        _dodgeSprite?.SetActive(false);
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

    private bool DetatchSingle()
    {
        Debug.Log("Detaching single gun");
        bool detachedSuccess = false;
        AbsAttachable throwingGun = GetRandAttachedTail();
        if (throwingGun != null)
        {
            throwingGun.DetachWithSpeed();
            detachedSuccess = true;
        }

        UpdateGunList();

        return detachedSuccess;
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
