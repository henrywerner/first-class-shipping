using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsAttachable : MonoBehaviour
{
    [SerializeField] Transform ConnectingNode;
    [SerializeField] Transform RecievingNode;
    public AbsAttachable NextAttachment = null;
    public AbsAttachable ParrentAttachment = null;

    public void AttachTo(AbsAttachable argParentAttachment)
    {
        PickupDrifting drift = this.GetComponent<PickupDrifting>();
        if (drift != null)
        {
            Destroy(drift);
        }
        Rigidbody2D rb2d = this.gameObject.GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            Destroy(rb2d);
        }
        this.transform.rotation = argParentAttachment.transform.rotation;
        this.transform.position = argParentAttachment.RecievingNode.position + (this.transform.position - ConnectingNode.transform.position);
        argParentAttachment.NextAttachment = this;
        ParrentAttachment = argParentAttachment;
    }

    public void DetachWithSpeed()
    {
        float random = Random.Range(0f, 260f);
        DetachWithSpeed(new Vector2(Mathf.Cos(random), Mathf.Sin(random)) * 100);
    }

    public void DetachWithSpeed(Vector2 argForce)
    {
        Detach();
        this.gameObject.AddComponent<Rigidbody2D>();
        Rigidbody2D rb2d = this.gameObject.GetComponent<Rigidbody2D>();
        rb2d.isKinematic = false;
        rb2d.gravityScale = 0;
        float random = Random.Range(0f, 260f);
        rb2d.AddForce(argForce);
        rb2d.AddTorque(Random.Range(4, 8), ForceMode2D.Impulse);
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

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("TEST");
    }
}
