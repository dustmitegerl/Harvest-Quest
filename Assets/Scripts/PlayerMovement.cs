using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    private bool walking;

    private Rigidbody2D rb;
    private Animator anim;

    Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x != 0)
        {
            movement.y = 0;
        }

        if (movement != Vector2.zero)
        {
            if (!walking)
            {
                walking = true;
                anim.SetBool("walking", true);
            }

            Move();
        }

        else
        {
            if (walking)
            {
                walking = false;
                anim.SetBool("walking", false);
            }
        }
    }

    private void Move()
    {
        anim.SetFloat("X", movement.x);
        anim.SetFloat("Y", movement.y);

        transform.Translate(movement.x * moveSpeed * Time.fixedDeltaTime, movement.y * moveSpeed * Time.fixedDeltaTime, 0);
    }
}
