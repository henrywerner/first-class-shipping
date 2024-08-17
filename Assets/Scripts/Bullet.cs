using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D _rb;
    
    [Header("Stats")]
    [SerializeField] public float MoveSpeed = 0.3f;
    [SerializeField] private int _damage = 1;
    [SerializeField] public bool isEnemy = true;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0;
    }

    private void FixedUpdate()
    {
        Movement(_rb);
    }

    protected virtual void Movement(Rigidbody2D rb)
    {
        Vector2 moveOffset = transform.up * MoveSpeed;
        rb.MovePosition(rb.position + moveOffset);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        IDamageable obj = other.gameObject.GetComponent<IDamageable>();
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
