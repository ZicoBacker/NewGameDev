using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //riggty ranch of empty names... some have default values though!

    public static PlayerController Instance { get; private set; }
    public float horizontalInput;
    public float verticalInput;

    public float health = 100f;
    public float speed = 0f;
    public float attackDamage = 50f;
    public float knockback = 3f;

    private Rigidbody2D rb;
    public Vector2 move;
    private Animator anim;
    public ParticleSystem runningParticle;

    private Vector2 screenBounds;
    public float boundOffset = 0.5f;
    public GameObject playerAttack;

    public bool canAttack;
    public float attackCDN;
    private float attackTimer;

    // player is singleton Yippie
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //giving names their worth
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        //alot of naming huh?
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        GameManager.Instance.SetHealh(health);

    }

    private void FixedUpdate()
    {
        //just to have it nice :)
        Move();
        Flip();
    }

    // Update is called once per frame
    void Update()
    {
        // Gamemanager.instance.playerHealth.text = "hp: " + health;
        // put the movement in move.
        move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // give the animator my km/s
        anim.SetFloat("Xspeed", Mathf.Abs(move.x));

        // no hp I die. script die with me.
        if (health <= 0)
        {
            Death();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            health = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }


    private void Move()
    {
        // not moving if you and moving!
        if ((move.x != 0 || move.y != 0) && GameManager.Instance.isRunning)
        {
            rb.velocity = new Vector2(move.x * speed * Time.deltaTime, rb.velocity.y);

            //if there's wind, then don't do more wind.
            if (!runningParticle.isPlaying)
            {
                runningParticle.Play();
            }
        }
        else
        {
            // if bitchass is not running, don't give him wind.
            runningParticle.Stop();
        }
    }

    private void Flip()
    {
        //jiggles the other way when moving the other way
        if (move.x < 0)
        {
            transform.rotation = new quaternion(0, 180, 0, 0);


        }
        else if (move.x > 0)
        {
            transform.rotation = new quaternion(0, 0, 0, 0);
        }
    }

    // camera will always see you...
    void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 + boundOffset, screenBounds.x + -boundOffset);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1, screenBounds.y);
        transform.position = viewPos;
    }

    private void Death()
    {
        // rb.excludeLayers = LayerMask.GetMask("Player");
        gameObject.layer = LayerMask.NameToLayer("Deathlayer");
        AudioManager.Instance.PlaySFX("oshit");
        Destroy(rb);
        anim.SetTrigger("Die");
        enabled = false;
    }

    public void Die()
    {
        //gets called from animation UwU
        // self explanitory no?

        //that was before, now this is dumb. we just call lose screen lol.
        GameManager.Instance.LoseScreen();
    }

    void Attack()
    {
        // as long as no attack, we wait..
        if (canAttack && health > 0)
        {
            AudioManager.Instance.PlaySFX("weapon_woosh");
            canAttack = false;
            Invoke("AttackCDN", attackCDN);
            anim.SetTrigger("attack");
        }

    }

    void AttackCDN()
    {
        canAttack = true;
    }


    void AttackStart()
    {
        playerAttack.SetActive(true);
    }

    void AttackEnd()
    {
        playerAttack.SetActive(false);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        anim.SetTrigger("Hurt");
        GameManager.Instance.SetHealh(health);
    }

    public void UpATTSpeed()
    {
        attackCDN -= 0.25f;
    }

    public void UpDamage()
    {
        attackDamage += 20;
    }


    public void Upgrade(GameManager.UpChoice upgrade, float percentage)
    {
        switch (upgrade.ToString())
        {
            case "Speed":
                attackCDN *= 1f - (percentage / 100);
                break;
            case "Damage":
                attackDamage *= 1f + (percentage / 100);
                break;
            case "Heal":
                health += percentage;
                GameManager.Instance.SetHealh(health);
                break;
            case "knockback":
                knockback *= 1f + (percentage / 100);
                break;
        }
    }
}