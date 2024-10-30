using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{ // reference to script: https://www.youtube.com/watch?v=_1pz_ohupPs

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerPosition;
    public Transform enemyPosition;

    LifeManaHandler playerUnit;
    LifeManaHandler enemyUnit;

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
      playerUnit = playerGO.GetComponent<LifeManaHandler>();

      GameObject enemyGO = Instantiate(enemyPrefab, enemyPosition);
      enemyUnit = enemyGO.GetComponent<LifeManaHandler>();
      
        state = BattleState.PLAYERTURN;
        PlayerTurn();

        yield return new WaitForSeconds(2f);
    }

    void PlayerTurn() 
    { 
      
    }
    // Setting the player to damage the enemy
    IEnumerator PlayerAttack() 
    { 
      bool isDead = enemyUnit.TakeDamage(playerUnit.attackDamage);

        if (isDead)
        {
            state = BattleState.WON;
            EnemyHUD.SetHP(enemyUnit.currentHP = 0);
            EndBattle();
        }
        else 
        { 
            state = BattleState.ENEMYTURN;
            EnemyHUD.SetHP(enemyUnit.currentHP);
            dialogueText.text = "You deal" + playerUnit.attackDamage + "damage" ;

            yield return new WaitForSeconds(2f);
            StartCoroutine(EnemyTurn());
        }
    }

    // Setting the Enemy and Player Turn
    IEnumerator EnemyTurn() 
    {
        //Add dialogue saying that the enemy is attacking the player
        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.attackDamage);

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

<<<<<<< Updated upstream
    // Setting the End Battle
    public void EndBattle() 
    {
        if (state == BattleState.WON)
        {
            // Add dialogue saying you won
            Destroy(enemyUnit.gameObject);
        }
        else if (state == BattleState.LOST)
        {

        }
    }
=======
    
   
     
>>>>>>> Stashed changes
    
    public void OnAttackButton() 
    { 
     if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());
    }
}
