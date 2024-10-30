using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{ // reference to script: https://www.youtube.com/watch?v=_1pz_ohupPs

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerPosition;
    public Transform enemyPosition;

    public LifeManaHandler playerHUD;
    public LifeManaHandler enemyHUD;

    public TextMeshProUGUI dialogueText;
    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());

    }

    public IEnumerator SetupBattle() 
    { 
      GameObject playerGO = Instantiate(playerPrefab, playerPosition);
      playerHUD = playerGO.GetComponent<LifeManaHandler>();

      GameObject enemyGO = Instantiate(enemyPrefab, enemyPosition);
      enemyHUD = enemyGO.GetComponent<LifeManaHandler>();

        dialogueText.text = "The enemy is going to attack you!";
      
        state = BattleState.PLAYERTURN;
        PlayerTurn();

        yield return new WaitForSeconds(2f);
    }

    void PlayerTurn() 
    {
        dialogueText.text = "Select your tactic...";
    }
    
    // Setting the player to damage the enemy
    IEnumerator PlayerAttack() 
    { 
      bool isDead = enemyHUD.TakeDamage(playerHUD.attackDamage);
      enemyHUD.UpdateEnemyUI();

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else 
        { 
            state = BattleState.ENEMYTURN;
            dialogueText.text = "Enemy turn!" ;

            yield return new WaitForSeconds(2f);
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerSkill() 
    {
        if (playerHUD.currentMP >= playerHUD.manaCost) 
        {
            playerHUD.UseSkill();
            enemyHUD.UpdateEnemyUI();
            playerHUD.MPBar.value = playerHUD.currentMP;

            yield return new WaitForSeconds(2f);

            bool isDead = enemyHUD.currentHP <= 0;
            if (isDead)
            {
                state = BattleState.WON;
                EndBattle();
            }

            else 
            {
              state = BattleState.ENEMYTURN;
                dialogueText.text = "Enemy's attack you";
                StartCoroutine(EnemyTurn());
            }
        }
    }

    // Setting the Enemy and Player Turn
    IEnumerator EnemyTurn() 
    {
        dialogueText.text = "The enemy is attacking you.";
        yield return new WaitForSeconds(1f);

        bool isDead = playerHUD.TakeDamage(enemyHUD.attackDamage);

        playerHUD.HPBar.value = playerHUD.currentHP;

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else 
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }

    }

    // Setting the End Battle
    public void EndBattle() 
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "You defeated the enemy!";
            Destroy(enemyHUD.gameObject);
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You loss.";
        }
    }

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

        StartCoroutine(PlayerSkill());
    }
}
