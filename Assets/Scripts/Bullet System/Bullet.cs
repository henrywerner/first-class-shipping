using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{   
    [Header("Stats")]
    [SerializeField] public float MoveSpeed = 0.3f;
    [SerializeField] private int _damage = 1;
    [SerializeField] public bool isEnemy = true;
    [SerializeField] float _despawnTime = 3f;

    float _startTime;

    private void Awake()
    {
        Physics2D.IgnoreLayerCollision(6, 6); // Ignore collisions with other bullets  
        _startTime = Time.time;
    }

    private void FixedUpdate()
    {
        Movement();
        CheckShouldDespawn();
    }

    protected virtual void Movement()
    {
        Vector3 moveOffset = transform.right * MoveSpeed;
        transform.position = transform.position + moveOffset;
    }

    private void CheckShouldDespawn()
    {
        if(Time.time - _startTime >= _despawnTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        IDamageable obj = other.gameObject.GetComponent<IDamageable>();

        if (obj == null) { // if obj isn't damageable
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.gameObject.GetComponent<Collider2D>());
            return;
        }
        else if (other.gameObject.CompareTag("Enemy") && isEnemy) {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.gameObject.GetComponent<Collider2D>());
            return;
        }
        else if (other.gameObject.CompareTag("Friendly") && !isEnemy) {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.gameObject.GetComponent<Collider2D>());
            return;
        }
        else if ((other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Gun")) && !isEnemy)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.gameObject.GetComponent<Collider2D>());
            return;
        }

        obj?.Damage(_damage); // all enemy bullets always 
        Feedback(); // spawn particles and sfx
        gameObject.SetActive(false);
    }

    private void Feedback()
    {
        // TODO: add hit FX here.
        
        Destroy(gameObject);
    }

}
