using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    private bool walking;
    public LayerMask solidObjectLayer;
    public LayerMask interactableLayer;
    public LayerMask wallLayer;

    public event Action OnEncountered;

    //private Rigidbody2D rb;
    private Animator anim;

    private Vector2 movement;
    private Vector3 moveToPosition;

    void Start()
    {//Animation will happen once movement start
        //rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void HandleUpdate()
    {
        if (!walking)
        {//Player's movement
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
                anim.SetFloat("moveX", movement.x);
                anim.SetFloat("moveY", movement.y);

                if(Walkable(moveToPosition))
                    StartCoroutine (Move(moveToPosition));
            }
        }//Animation for the player's movement
        anim.SetBool("isMoving", walking);
        //for the dialog
        if (Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(Interact());
    }

    IEnumerator Interact()
    {
        var facingDir = new Vector3(anim.GetFloat("moveX"), anim.GetFloat("moveY"));
        var interactPos = transform.position + facingDir;

        var collider = Physics2D.OverlapCircle(interactPos, 0.5f, interactableLayer);
        if (collider != null)
        {
            yield return collider.GetComponent<Interactable>()?.Interact(transform);
        }
    }

    IEnumerator Move(Vector3 newPos)
    {
        walking = true;

        while ((newPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = newPos;

        walking = false;
    }

    private bool Walkable(Vector3 newPos)
    {//Not walk over objects
        if (Physics2D.OverlapCircle(newPos, 0.5f, solidObjectLayer | wallLayer) != null)
        {
            return false;
        }

        return true;
    }
    private void CheckForEncounters()
    {//Not walk over objects
        if(Physics2D.OverlapCircle(transform.position, 0.5f) != null)
        {
            if (UnityEngine.Random.Range(1, 101) <= 10)
            {
                anim.SetBool("walking", false);
                OnEncountered();
            }
        }
    }
}
