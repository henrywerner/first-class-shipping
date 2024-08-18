using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _tickrate = 1;
    [SerializeField] public bool isEnemy = true;
    private bool _isOnCooldown = false;

    private void OnCollisionStay2D(Collision2D other)
    {
        Debug.Log($"{other.collider.gameObject} hit beam");
        if (!_isOnCooldown)
        {
            IDamageable obj = other.collider.gameObject.GetComponent<IDamageable>();

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

            StartCoroutine(DamageAtTickRate(obj, _damage, _tickrate));
            Feedback(); // spawn particles and sfx
        }
    }

    IEnumerator DamageAtTickRate(IDamageable damageable, int Damage, float Tickrate)
    {
        //damage
        damageable?.Damage(Damage); // all enemy bullets always 
        //cooldown
        _isOnCooldown = true;
        yield return new WaitForSeconds(Tickrate);
        _isOnCooldown = false;
    }

    private void Feedback()
    {
        // TODO: add hit FX here.
    }
}
