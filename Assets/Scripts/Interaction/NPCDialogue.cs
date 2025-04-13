using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public AdvancedDialogueSO[] conversation;

    private Transform player;
    private SpriteRenderer speechBubbleRenderer;

    private AdvancedDialogueManager advancedDialogueManager;

    private bool dialogueInitiated;

    // Start is called before the first frame update
    void Start()
    {
        advancedDialogueManager = GameObject.Find("DialogueManager").GetComponent<AdvancedDialogueManager>();
        speechBubbleRenderer = GetComponent<SpriteRenderer>();
        speechBubbleRenderer.enabled = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !dialogueInitiated)
        {
            //Speech bubble on
            speechBubbleRenderer.enabled = true;

            //Find the player's transform
            player = collision.gameObject.GetComponent<Transform>();

            //Check to see where the player is, and turn toward them
            if(player.position.x > transform.position.x && transform.parent.localScale.x < 0)
            {
                Flip();
            }

            else if(player.position.x < transform.position.x && transform.parent.localScale.x > 0)
            {
                Flip();
            }

            /*else if (player.position.y > transform.position.y && transform.parent.localScale.y < 0)
            {
               // Flip();
            }

           else if(player.position.y < transform.position.y && transform.parent.localScale.y > 0)
            {
                Flip();
            }*/

            advancedDialogueManager.InitiateDialogue(this);
            dialogueInitiated = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            speechBubbleRenderer.enabled = false;

            advancedDialogueManager.TurnOffDialogue();
            dialogueInitiated = false;
        }
        
    }

    private void Flip()
    {
        Vector3 currentScale = transform.parent.localScale;
        currentScale.x *= -1;
        //currentScale.y *= -1;
        transform.parent.localScale = currentScale;
    }
}
