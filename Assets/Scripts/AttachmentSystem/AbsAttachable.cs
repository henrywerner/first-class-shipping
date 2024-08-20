using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsAttachable : MonoBehaviour
{
    [SerializeField] Transform ConnectingNode;
    [SerializeField] Transform RecievingNode;
    public AbsAttachable NextAttachment = null;
    public AbsAttachable ParrentAttachment = null;
    [SerializeField] float detatchCooldown = 0.5f;
    [SerializeField] GameObject attachSFX;
    private bool _hasAttached = false;
    [SerializeField] GameObject attachVFX;
    [SerializeField] GameObject _pickupIndicator;
    private bool _isFlashing = false;

    public static event Action OnAttachmentsUpdate;

    public void AttachmentsUpdate()
    {
        OnAttachmentsUpdate?.Invoke();
    }

    private void FixedUpdate()
    {
        if (ParrentAttachment == null)
        {
            Vector3 cameraBounds = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
            float boundsX = cameraBounds.x - (transform.lossyScale.x * 0.5f);
            float boundsY = cameraBounds.y - (transform.lossyScale.y * 0.5f);
            if (this.transform.position.x < -boundsX - 1 || this.transform.position.x > boundsX + 1
                || this.transform.position.y < -boundsY - 1 || this.transform.position.y > boundsY + 1)
            {
                this.gameObject.SetActive(false);
            }
        }

        if (!_hasAttached && !_isFlashing && _pickupIndicator != null)
        {
            StartCoroutine(FlashingPickup());
        }
    }

    public void AttachTo(AbsAttachable argParentAttachment)
    {
        _hasAttached = true;
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
        AudioController.controller.PlaySFX(attachSFX, transform.position);
        if(attachVFX != null)
        {
            Instantiate(attachVFX, transform.position, transform.rotation);
        }

        AttachmentsUpdate();
    }

    public void DetachWithForce()
    {
        float random = UnityEngine.Random.Range(0f, 260f);
        DetachWithForce(new Vector2(Mathf.Cos(random), Mathf.Sin(random)) * 100);
    }

    public void DetachWithForce(float forceMagnitude)
    {
        float random = UnityEngine.Random.Range(0f, 260f);
        DetachWithForce(new Vector2(Mathf.Cos(random), Mathf.Sin(random)) * forceMagnitude);
    }

    public void DetachWithForce(Vector2 argForce)
    {
        if (ParrentAttachment != null) //already detatched
        {
            Detach();
            Rigidbody2D rb2d = this.gameObject.AddComponent<Rigidbody2D>();
            rb2d.isKinematic = false;
            rb2d.gravityScale = 0;

            rb2d.AddForce(argForce);
            rb2d.AddTorque(UnityEngine.Random.Range(-8, 8), ForceMode2D.Impulse);

            if (NextAttachment != null)
            {
                NextAttachment.DetachWithForce(argForce);
            }
        }
    }

    virtual protected void Detach()
    {
        this.transform.parent = null;
        ParrentAttachment.NextAttachment = null;
        ParrentAttachment = null;

        StartCoroutine(DisableCollionsForSeconds(detatchCooldown));

        AttachmentsUpdate();
    }

    virtual public void DetachAllConnected()
    {
        Detach();

        if (NextAttachment != null)
        {
            NextAttachment.DetachAllConnected();
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
        // Debug.Log("Collided with " + other.gameObject.name);
        if (other.gameObject.tag == "Player" && other.collider.gameObject.tag != "Friendly")
        {
            //Debug.Log("Collided with " + other.gameObject.name);
            other.gameObject.GetComponent<PlayerController>().AttachComponent(this);
        }

        if (other.gameObject.tag == "Enemy") {
            Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
            if (rb != null) {
                float x = rb.velocity.x;
                float y = rb.velocity.y;
                float slope = y / x;
                double dmg = Math.Sqrt(Math.Pow((double)x, 2) + Math.Pow((double)y, 2));
                dmg = Math.Abs(dmg);
                int dmgInt = Convert.ToInt32(dmg) * 3; // times 10 because idk
                Debug.Log("Thrown weapon doing " + dmgInt + " damage on hit");
                other.gameObject.GetComponent<IDamageable>()?.Damage(dmgInt);
            }
        }

        if (this.ParrentAttachment == null && (other.gameObject.tag == "Friendly" || other.gameObject.tag == "Enemy")) // if this is not attached, don't collide with bullets
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.collider.gameObject.GetComponent<Collider2D>());
            return;
        }

    }

    private IEnumerator FlashingPickup()
    {
        _isFlashing = true;
        _pickupIndicator.SetActive(true);
        yield return StartCoroutine(LerpColor(0, 0.4f, 15));
        yield return StartCoroutine(LerpColor(0.4f, 0, 15));
        _pickupIndicator.SetActive(false);
        // wait 30 frames
        for (int i = 0; i < 30; i++)
        {
            yield return new WaitForFixedUpdate();
        }
        
        _isFlashing = false;
    }

    private IEnumerator LerpColor(float alphaStart, float alphaEnd, int frames)
    {
        Color startColor = _pickupIndicator.GetComponent<SpriteRenderer>().material.color;
        startColor.a = alphaStart;
        _pickupIndicator.GetComponent<SpriteRenderer>().material.color = startColor;
        Color colorGoal = startColor;
        colorGoal.a = alphaEnd;

        int framesElapsed = 0;

        while (_pickupIndicator.GetComponent<SpriteRenderer>().material.color.a != alphaEnd && !_hasAttached)
        {
            Debug.Log("alpha value = " + _pickupIndicator.GetComponent<SpriteRenderer>().material.color.a);
            _pickupIndicator.GetComponent<SpriteRenderer>().material.color = Color.Lerp(startColor, colorGoal, (float)framesElapsed / frames);
            yield return new WaitForFixedUpdate();
            framesElapsed++;
        }
    }
}
