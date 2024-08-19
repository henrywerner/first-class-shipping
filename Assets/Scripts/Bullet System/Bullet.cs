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

    [Header("VFX")]
    [SerializeField] GameObject _hitFX;
    
    private float boundsX, boundsY;

    float _startTime;

    private void Awake()
    {
        Physics2D.IgnoreLayerCollision(6, 6); // Ignore collisions with other bullets  
        _startTime = Time.time;

        Vector3 cameraBounds = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        boundsX = cameraBounds.x;
        boundsY = cameraBounds.y;
    }

    private void FixedUpdate()
    {
        Movement();
        CheckShouldDespawn();
    }

    protected virtual void Movement()
    {
        Vector3 moveOffset = transform.up * MoveSpeed;
        transform.position = transform.position + moveOffset;
    }

    private void CheckShouldDespawn()
    {
        if (Time.time - _startTime >= _despawnTime)
        {
            Destroy(gameObject);
        }
        else if (transform.position.x < -boundsX || transform.position.x > boundsX)
        {
            Destroy(gameObject);
        }
        else if (transform.position.y < -boundsY || transform.position.y > boundsY)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        IDamageable obj = other.collider.gameObject.GetComponent<IDamageable>();
        // Debug.Log($"{other.collider.gameObject} hit bullet");

        if (obj == null)
        { // if obj isn't damageable
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.collider.gameObject.GetComponent<Collider2D>());
            return;
        }
        else if (other.collider.gameObject.CompareTag("Enemy") && isEnemy)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.collider.gameObject.GetComponent<Collider2D>());
            return;
        }
        else if (other.collider.gameObject.CompareTag("Friendly") && !isEnemy)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.collider.gameObject.GetComponent<Collider2D>());
            return;
        }
        else if ((other.collider.gameObject.CompareTag("Player") || other.collider.gameObject.CompareTag("Gun")) && !isEnemy)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.collider.gameObject.GetComponent<Collider2D>());
            return;
        }

        obj?.Damage(_damage); // all enemy bullets always 
        Feedback(); // spawn particles and sfx
        gameObject.SetActive(false);
    }

    private void Feedback()
    {
        if (_hitFX != null)
        {
            Instantiate(_hitFX, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }

}
