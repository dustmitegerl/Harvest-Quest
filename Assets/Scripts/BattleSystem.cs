using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
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

    // Starting the Battle State
    void Start()
    {
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

    // Creating functions for the enemy turn

    IEnumerator EnemyTurn() 
    {
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
        }
        else if (state == BattleState.LOST) 
        {
            dialogueText.text = "The enemy has ended your story.";
            Destroy(playerUnit.gameObject);
        }
    }
    // Creating Player Turn Function

    void PlayerTurn() 
    {
        dialogueText.text = "Select your moves:";
        Debug.Log("Player is picking an action.");
    }

    // Creating Button Functions

    public void OnAttackButton() 
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());

    }

    public void OnSkillButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerSPAction());

    }
}
