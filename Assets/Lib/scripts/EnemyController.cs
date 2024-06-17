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
    public float damage = 10f;
    public float moveSpeed = 1f;
    public float stopRadius = 1f;
    public float attackCDN = 2f;
    public ParticleSystem deathParticles;

    private float attackTimer;
    private bool canAttack;
    private bool isDead;
    private Animator animator;
    private Rigidbody2D rb;
    private GameObject attackArea;
    private Vector2 positionDifference;
    [SerializeField] private Transform target;
    [SerializeField] private bool damagable = true;

    [SerializeField] private float playerX;

    // Start is called before the first frame update
    void Start()
    {
        attackArea = gameObject.transform.GetChild(0).gameObject;
        target = PlayerController.Instance.transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        playerX = MathF.Abs(positionDifference.x);
        positionDifference = transform.position - target.position;
        animator.SetFloat("Xspeed", MathF.Abs(positionDifference.x) - stopRadius);

        //walks towards you until in range
        if ((MathF.Abs(positionDifference.x) > stopRadius) && !isDead && damagable)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        }
        //attacks your ass when in range, with cooldown!
        else if (canAttack && !isDead && damagable && PlayerController.Instance.health > 0)
        {
            canAttack = false;

            //This will be important post animations added.
            AudioManager.Instance.PlaySFX("weapon_woosh");
            animator.SetTrigger("attack");
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

    void CheckHealth()
    {
        // die when dead
        if (health <= 0 && !isDead)
        {
            isDead = true;

            //makes the enemy useless, bye bye!!!!
            gameObject.layer = LayerMask.NameToLayer("Deathlayer");

            animator.SetTrigger("die");
            AudioManager.Instance.PlaySFX("orc_death");
        }
    }

    public IEnumerator Die()
    {
        GameManager.Instance.EnemyDied(gameObject);
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

        animator.SetTrigger("Hurt");
        TakeKb(new Vector2(3f, 0));
        health -= damage;

    }

    void TakeKb(Vector2 knockback)
    {
        //appearantly quaternions are a 1 or 0 value. What the fuck...
        Vector2 deliveredKb = transform.rotation.y != 1 ? new Vector2(-knockback.x, knockback.y) : knockback;
        rb.velocity = new Vector2(deliveredKb.x, rb.velocity.y + deliveredKb.y);
    }

    void StartAttack()
    {
        attackArea.SetActive(true);
    }

    void EndAttack()
    {
        attackArea.SetActive(false);
    }
}
