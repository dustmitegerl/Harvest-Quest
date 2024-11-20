using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }
public class BattleSystem : MonoBehaviour
{
    LevelLoader levelLoader;

    [SerializeField]
    GameObject levelLoaderPrefab;

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

    public Button attackButton;
    public Button skillButton;

    // Starting the Battle State
    void Start()
    {
        levelLoaderPrefab = GameObject.FindGameObjectWithTag("Game Manager");
        levelLoader = levelLoaderPrefab.GetComponent<LevelLoader>();
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    // Setting up the player and enemy positions while including dialogue
    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab);
        playerGO.transform.position = playerPosition.position;
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
        yield return new WaitForSeconds(1f);

        bool escapeSuccessful = Random.Range(0, 2) == 0;

        if (escapeSuccessful)
        {
            dialogueText.text = "You strategically withdrawn from battle!";
            Debug.Log("Player ran from battle.");

            yield return new WaitForSeconds(1f);
            levelLoader.EndBattle();
        }
        else 
        {
            dialogueText.text = "No where to run.";
            Debug.Log("Player failed to escape.");

            yield return new WaitForSeconds(1f);

            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }

    }
    // Creating functions for the enemy turn

    IEnumerator EnemyTurn() 
    {
        actionMenu.SetActive(false);

        dialogueText.text = enemyUnit.unitName + " is making their moves.";
        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
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

        attackButton.interactable = false;
        skillButton.interactable = false;

        StartCoroutine(PlayerSPAction());

    }

    // Creating a Function Run Button
    public void OnRunButton() 
    { 
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerRun());
    }
}
