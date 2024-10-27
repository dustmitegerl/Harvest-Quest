using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    private bool walking;
    public LayerMask solidObjectLayer;
    public LayerMask interactableLayer;

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

                if(Walkable(moveToPosition))
                    StartCoroutine (Move(moveToPosition));
            }
        }
        anim.SetBool("walking", walking);

        if (Input.GetKeyDown(KeyCode.Tab))
            Interact();
    }

    void Interact()
    {
        var facingDir = new Vector3(anim.GetFloat("X"), anim.GetFloat("Y"));
        var interactPos = transform.position + facingDir;

        //Debug.DrawLine(transform.position, interactPos, Color.green, 0.5f);

        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, interactableLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
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

    private bool Walkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectLayer | interactableLayer) != null)
        {
            return false;
        }

        return true;
    }
}
