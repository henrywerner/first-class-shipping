using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] float _moveSpeed = .25f;
    public float MoveSpeed
    {
        get => _moveSpeed;
        set => _moveSpeed = value;
    }

    Rigidbody2D _rb2D = null;

    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        // calculate the move amount
        float verticalMoveThisFrame = Input.GetAxis("Vertical") * _moveSpeed;
        float horizontalMoveThisFrame = Input.GetAxis("Horizontal") * _moveSpeed;
        // create a vector from amount and direction
        Vector2 moveOffset = (transform.up * verticalMoveThisFrame) + (transform.right * horizontalMoveThisFrame);
        // apply vector to the rigidbody
        _rb2D.MovePosition(_rb2D.position + moveOffset);
        // technically adjusting vector is more accurate! (but more complex)
    }
}
