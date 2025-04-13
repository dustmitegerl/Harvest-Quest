using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdvancedDialogueManager : MonoBehaviour
{
    //The NPC Dialogue we are currently stepping through
    private AdvancedDialogueSO currentConversation;
    private int stepNum;
    private bool dialogueActivated;

    //UI references
    private GameObject dialogueCanvas;
    private TMP_Text actor;
    private TMP_Text dialogueText;

    private string currentSpeaker;

    public ActorSO[] actorSO;

    //Button references
    private GameObject[] optionButton;
    private TMP_Text[] optionButtonText;
    private GameObject optionsPanel;

    //Canvas to deactivate
    private GameObject Canvas;

    //Player freeze
    private PlayerMovement playerMove;

    //Typewriter effect
    [SerializeField]
    private float typingSpeed = 0.02f;
    private Coroutine typeWriterRoutine;
    private bool canContinueText = true;

    // Start is called before the first frame update
    void Start()
    {
        Canvas = GameObject.Find("Canvas");

        //Find player move script
        playerMove = GameObject.Find("Player").GetComponent<PlayerMovement>();

        //Find buttons
        optionButton = GameObject.FindGameObjectsWithTag("OptionButton");
        optionsPanel = GameObject.Find("OptionsPanel");
        optionsPanel.SetActive(false);

        //Find the TMP text on the buttons
        optionButtonText = new TMP_Text[optionButton.Length];
        for (int i = 0; i < optionButton.Length; i++)
            optionButtonText[i] = optionButton[i].GetComponentInChildren<TMP_Text>();

        //Turn off the buttons to start
        for (int i = 0; i < optionButton.Length; i++)
            optionButton[i].SetActive(false);

        dialogueCanvas = GameObject.Find("DialogueCanvas");
        actor = GameObject.Find("ActorText").GetComponent<TMP_Text>();
        dialogueText = GameObject.Find("DialogueText").GetComponent<TMP_Text>();

        dialogueCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(dialogueActivated && Input.GetKeyDown(KeyCode.Space) && canContinueText)
        {
            //Deactivate Overworld Canvas
            Canvas.SetActive(false);

            //Freeze the player
            playerMove.enabled = false;

            //Cancel dialogue if there are no lines of dialogue remaining
            if (stepNum >= currentConversation.actors.Length)
                TurnOffDialogue();

            //Contains dialogue
            else
                PlayDialogue();
        }
    }

    void PlayDialogue()
    {
        //If it's a random NPC
        if (currentConversation.actors[stepNum] == DialogueActors.Random)
            SetActorInfo(false);

        //if it's a recurring character
        else
            SetActorInfo(true);

        //Display dialogue
        actor.text = currentSpeaker;

        //If there is a branch
        if(currentConversation.actors [stepNum] == DialogueActors.Branch)
        {
            for (int i = 0; i < currentConversation.optionText.Length; i++)
            {
                if (currentConversation.optionText[i] == null)
                    optionButton[i].SetActive(false);
                else
                {
                    optionButtonText[i].text = currentConversation.optionText[i];
                    optionButton[i].SetActive(true);
                }

                //Set the first button to be auto-selected
                optionButton[0].GetComponent<Button>().Select();
            }
        }

        //Keep the routine from running multiple times at the same time
        if (typeWriterRoutine != null)
            StopCoroutine(typeWriterRoutine);

        if (stepNum < currentConversation.dialogue.Length)
            typeWriterRoutine = StartCoroutine(TypeWriterEffect(dialogueText.text = currentConversation.dialogue[stepNum]));
        else
            optionsPanel.SetActive(true);

        dialogueCanvas.SetActive(true);
        stepNum += 1;
    }

    void SetActorInfo(bool recurringCharacter)
    {
        if (recurringCharacter)
        {
            for(int i = 0; i < actorSO.Length; i++)
            {
                if(actorSO[i].name == currentConversation.actors[stepNum].ToString())
                {
                    currentSpeaker = actorSO[i].actorName;
                }
            }
        }
        else
        {
            currentSpeaker = currentConversation.randomActorName;
        }
    }

    public void Option(int optionNum)
    {
        foreach (GameObject button in optionButton)
            button.SetActive(false);

        if (optionNum == 0)
            currentConversation = currentConversation.option0;
        if (optionNum == 1)
            currentConversation = currentConversation.option1;
        if (optionNum == 2)
            currentConversation = currentConversation.option2;
        if (optionNum == 3)
            currentConversation = currentConversation.option3;

        stepNum = 0;
    }

    private IEnumerator TypeWriterEffect(string line)
    {
        dialogueText.text = "";
        canContinueText = false;
        bool addingRichTextTag = false;
        yield return new WaitForSeconds(.5f);
        foreach (char letter in line.ToCharArray())
        {
            if (Input.GetKeyDown(KeyCode.Equals))
            {
                dialogueText.text = line;
                break;
            }
           
            //Check to see if we are working with rich text tags
            if(letter == '<' || addingRichTextTag)
            {
                addingRichTextTag = true;
                dialogueText.text += letter;
                if (letter == '>')
                    addingRichTextTag = false;
            }

            //If not using rich text tags
            else
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }
        canContinueText = true;
    }

    public void InitiateDialogue(NPCDialogue npcDialogue)
    {
        //the array we are currently stepping through
        currentConversation = npcDialogue.conversation[0];

        dialogueActivated = true;
    }

    public void TurnOffDialogue()
    {
        stepNum = 0;

        dialogueActivated = false;
        optionsPanel.SetActive(false);
        dialogueCanvas.SetActive(false);

        //Activate Overworld Canvas
        Canvas.SetActive(true);

        //Unfreeze the player
        playerMove.enabled = true;
    }
}

public enum DialogueActors
{
    Player,
    Blacksmith,
    Random,
    Branch
};
