using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsAttachable : MonoBehaviour
{
    [SerializeField] Transform ConnectingNode;
    [SerializeField] Transform RecievingNode;
    public AbsAttachable NextAttachment = null;
    public AbsAttachable ParrentAttachment = null;

    Rigidbody2D _rb2d = null;

    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    public void AttachTo(AbsAttachable argParentAttachment)
    {
        _rb2d.isKinematic = true;
        this.transform.rotation = argParentAttachment.transform.rotation;
        this.transform.position = argParentAttachment.RecievingNode.position + (this.transform.position - ConnectingNode.transform.position);
        argParentAttachment.NextAttachment = this;
        ParrentAttachment = argParentAttachment;
    }

    public void DetachWithSpeed()
    {
        Detach();
        _rb2d.isKinematic = false;
        float random = Random.Range(0f, 260f);
        _rb2d.AddForce(new Vector2(Mathf.Cos(random), Mathf.Sin(random)) * 100);
        _rb2d.AddTorque(Random.Range(2, 6), ForceMode2D.Impulse);

    }

    virtual public void Detach()
    {
        this.transform.parent = null;
        ParrentAttachment.NextAttachment = null;
        ParrentAttachment = null;

        StartCoroutine(DisableCollionsForSeconds(0.5f));

        if (NextAttachment != null)
        {
            NextAttachment.Detach();
        }
    }
    
    IEnumerator DisableCollionsForSeconds(float seconds)
    {
        BoxCollider2D collider = this.gameObject.GetComponent<BoxCollider2D>();
        collider.enabled = false;
        yield return new WaitForSeconds(seconds);
        collider.enabled = true;
    }

    public AbsAttachable FindTail()
    {
        if (NextAttachment == null)
            return this;
        else
            return NextAttachment.FindTail();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collided with " + other.gameObject.name);
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Collided with " + other.gameObject.name);
            other.gameObject.GetComponent<PlayerController>().AttachComponent(this);
        }
    }
}
