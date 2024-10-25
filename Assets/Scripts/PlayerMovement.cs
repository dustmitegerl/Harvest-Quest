using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    private bool walking;

    private Rigidbody2D rb;
    private Animator anim;

    private Vector2 movement;
    private Vector3 moveToPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!walking)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            //prevents diagnial movement
            if (movement.x != 0)
            {
                movement.y = 0;
            }

            if (movement != Vector2.zero)
            {
                moveToPosition = transform.position + new Vector3(movement.x, movement.y, 0);
                anim.SetFloat("X", movement.x);
                anim.SetFloat("Y", movement.y);
                StartCoroutine (Move(moveToPosition));
            }

            anim.SetBool("walking", walking);
        }
    }

      IEnumerator Move(Vector3 newPos)
    {
        walking = true;

        while ((newPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPos, moveSpeed * Time.fixedDeltaTime);
            yield return null;
        }
        transform.position = newPos;

        walking = false;

        //transform.Translate(movement.x * moveSpeed * Time.fixedDeltaTime, movement.y * moveSpeed * Time.fixedDeltaTime, 0);
    }
}
