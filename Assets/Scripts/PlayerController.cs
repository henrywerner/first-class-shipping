using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class PlayerController : MonoBehaviour
{
    

    [SerializeField] GameObject[] ConnectionNodes;
    private Gun _starterGun;
    private List<Gun> _gunList = new List<Gun>();

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
    [SerializeField] float _dodgeDetachSpeed = 100f;
    [SerializeField] GameObject _dodgeSprite;
    [SerializeField] GameObject _dodgeSFX;

    Vector3 _moveDirection;
    Vector3 _dodgeDirection;
    HealthSystem _healthSystem;
    float _lastDodgeTime;
    bool _isDodging;

    private void Awake()
    {
        _starterGun = this.gameObject.GetComponentInChildren<Gun>();
        _gunList.Add(_starterGun);
        AbsAttachable.OnAttachmentsUpdate += UpdateGunList;

        _moveDirection = Vector3.zero;
        _healthSystem = GetComponent<HealthSystem>();
        _isDodging = false;
    }

    private void OnDisable()
    {
        AbsAttachable.OnAttachmentsUpdate -= UpdateGunList;
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
            DetachAllGunsWithForce();
            /*
            foreach (var node in ConnectionNodes)
            {
                node.gameObject.GetComponent<RootAttachable>().DetachAllConnected();
            }
            */
        }
    }

    public void Move()
    {
        if (_isDodging)
        {
            MoveInBounds(_dodgeDirection.normalized * _dodgeSpeed);
        }
        else // default to regular movement
        {
            MoveInBounds(_moveDirection.normalized * _moveSpeed);
        }
    }

    private void MoveInBounds(Vector3 desiredMovementVector)
    {
        Vector3 cameraBounds = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        float boundsX = cameraBounds.x - (transform.lossyScale.x * 0.5f);
        float boundsY = cameraBounds.y - (transform.lossyScale.y * 0.5f);

        Vector3 clampedNewPosition = Vector3.zero;
        clampedNewPosition.x = Mathf.Clamp(desiredMovementVector.x + transform.position.x, -boundsX, boundsX);
        clampedNewPosition.y = Mathf.Clamp(desiredMovementVector.y + transform.position.y, -boundsY, boundsY);

        transform.position = clampedNewPosition;
    }

    public void FireGuns()
    {
        foreach (Gun gun in _gunList)
        {
            gun.Fire();
        }
    }

    private void UpdateGunList()
    {
        _gunList.Clear();
        _gunList.Add(_starterGun);

        foreach (GameObject node in ConnectionNodes)
        {
            AbsAttachable currentNode = node.GetComponent<RootAttachable>().NextAttachment;
            while (currentNode != null) 
            {
                if (currentNode.gameObject.GetComponent<GunAttachable>() != null)
                {
                    _gunList.Add(currentNode.GetComponent<Gun>());
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
    }

    public void DetachAllGunsWithForce()
    {
        foreach(GameObject node in ConnectionNodes)
        {
            AbsAttachable currentNode = node.GetComponent<RootAttachable>().NextAttachment;
            if (currentNode != null)
            {
                currentNode.DetachWithForce(400f);
            }
        }
    }

    private void DodgeRoll()
    {
        // Check not on cool down
        if (!_isDodging && Time.time >= _lastDodgeTime + _dodgeCoolDown)
        {
            // Require a part to use dodge
            Vector3 detachDirection = _moveDirection != Vector3.zero ? -_moveDirection : Vector3.left;
            //if (DetatchSingle(detachDirection, _dodgeDetachSpeed)) //Turn this back on if you want to require a gun to dodge.
            //{
            DetatchSingle(detachDirection, _dodgeDetachSpeed);
            Debug.Log("Dodge this!");
            // Set invul
            _healthSystem.SetTempInvul(_dodgeInvulTime);
            foreach (var gun in _gunList)
            {
                gun.GetComponent<AttachmentDamageable>()?.SetTempInvul(_dodgeInvulTime);
            }
            // Show dodge visuals and play sfx
            _dodgeSprite?.SetActive(true);
            AudioController.controller.PlaySFX(_dodgeSFX, transform.position);
            // Start Coroutine to count frames/time till dodge roll end. Pass in frames
            StartCoroutine(DodgeSpeedCoroutine(_dodgeBoostTime));
            // }
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

    //TODO make more abstract version with several gun detaches
    private bool DetatchSingle(Vector2 detatchDirection, float detachSpeed)
    {
        Debug.Log("Detaching single gun");
        bool detachedSuccess = false;
        AbsAttachable throwingGun = GetRandAttachedTail();
        if (throwingGun != null)
        {
            Vector2 detachForce = detatchDirection * detachSpeed;
            throwingGun.DetachWithForce(detachForce);
            detachedSuccess = true;
        }

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

    public int GetGunCount()
    {
        return _gunList.Count;
    }
}
