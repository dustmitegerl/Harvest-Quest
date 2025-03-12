using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class BattleSys : MonoBehaviour
{

    [SerializeField] private enum BattleState { Start, Selection, Battle, Won, Lost, Run }

    [Header("Battle State")]
    [SerializeField] private BattleState state;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] partySpawnPoints;
    [SerializeField] private Transform[] enemySpawnPoints;

    [Header("Battle Entities")]
    [SerializeField] private List<BattleEntities> allBattlers = new List<BattleEntities>();
    [SerializeField] private List<BattleEntities> enemyBattlers = new List<BattleEntities>();
    [SerializeField] private List<BattleEntities> playerBattlers = new List<BattleEntities>();

    [Header("UI")]
    [SerializeField] private GameObject[] enemySelectionButtons;
    [SerializeField] private GameObject battleMenu;
    [SerializeField] private GameObject enemySelectionMenu;
    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private GameObject bottomTextPopUp;
    [SerializeField] private TextMeshProUGUI bottomText;
    private PartyManager partyManager;
    private EnemyManager enemyManager;
    private int currentPlayer;

    private const string ACTION_MESSAGE = "'s Action:";
    private const string WIN_MESSAGE = "You won the battle!";
    private const string LOSE_MESSAGE = "You lost the battle!";
    private const int TURN_DURATION = 2; //turn duration
    void Start()
    {


        partyManager = GameObject.FindFirstObjectByType<PartyManager>();
        enemyManager = GameObject.FindFirstObjectByType<EnemyManager>();

        CreatePartyEntities();
        CreateEnemyEntities();
        ShowBattleMenu();
        //AttackAction(allBattlers[0], allBattlers[1]);


    }

    private IEnumerator BattleRoutine()
    {
        enemySelectionMenu.SetActive(false); //enemy selection menu disabled
        state = BattleState.Battle; //change state to BattleState
        bottomTextPopUp.SetActive(true); //bottom text pop up enabled

        for (int i = 0; i < allBattlers.Count; i++) //loop through all battlers
        {

            if (state == BattleState.Battle)
            {
                switch (allBattlers[i].BattleAction) //switch statement for the current battlers action
                {
                    case BattleEntities.Action.Attack: //if the action is attack  
                                                       //do the attack   
                        yield return StartCoroutine(AttackRoutine(i)); //wait for the attack to finish
                        break;
                    case BattleEntities.Action.Run: //if the action is run
                        yield return StartCoroutine(RunRoutine()); // Run
                        break;
                    default:
                        Debug.LogError("Error - incorrect battle action");
                        break;
                }
            }
        }
        // if no win or lost reapeat the loop by opening battle menu
        if (state == BattleState.Battle)
        {
            bottomTextPopUp.SetActive(false);
            currentPlayer = 0;
            ShowBattleMenu();

        }

        yield return null;
    }


    private IEnumerator AttackRoutine(int i)
    {
        //player turn
        if (allBattlers[i].IsPlayer == true)
        {
            BattleEntities currAttacker = allBattlers[i];
            if (allBattlers[currAttacker.Target].IsPlayer == true || currAttacker.Target >= allBattlers.Count)
            {
                currAttacker.SetTarget(GetRandomEnemy());
            }
            BattleEntities currTarget = allBattlers[currAttacker.Target];
            AttackAction(currAttacker, currTarget); //attack slected enemy
            yield return new WaitForSeconds(TURN_DURATION); //wait a few sec

            if (currTarget.CurrHealth <= 0)
            {
                bottomText.text = string.Format("{0} defeated {1}", currAttacker.Name, currTarget.Name);
                yield return new WaitForSeconds(TURN_DURATION); //wait a few sec
                allBattlers.Remove(currTarget);
                enemyBattlers.Remove(currTarget);

                if (enemyBattlers.Count <= 0)
                {
                    state = BattleState.Won;
                    bottomText.text = WIN_MESSAGE;
                    yield return new WaitForSeconds(TURN_DURATION); //wait a few sec
                    Debug.Log("go back to overworld scene");
                    SceneManager.LoadScene("Farm"); // Load the "Farm" scene
                    yield break;

                }

                //if no enemies remain
                // we won the battle
            }
        }

        if (allBattlers[i].IsPlayer == false)
        {
            BattleEntities currAttacker = allBattlers[i];
            currAttacker.SetTarget(GetRandomPartyMember());
            BattleEntities currTarget = allBattlers[currAttacker.Target];

            AttackAction(currAttacker, currTarget); //attack slected enemy
            yield return new WaitForSeconds(TURN_DURATION); //wait a few sec

            if (currTarget.CurrHealth <= 0)
            {
                bottomText.text = string.Format("{0} defeated {1}", currAttacker.Name, currTarget.Name);
                yield return new WaitForSeconds(TURN_DURATION); //wait a few sec
                allBattlers.Remove(currTarget);
                playerBattlers.Remove(currTarget);

                if (playerBattlers.Count <= 0)
                {
                    state = BattleState.Lost;
                    bottomText.text = LOSE_MESSAGE;
                    yield return new WaitForSeconds(TURN_DURATION); //wait a few sec
                    Debug.Log("gameover");

                }

                //if no enemies remain
                // we won the battle
            }

        }
    }

    private IEnumerator RunRoutine()
    {
        if (state == BattleState.Battle)
        {
            if (Random.Range(0, 101) >= 50)
            {
                bottomText.text = "You ran away!";
                state = BattleState.Run;
                allBattlers.Clear();
                yield return new WaitForSeconds(TURN_DURATION); // Wait a few seconds
                SceneManager.LoadScene("Farm");
                yield break;
            }
            else
            {
                bottomText.text = "You failed to run away!";
                yield return new WaitForSeconds(TURN_DURATION); // Wait a few seconds
                state = BattleState.Battle; // Reset state to Battle if run fails
            }
        }
    }



    //kill enenmy


    //enemies turn



    private void CreatePartyEntities()
    {
        //get current party
        //create battle entities for our party members
        //assign the values
        //spawning the visuals
        //set visuals starting vaklues
        //assign it to the battle entity

        List<PartyMember> currentParty = new List<PartyMember>();
        currentParty = partyManager.GetCurrentParty();

        for (int i = 0; i < currentParty.Count; i++)
        {
            BattleEntities tempEntity = new BattleEntities();
            tempEntity.SetEntityValues(currentParty[i].MemberName, currentParty[i].CurrHealth, currentParty[i].MaxHealth,
            currentParty[i].Initiative, currentParty[i].Strength, currentParty[i].Level, true);

            BattleVisuals tempBattleVisuals = Instantiate(currentParty[i].MemberBattleVisualPrefab,
            partySpawnPoints[i].position, Quaternion.identity).GetComponent<BattleVisuals>();

            tempBattleVisuals.SetStartingValues(currentParty[i].MaxHealth, currentParty[i].MaxHealth, currentParty[i].Level);
            tempEntity.BattleVisuals = tempBattleVisuals;

            allBattlers.Add(tempEntity);
            playerBattlers.Add(tempEntity);
        }


    }

    private void CreateEnemyEntities()
    {
        List<Enemy> currentEnemies = new List<Enemy>();
        currentEnemies = enemyManager.GetCurrentEnemies();

        for (int i = 0; i < currentEnemies.Count; i++)
        {
            BattleEntities tempEntity = new BattleEntities();

            tempEntity.SetEntityValues(currentEnemies[i].EnemyName, currentEnemies[i].CurrHealth, currentEnemies[i].MaxHealth,
            currentEnemies[i].Initiative, currentEnemies[i].Strength, currentEnemies[i].Level, false);


            BattleVisuals tempBattleVisuals = Instantiate(currentEnemies[i].EnemyVisualPrefab,
            enemySpawnPoints[i].position, Quaternion.identity).GetComponent<BattleVisuals>();

            tempBattleVisuals.SetStartingValues(currentEnemies[i].MaxHealth, currentEnemies[i].MaxHealth, currentEnemies[i].Level);
            tempEntity.BattleVisuals = tempBattleVisuals;

            allBattlers.Add(tempEntity);
            enemyBattlers.Add(tempEntity);
        }
    }

    public void ShowBattleMenu()
    {
        actionText.text = playerBattlers[currentPlayer].Name + ACTION_MESSAGE;
        SetEnemySelectionButtons();
        battleMenu.SetActive(true);
    }

    public void ShowEnemySelectionMenu()
    {
        battleMenu.SetActive(false); //disable action menu
        enemySelectionMenu.SetActive(true); //enable slection menu

    }

    private void SetEnemySelectionButtons()
    {
        for (int i = 0; i < enemySelectionButtons.Length; i++) //looping through all our enemy selection buttons array
        {
            enemySelectionButtons[i].SetActive(false);
        }

        for (int j = 0; j < enemyBattlers.Count; j++) //looping through all our enemy battlers list
        {
            enemySelectionButtons[j].SetActive(true);
            enemySelectionButtons[j].GetComponentInChildren<TextMeshProUGUI>().text = enemyBattlers[j].Name; //assigning an enemy name read earlier to the button text
        }
    }

    public void SelectEnemy(int currentEnemy)

    {
        BattleEntities currentPlayerEntity = playerBattlers[currentPlayer]; //local reference for current player
        currentPlayerEntity.SetTarget(allBattlers.IndexOf(enemyBattlers[currentEnemy])); //setting current members target

        currentPlayerEntity.BattleAction = BattleEntities.Action.Attack; //setting current members action
        currentPlayer++; //incrementing current player

        if (currentPlayer >= playerBattlers.Count) //if current player is greater than or equal to player battlers count pretty much end the current turn if we went through all players
        {
            StartCoroutine(BattleRoutine());

        }
        else
        {
            enemySelectionMenu.SetActive(false); //disable enemy selection menu
            ShowBattleMenu(); //show battle menu
        }

    }

    private void AttackAction(BattleEntities currAttacker, BattleEntities currTarget)
    {
        int damage = currAttacker.Strenght; //get the damage value
                                            //currAttacker.BattleVisuals.PlayAttackAnimation(); //play attack animation
        currTarget.CurrHealth -= damage; //apply damage to the target
                                         //currTarget.BattleVisuals.PlayHitAnimation(); //play hit animation
        currTarget.UpdateUI(); //update the UI
        bottomText.text = string.Format("{0} attacks {1} for {2} damage", currAttacker.Name, currTarget.Name, damage); //set the bottom text
    }

    private int GetRandomPartyMember()
    {
        List<int> partyMembers = new List<int>(); //create a temp list of party members type int

        for (int i = 0; i < allBattlers.Count; i++) //loop through all player battlers
        {
            if (allBattlers[i].IsPlayer == true) // we have a party member
            {
                partyMembers.Add(i); //add the index to the party members list
            }
        }
        return partyMembers[Random.Range(0, partyMembers.Count)]; //return a random party member index
    }

    private int GetRandomEnemy()
    {
        List<int> enemies = new List<int>(); //create a temp list of enemy members type int

        for (int i = 0; i < allBattlers.Count; i++) //loop through all enemy battlers
        {
            if (allBattlers[i].IsPlayer == false) // we have an enemy member
            {
                enemies.Add(i); //add the index to the enemy members list
            }
        }
        return enemies[Random.Range(0, enemies.Count)]; //return a random enemy member index
    }

    public void SelectRunAction()
{
    state = BattleState.Run; // Set the state to Run
    BattleEntities currentPlayerEntity = playerBattlers[currentPlayer]; // Local reference for current player
    
    currentPlayerEntity.BattleAction = BattleEntities.Action.Run; // Setting current member's action to run
    
    battleMenu.SetActive(false); // Disable battle menu
    currentPlayer++; // Incrementing current player

    if (currentPlayer >= playerBattlers.Count) // If current player is greater than or equal to player battlers count, end the current turn if we went through all players
    {
        StartCoroutine(BattleRoutine());
    }
    else
    {
        enemySelectionMenu.SetActive(false); // Disable enemy selection menu
        ShowBattleMenu(); // Show battle menu
    }
}
}



[System.Serializable]
public class BattleEntities
{
    public enum Action { Attack, Run }
    public Action BattleAction;
    public string Name;
    public int CurrHealth;
    public int MaxHealth;
    public int Initiative;
    public int Strenght;
    public int Level;
    public bool IsPlayer;
    public BattleVisuals BattleVisuals;
    public int Target;


    public void SetEntityValues(string name, int currHealth, int maxHealth, int initiative, int strenght, int level, bool isPlayer)
    {
        Name = name;
        CurrHealth = currHealth;
        MaxHealth = maxHealth;
        Initiative = initiative;
        Strenght = strenght;
        Level = level;
        IsPlayer = isPlayer;
    }
    public void SetTarget(int target)
    {
        Target = target;
    }

    public void UpdateUI()
    {
        BattleVisuals.ChangeHealth(CurrHealth);
    }

}









