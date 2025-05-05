using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{ //reference to the script: https://www.youtube.com/watch?v=_nRzoTzeyxU&t=296s
   public Dialogue dialogue;
   public void TriggerDialogue ()
   { 
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
   }
}
