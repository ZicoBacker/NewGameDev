using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float health = 100f;
    [SerializeField] private Transform target;
    private Rigidbody2D rb;
    private Vector2 moveDirection;

    public float moveSpeed = 1f;
    public float stopRadius = 1f;

    public float attackCDN = 2f;
    private float attackTimer;
    private bool canAttack;

    public ParticleSystem deathParticles;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 positionDifference = transform.position - target.position;

        if (MathF.Abs(positionDifference.x) > stopRadius)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        }
        else if (canAttack)
        {
            Attack();
        }


        // as long as no attack, we wait..
        if (!canAttack)
        {
            {
                attackTimer += Time.deltaTime;

                if (attackTimer >= attackCDN)
                {
                    attackTimer = 0;
                    canAttack = true;

                }
            }
        }

        checkHealth();
        Flip();
    }

    private void Attack()
    {
        canAttack = false;
        //perhaps I should add attacking code.
        Debug.Log("i attacked your ass!");
    }


    void checkHealth()
    {
        // die when dead
        if (health <= 0)
        {
            Destroy(gameObject);
            Debug.Log("I fucking died!");
            Instantiate(deathParticles, gameObject.transform.position, quaternion.identity);
        }
    }

    void Flip()
    {
        float playerXDiff = target.position.x - transform.position.x;

        if (playerXDiff < 0)
        {
            transform.rotation = new quaternion(0, 180f, 0, 0);
        }
        else if (playerXDiff > 0)
        {
            transform.rotation = new quaternion(0, 0, 0, 0);
        }
    }

    //Literally does what is says, takes damage from attack.
    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("took " + damage + " damage!");
        takeKb(new Vector2(3f, 2.5f));
    }

    void takeKb(Vector2 knockback)
    {
        //appearantly quaternions are a 1 or 0 value. What the fuck...
        Vector2 deliveredKb = transform.rotation.y != 1 ? new Vector2(-knockback.x, knockback.y) : knockback;
        rb.velocity = new Vector2(deliveredKb.x, rb.velocity.y + deliveredKb.y);
    }

}
