using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float health = 100f;
    [SerializeField] private Transform target;
    private Rigidbody2D rb;

    public float moveSpeed = 1f;
    public float stopRadius = 1f;

    public float attackCDN = 2f;
    private float attackTimer;
    private bool canAttack;
    private Animator animator;
    private bool isDead;
    public ParticleSystem deathParticles;

    [SerializeField] private float wtf;

    // Start is called before the first frame update
    void Start()
    {
        target = Gamemanager.instance.playerPosition;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Debug.Log(stopRadius * 0.66f);
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 positionDifference = transform.position - target.position;
        animator.SetFloat("Xspeed", positionDifference.x - stopRadius);

        //walks towards you until in range
        if ((MathF.Abs(positionDifference.x) > stopRadius) && !isDead)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        }
        //attacks your ass when in range, with cooldown!
        else if (canAttack && !isDead && Gamemanager.instance.playerHealth > 0)
        {
            canAttack = false;

            //This will be important post animations added.
            animator.SetTrigger("attack");
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

        CheckHealth();
        Flip();
    }

    private void Attack()
    {
        //Code that might perhaps attack your ass.
        Debug.Log("yo ass got hit");
    }


    void CheckHealth()
    {
        // die when dead
        if (health <= 0 && !isDead)
        {
            isDead = true;

            //makes the enemy useless, bye bye!!!!
            rb.excludeLayers = LayerMask.GetMask("Player");
            gameObject.layer = LayerMask.GetMask("floor");

            animator.SetTrigger("die");
            Debug.Log("I fucking died!");
        }
    }

    public IEnumerator Die()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
        Instantiate(deathParticles, gameObject.transform.position, quaternion.identity);
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
        takeKb(new Vector2(3f, 0));
    }

    void takeKb(Vector2 knockback)
    {
        //appearantly quaternions are a 1 or 0 value. What the fuck...
        Vector2 deliveredKb = transform.rotation.y != 1 ? new Vector2(-knockback.x, knockback.y) : knockback;
        rb.velocity = new Vector2(deliveredKb.x, rb.velocity.y + deliveredKb.y);
    }
}
