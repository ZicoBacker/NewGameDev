using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //riggty ranch of empty names... some have default values though!
    public float horizontalInput;
    public float verticalInput;

    public float health = 100f;
    public float speed = 0f;
    public float damageTaken = 10f;
    private Rigidbody2D rb;
    public Vector2 move;
    private Animator anim;
    public ParticleSystem runningParticle;

    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;
    public float boundOffset = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        //giving names their worth
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        //alot of naming huh?
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;

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
        // put the movement in move.
        move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // give the animator my km/s
        anim.SetFloat("Xspeed", Mathf.Abs(move.x));

        // no hp I die. script die with me.
        if (health <= 0)
        {
            anim.SetTrigger("Died");
            enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            health = 0;
        }
    }


    private void Move()
    {
        // not moving if you and moving!
        if ((move.x != 0 || move.y != 0))
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
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 - objectWidth + boundOffset, screenBounds.x + objectWidth + -boundOffset);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1 - objectHeight, screenBounds.y + objectHeight);
        transform.position = viewPos;
    }

    public void Die()
    {
        //gets called from animation UwU
        // self explanitory no?

        //that was before, now this is dumb. we just call lose screen lol.
        GameObject.FindWithTag("GameManager").GetComponent<GameController>().LoseScreen();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            health -= damageTaken;
        }
    }
}