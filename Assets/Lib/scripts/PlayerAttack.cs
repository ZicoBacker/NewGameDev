using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    // public LayerMask attackLayer;
    // private RaycastHit2D[] hits;

    private float damage;

    // [SerializeField] private Transform attackTransform;
    // [SerializeField] private float attackRange = 1.5f;
    // [SerializeField] private LayerMask attackableLayer;
    // public float attackDamage = 50f;

    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.DrawWireSphere(attackTransform.position, attackRange);
    // }

    // public void Attack()
    // {
    //     hits = Physics2D.CircleCastAll(attackTransform.position, attackRange, transform.right, 0f, attackableLayer);

    //     for (int i = 0; i < hits.Length; i++)
    //     {
    //         Debug.Log(hits[i]);
    //         hits[i].collider.gameObject.GetComponent<EnemyController>().TakeDamage(attackDamage);
    //     }
    // }


    void Start()
    {
        damage = Gamemanager.instance.player.GetComponent<PlayerController>().attackDamage;
    }
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Attackable"))
        {
            other.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
        }
    }



}
