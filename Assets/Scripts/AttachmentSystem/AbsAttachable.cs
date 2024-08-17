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
        this.transform.rotation = argParentAttachment.transform.rotation;
        this.transform.position = argParentAttachment.RecievingNode.position + (this.transform.position - ConnectingNode.transform.position);
        argParentAttachment.NextAttachment = this;
        ParrentAttachment = argParentAttachment;
    }

    virtual public void Detach()
    {
        // something about this isn't working
        // TODO
        this.transform.parent = null;
        ParrentAttachment = null;

        BoxCollider2D collider = this.gameObject.GetComponent<BoxCollider2D>();
        collider.enabled = false;
        StartCoroutine(ReEnableCollions(collider, 0.5f));
        if (NextAttachment != null)
        {
            NextAttachment.Detach();
            NextAttachment = null;
        }
    }
    
    IEnumerator ReEnableCollions(BoxCollider2D argCollider, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        argCollider.enabled = true;
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
