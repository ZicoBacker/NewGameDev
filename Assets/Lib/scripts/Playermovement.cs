using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Playermovement : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;
    public float speed = 0f;
    public Rigidbody2D rb;
    public Vector2 move;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // Double check for player starting movement
        if ((move.x != 0 || move.y != 0))
        {
            rb.velocity = new Vector2(move.x * speed * Time.deltaTime, rb.velocity.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // makes the player move and put it in a new vector2
        move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

    }
}
