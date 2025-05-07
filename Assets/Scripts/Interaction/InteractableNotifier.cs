using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableNotifier : MonoBehaviour
{
    [SerializeField] private GameObject interactionPrompt; // UI Text for "Press E"

    private void Start()
    {
       if (interactionPrompt == null)
        {
            interactionPrompt = GameObject.FindGameObjectWithTag("Interaction Prompt");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (interactionPrompt != null)
                interactionPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (interactionPrompt != null)
                interactionPrompt.SetActive(false);
        }
    }
}
