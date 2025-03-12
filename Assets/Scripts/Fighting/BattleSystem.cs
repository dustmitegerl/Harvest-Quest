using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, PLAYERCOUNTER, WON, LOST }
public class BattleSystem : MonoBehaviour
{
    LevelLoader levelLoader;

    // Reference to script: https://www.youtube.com/watch?v=_1pz_ohupPs

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerPosition;
    public Transform enemyPosition;

    Unit playerUnit;
    Unit enemyUnit;

    public TextMeshProUGUI dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public BattleState state;

    public GameObject actionMenu;
    public GameObject skillsMenu;
    public GameObject dialogueBox;
    public GameObject mainMenu;
    

    public Button attackButton;
    public Button skillButton;
    public Button healButton;
    public Button backButton;
    public Button MeleeButton;
    public Button counterButton;

    public Button atkdownButton;
 
    public int fireCost;
    public int fireDamage;

    public int iceCost;
    public int iceDamage;

    public int healCost;
    public int healAmount;

    public int attackDownSPCost;
    public int attackDownAmount;
    public int attackDownDuration;

    public int shuffleSPCost;

    // Starting the Battle State
    void Start()
    {
        levelLoader = GameObject.FindGameObjectWithTag("Level Loader").GetComponent<LevelLoader>();
        state = BattleState.START;
        StartCoroutine(SetupBattle());
        
        healButton.onClick.AddListener(OnHealSkill); 
    }
    // Setting up the player and enemy positions while including dialogue
    IEnumerator SetupBattle()
    { 
        GameObject playerGO = Instantiate(playerPrefab);
        Destroy(playerGO.transform.Find("Main Camera").gameObject);
        playerGO.transform.position = playerPosition.position;
        playerGO.transform.rotation = Quaternion.identity;
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab);
        enemyGO.transform.position = enemyPosition.position;
        enemyUnit = enemyGO.GetComponent<Unit>();

        dialogueText.text = "You are being attack by " + enemyUnit.unitName + "!";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        //Transitioning to PLayer's Turn

        state = BattleState.PLAYERTURN;
        PlayerTurn();
        Debug.Log("It's the player's turn.");
    }

    public void ShowSkillsMenu() 
    {
        actionMenu.SetActive(false);
        skillsMenu.SetActive(true);
    }

    public void HideSkillsMenu()
    {
        actionMenu.SetActive(true);
        skillsMenu.SetActive(false);
    }

    public void HideDialogueBox() 
    { 
        dialogueBox.SetActive(false);
        dialogueText.text = "";
    }

    private void ShowDialogueBox(string message) 
    { 
        dialogueBox.SetActive(true);
        dialogueText.text = message;
    }
    // Adding function to Player Attack Button

    IEnumerator PlayerAttack() 
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = "The attack hit " + enemyUnit.unitName + "!"; 

        yield return new WaitForSeconds(2f);

        // Creating a Battle State for winning and losing

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
            Debug.Log("Player Won.");
        }
        else 
        { 
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
            Debug.Log("Enemy Turn!");
        }
    }

    // Adding function to Player Attack Button

    IEnumerator PlayerSPAction() 
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        enemyHUD.SetSP(enemyUnit.currentHP);
        dialogueText.text = "Player unleash their skills.";

        playerHUD.SetSP(playerUnit.currentSP);


        yield return new WaitForSeconds(2f);

        // Creating a Battle State for winning and losing

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
            Debug.Log("Player Won.");
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
            Debug.Log("Enemy Turn!");
        }

        state = BattleState.ENEMYTURN;
        dialogueText.text = "Enemy's Turn!";
    }

    // Implementing Run Button
    IEnumerator PlayerRun() 
    {
        //dialogueText.text = "Is it best to run?";
        yield return new WaitForSeconds(1f);

        bool escapeSuccessful = Random.Range(0, 2) == 0;

        if (escapeSuccessful)
        {
            dialogueText.text = "You strategically withdraw from battle!";
            Debug.Log("Player ran from battle.");

            yield return new WaitForSeconds(1f);
            levelLoader.EndBattle();
        }
        else 
        {
            dialogueText.text = "No where to run.";
            Debug.Log("Player failed to escape.");

            yield return new WaitForSeconds(1f);

            if (state == BattleState.PLAYERTURN) 
            {
                attackButton.interactable = true;
                skillButton.interactable = true;
                healButton.interactable = true;
                backButton.interactable = true;
            }
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }

    }
    // Creating functions for the enemy turn

    IEnumerator EnemyTurn() 
    {
        actionMenu.SetActive(false);

        dialogueText.text = enemyUnit.unitName + " is making their move.";
        yield return new WaitForSeconds(1f);

        /*bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
        playerHUD.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
            Debug.Log("Enemy Won");
        }
        else 
        { 
            state = BattleState.PLAYERTURN;
            PlayerTurn();
            Debug.Log("Player takes the turn");
        }*/

        bool isDead;

        if (state == BattleState.PLAYERCOUNTER)
        {
            // Reduce damage taken
            int reduceDamage = Mathf.Max(enemyUnit.damage / 2, 1);
            isDead = playerUnit.TakeDamage(reduceDamage);
            playerHUD.SetHP(playerUnit.currentHP);

            dialogueText.text = playerUnit.unitName + " has block the attack and is ready to counter";
            yield return new WaitForSeconds(1f);

            // Player Countering an Attack
            bool enemyDefeated = enemyUnit.TakeDamage(playerUnit.damage);
            enemyHUD.SetHP(enemyUnit.currentHP);
            dialogueText.text = enemyUnit.unitName + " was hit with a counter attack!";

            yield return new WaitForSeconds(1f);

            if (enemyDefeated)
            {
                state = BattleState.WON;
                EndBattle();
                yield break;
            }
        }
        else 
        {
            // Regular Attacks
            isDead = playerUnit.TakeDamage(enemyUnit.damage);
            playerHUD.SetHP(playerUnit.currentHP);
            yield return new WaitForSeconds(1f);
        }

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
            Debug.Log("Enemy Won");
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
            Debug.Log("Player takes the turn");
        }
    }

    // Creating an End Battle State

    void EndBattle() 
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "You were victorious!";
            Destroy(enemyUnit.gameObject);
            levelLoader.EndBattle();
        }
        else if (state == BattleState.LOST) 
        {
            dialogueText.text = "The enemy has ended your story.";
            Destroy(playerUnit.gameObject);
            levelLoader.EndBattle();
        }
    }
    // Creating Player Turn Function

    void PlayerTurn() 
    {
        dialogueText.text = "Select your moves:";
        actionMenu.SetActive(true);

        attackButton.interactable = true;
        skillButton.interactable = true;

        Debug.Log("Player is picking an action.");
    }

    // Creating Button Functions

    public void OnAttackButton() 
    {
        if (state != BattleState.PLAYERTURN)
            return;

        attackButton.interactable = false;
        skillButton.interactable = false;

        StartCoroutine(PlayerAttack());

    }

    public void OnSkillButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        //attackButton.interactable = false;
        //skillButton.interactable = false;

        //skillsMenu.SetActive(!skillsMenu.activeSelf);
        //StartCoroutine(PlayerSPAction());

        actionMenu.SetActive(false);
        skillsMenu.SetActive(true);

        HideDialogueBox();
        //ShowDialogueBox("Unleash your skill and defeat the enemy:");
        //dialogueBox.SetActive(false);


    }

    //Create Melee Button Function
    public void OnMeleeButton() 
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = "The attack hit " + enemyUnit.unitName + "!";

        skillsMenu.SetActive(false);
        actionMenu.SetActive(true);
        ShowDialogueBox("The next move is yours");

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
            
    }

    // Creating Function to the Counter Button
    public void OnCounterButton() 
    {
        if (state != BattleState.PLAYERTURN)
            return;

        actionMenu.SetActive(false);
        skillsMenu.SetActive(false);

        dialogueText.text = playerUnit.unitName + " is about to counter" + enemyUnit.unitName + " 's attack!";

        //Counter State
        state = BattleState.PLAYERCOUNTER;

        // Ending Player's Turn
        StartCoroutine(EnemyTurn());
    }

    // Creating Function to Fire Attack Button
    public void OnFireSkill() 
    {
        if (playerUnit.currentSP >= fireCost)
        {
            playerUnit.currentSP -= fireCost;
            playerHUD.SetSP(playerUnit.currentSP);

            bool isDead = enemyUnit.TakeDamage(fireDamage);
            enemyHUD.SetHP(enemyUnit.currentHP);

            dialogueText.text = playerUnit.unitName + " has used Fire.";

            skillsMenu.SetActive(false);
            actionMenu.SetActive(true);
            ShowDialogueBox("Next move is yours");
            
            //dialogueBox.SetActive(true);
            

            if (isDead)
            {
                state = BattleState.WON;
                EndBattle();
            }
            else
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
        }
        else 
        {
            dialogueText.text = "No SP for that skill";
            backButton.interactable = true;
        }
    }
    // Creating Function to Ice Attack Button
    public void OnIceSkill()
    {
        if (playerUnit.currentSP >= iceCost)
        {
            playerUnit.currentSP -= iceCost;
            playerHUD.SetSP(playerUnit.currentSP);

            bool isDead = enemyUnit.TakeDamage(iceDamage);
            enemyHUD.SetHP(enemyUnit.currentHP);

            dialogueText.text = playerUnit.unitName + " has used Ice.";
            
            ShowDialogueBox("The next move is yours");
            
            skillsMenu.SetActive(false);
            actionMenu.SetActive(true);
            
            //dialogueBox.SetActive(true);

            if (isDead)
            {
                state = BattleState.WON;
                EndBattle();
            }
            else
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
        }
        else
        {
            dialogueText.text = "No SP for that skill";
            //ShowDialogueBox(dialogueText.text);

            //backButton.gameObject.SetActive(true);
            backButton.interactable = true;
        }
    }
    // Creating Function to Healing Button
    public void OnHealSkill() 
    {
        if (playerUnit.currentSP >= healCost)
        {
            playerUnit.currentSP -= healCost;
            playerHUD.SetSP(playerUnit.currentSP);

            playerUnit.Heal(healAmount);
            playerHUD.SetHP(playerUnit.currentHP);

            dialogueText.text = playerUnit.unitName + " used healing.";

            skillsMenu.SetActive(false);
            actionMenu.SetActive(true);
            ShowDialogueBox("The next move is yours.");

            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
        else 
        {
            dialogueText.text = "No SP for that skill";
            //ShowDialogueBox(dialogueText.text);

            //backButton.gameObject.SetActive(true);
            backButton.interactable = true;
        }
    }

    // Creating Attack Down Button
    public void OnAttackDownButton()
    {
        if (state != BattleState.PLAYERTURN)
        return;

        if (playerUnit.currentSP <attackDownSPCost)
        {
            dialogueText.text = "Not enough SP!";
            return;
        }

        // Reducing SP
        playerUnit.currentSP -= attackDownSPCost;
        playerHUD.SetSP(playerUnit.currentSP);

        // Using the Attack Down to the enemy
        enemyUnit.ApplyAttackDebuff(attackDownAmount, attackDownDuration);

        enemyUnit.DecreaseDebuffTurns();
        playerUnit.DecreaseDebuffTurns();

            skillsMenu.SetActive(false);
            actionMenu.SetActive(true);
            
        dialogueText.text = enemyUnit.unitName + " 's attack level went down for" + attackDownDuration + " turns!";
        StartCoroutine(EnemyTurn());
    }

    // Creating Shuffle Button 
    public void OnShuffleButton()
    {
        if (state != BattleState.PLAYERTURN)
        return;

        if(playerUnit.currentSP < shuffleSPCost)
        {
            dialogueText.text = "Not enough SP!";
            return;
        }

        // Reducing SP
        playerUnit.currentSP -= shuffleSPCost;
        playerHUD.SetSP(playerUnit.currentSP);

        // 50-50 Chance for enemy to lose a turn
        bool enemySkipsTurn = Random.value < 0.5f;

        if(enemySkipsTurn)
        {
            dialogueText.text = "The enemy is unable to think straight and loses their chance to strike.";
            StartCoroutine(PlayerTurnAfterShuffle()); 
        }
        else
        {
            dialogueText.text = "The enemy do not lose their thoughts. Watch out for their attack!";
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerTurnAfterShuffle()
    {
        yield return new WaitForSeconds(2f);
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }
    // Creating a Function Run Button
    public void OnRunButton() 
    { 
        if (state != BattleState.PLAYERTURN)
            return;

        attackButton.interactable = false;
        skillButton.interactable = false;
        healButton.interactable = false;
        backButton.interactable = true;
        
        StartCoroutine(PlayerRun());
    }

    // Adding Back Button for the player to go back to the Action Selector
    public void OnBackButton() 
    {
        skillsMenu.SetActive(false);
        actionMenu.SetActive(true);

        attackButton.interactable = true;
        skillButton.interactable = true;
        healButton.interactable = true;
        backButton.interactable = true;

        //backButton.gameObject.SetActive(true);

        ShowDialogueBox("Pick a move");
    }
      
}
