using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsAttachable : MonoBehaviour
{
    [SerializeField] Transform ConnectingNode;
    [SerializeField] Transform RecievingNode;
    public AbsAttachable NextAttachment = null;

    public void AttachTo(AbsAttachable parentAttachment)
    {
        this.transform.rotation = parentAttachment.transform.rotation;
        this.transform.position = parentAttachment.RecievingNode.position + (this.transform.position - ConnectingNode.transform.position);
        parentAttachment.NextAttachment = this;
        this.NextAttachment = null;
    }

    virtual public void Detach()
    {
        // something about this isn't working
        // TODO
        this.transform.parent = null;
        if (NextAttachment != null)
        {
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
