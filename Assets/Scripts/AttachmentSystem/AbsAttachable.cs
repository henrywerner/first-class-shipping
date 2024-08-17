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
        if (NextAttachment != null)
        {
            NextAttachment = null;
            NextAttachment.Detach();
        }
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
