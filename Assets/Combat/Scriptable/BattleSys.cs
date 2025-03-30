using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.UI;
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
    [SerializeField] private GameObject[] skillButtons;
    [SerializeField] private GameObject skillSelectionMenu; // like the Battle Menu

    //[SerializeField] private Transform skillButtonContainer;
    //[SerializeField] private GameObject skillButtonPrefab;
    [SerializeField] private TextMeshProUGUI skillDescriptionText;

    private PartyManager partyManager;
    private EnemyManager enemyManager;
    private int currentPlayer;

    private const string ACTION_MESSAGE = "'s Action:";
    private const string WIN_MESSAGE = "You won the battle!";
    private const string LOSE_MESSAGE = "You lost the battle!";
    private const int TURN_DURATION = 2; //turn duration
    void Start()
    {
        partyManager = FindObjectOfType<PartyManager>();
        enemyManager = FindObjectOfType<EnemyManager>();

        CreatePartyEntities();
        CreateEnemyEntities();

        currentPlayer = 0;
        state = BattleState.Battle; // ✅ Critical line added
        ShowBattleMenu(); // ✅ This now works properly
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
        else
        {
            battleMenu.SetActive(false);
            skillSelectionMenu.SetActive(false);
            enemySelectionMenu.SetActive(false);
        }


        yield return null;
    }


    private IEnumerator AttackRoutine(int i)
    {
        BattleEntities currAttacker = allBattlers[i];

        // === PLAYER TURN ===
        if (currAttacker.IsPlayer)
        {
            if (currAttacker.Target < 0 || currAttacker.Target >= allBattlers.Count || allBattlers[currAttacker.Target].IsPlayer)
            {
                currAttacker.SetTarget(GetRandomEnemy());
            }

            BattleEntities currTarget = allBattlers[currAttacker.Target];
            AttackAction(currAttacker, currTarget);
            yield return new WaitForSeconds(TURN_DURATION);

            if (currTarget.CurrHealth <= 0)
            {
                allBattlers.Remove(currTarget);
                enemyBattlers.Remove(currTarget);
                Destroy(currTarget.BattleVisuals.gameObject);

                bottomText.text = currAttacker.Name + " defeated " + currTarget.Name + "!";

                if (enemyBattlers.Count <= 0)
                {
                    state = BattleState.Won;
                    bottomText.text = WIN_MESSAGE;
                    StartCoroutine(DelayedSceneLoad("Farm", TURN_DURATION));
                    yield break;
                }
            }
        }

        // === ENEMY TURN ===
        else
        {
            currAttacker.BattleAction = BattleEntities.Action.Attack;

            // Skill selection
            Skill chosen = null;
            Enemy matchingEnemyData = enemyManager.GetCurrentEnemies().Find(e => e.EnemyName == currAttacker.Name);

            if (matchingEnemyData != null)
            {
                bool useSpecial = Random.value > 0.7f && matchingEnemyData.SpecialSkill != null;

                if (useSpecial)
                    chosen = matchingEnemyData.SpecialSkill;
                else if (matchingEnemyData.BasicSkill != null)
                    chosen = matchingEnemyData.BasicSkill;
            }

            // Fallback skill
            if (chosen == null)
            {
                Debug.LogWarning($"{currAttacker.Name} has no skill assigned. Using fallback.");
                chosen = ScriptableObject.CreateInstance<Skill>();
                chosen.SkillName = "Enemy Claw";
                chosen.DealsDamage = true;
                chosen.DamageAmount = currAttacker.Strenght;
                chosen.TargetType = SkillTarget.OneEnemy;
            }

            currAttacker.ChosenSkill = chosen;

            // === Targeting based on skill ===
            switch (chosen.TargetType)
            {
                case SkillTarget.OneEnemy:
                    currAttacker.SetTarget(GetRandomPartyMember());
                    break;

                case SkillTarget.AllEnemies:
                case SkillTarget.AllAllies:
                case SkillTarget.Self:
                    currAttacker.SetTarget(-1); // No specific target needed
                    break;

                case SkillTarget.OneAlly:
                    currAttacker.SetTarget(GetRandomEnemy());
                    break;
            }

            // Perform action
            BattleEntities currTarget = currAttacker.Target >= 0 && currAttacker.Target < allBattlers.Count
                ? allBattlers[currAttacker.Target]
                : null;

            AttackAction(currAttacker, currTarget);
            yield return new WaitForSeconds(TURN_DURATION);

            // Handle target death (only if it's a player and single-target)
            if (currTarget != null && currTarget.IsPlayer && currTarget.CurrHealth <= 0)
            {
                allBattlers.Remove(currTarget);
                playerBattlers.Remove(currTarget);
                Destroy(currTarget.BattleVisuals.gameObject);

                bottomText.text = currAttacker.Name + " defeated " + currTarget.Name + "!";

                if (playerBattlers.Count <= 0)
                {
                    state = BattleState.Lost;
                    bottomText.text = LOSE_MESSAGE;
                    battleMenu.SetActive(false);
                    skillSelectionMenu.SetActive(false);
                    enemySelectionMenu.SetActive(false);
                    StartCoroutine(DelayedSceneLoad("Farm", TURN_DURATION));
                    yield break;
                }
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
        // Skip any dead players
        while (currentPlayer < playerBattlers.Count && playerBattlers[currentPlayer].CurrHealth <= 0)
        {
            currentPlayer++;
        }

        // If all players are dead or we've gone past the list, trigger enemy turn
        if (currentPlayer >= playerBattlers.Count)
        {
            StartCoroutine(BattleRoutine());
            return;
        }

        if (state != BattleState.Battle)
        {
            Debug.LogWarning("Tried to show battle menu while not in battle state.");
            return;
        }

        // Set UI title
        actionText.text = playerBattlers[currentPlayer].Name + ACTION_MESSAGE;

        // Pull current player's skills
        PartyMember memberData = partyManager.GetCurrentParty()[currentPlayer];

        for (int i = 0; i < skillButtons.Length; i++)
        {
            if (i < memberData.Skills.Length)
            {
                Skill skill = memberData.Skills[i];

                skillButtons[i].SetActive(true);
                skillButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = skill.SkillName;

                int index = i; // capture index for closure
                skillButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
                skillButtons[i].GetComponent<Button>().onClick.AddListener(() => OnSkillSelected(skill));

                // Show description on hover
                EventTrigger trigger = skillButtons[i].GetComponent<EventTrigger>();
                if (trigger != null)
                {
                    trigger.triggers.Clear();
                    var entry = new EventTrigger.Entry
                    {
                        eventID = EventTriggerType.PointerEnter
                    };
                    entry.callback.AddListener((_) => skillDescriptionText.text = skill.Description);
                    trigger.triggers.Add(entry);
                }
            }
            else
            {
                skillButtons[i].SetActive(false); // hide unused buttons
            }
        }

        // Show menu
        battleMenu.SetActive(true);
    }



    private void OnSkillSelected(Skill selectedSkill)
    {
        skillSelectionMenu.SetActive(false);

        BattleEntities entity = playerBattlers[currentPlayer];
        entity.ChosenSkill = selectedSkill;

        if (selectedSkill.TargetType == SkillTarget.OneEnemy)
        {
            ShowEnemySelectionMenu();
        }
        else
        {
            entity.SetTarget(-1);
            entity.BattleAction = BattleEntities.Action.Attack;
            currentPlayer++;

            if (currentPlayer >= playerBattlers.Count)
            {
                StartCoroutine(BattleRoutine());
            }
            else
            {
                ShowBattleMenu();
            }
        }
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
        Skill usedSkill = currAttacker.ChosenSkill;
        string actionLine = currAttacker.Name + " uses " + usedSkill.SkillName;

        if (usedSkill.RestoresSP)
        {
            currAttacker.CurrSP += usedSkill.SPRestoreAmount;
            if (currAttacker.CurrSP > currAttacker.MaxSP)
                currAttacker.CurrSP = currAttacker.MaxSP;

            bottomText.text += $"\n{currAttacker.Name} restores {usedSkill.SPRestoreAmount} SP!";
        }
        else if (usedSkill.SPCost > 0)
        {
            currAttacker.CurrSP -= usedSkill.SPCost;
            if (currAttacker.CurrSP < 0) currAttacker.CurrSP = 0;
        }

        if (currAttacker.BattleVisuals != null)
        {
            if (usedSkill.SkillName.ToLower().Contains("special"))
                currAttacker.BattleVisuals.PlaySpecialAttackAnimation();
            else
                currAttacker.BattleVisuals.PlayAttackAnimation();
        }

        if (usedSkill.DealsDamage)
        {
            if (usedSkill.TargetType == SkillTarget.AllEnemies)
            {
                List<BattleEntities> enemiesToRemove = new List<BattleEntities>();

                foreach (var enemy in enemyBattlers)
                {
                    enemy.CurrHealth -= usedSkill.DamageAmount;
                    enemy.UpdateUI();

                    if (enemy.CurrHealth <= 0)
                    {
                        enemiesToRemove.Add(enemy);
                        bottomText.text = currAttacker.Name + " defeated " + enemy.Name + "!";
                    }
                }

                foreach (var deadEnemy in enemiesToRemove)
                {
                    allBattlers.Remove(deadEnemy);
                    enemyBattlers.Remove(deadEnemy);
                    StartCoroutine(SafeDestroy(deadEnemy.BattleVisuals)); // 🔁 Safe destroy
                }

                bottomText.text = actionLine + " and hits all enemies for " + usedSkill.DamageAmount + " damage!";

                if (enemyBattlers.Count <= 0)
                {
                    state = BattleState.Won;
                    bottomText.text = WIN_MESSAGE;
                    StartCoroutine(DelayedSceneLoad("Farm", TURN_DURATION));
                    return;
                }
            }
            else if (currTarget != null)
            {
                currTarget.CurrHealth -= usedSkill.DamageAmount;
                currTarget.UpdateUI();
                bottomText.text = actionLine + " on " + currTarget.Name + " for " + usedSkill.DamageAmount + " damage!";

                if (currTarget.CurrHealth <= 0)
                {
                    allBattlers.Remove(currTarget);
                    if (!currTarget.IsPlayer)
                        enemyBattlers.Remove(currTarget);
                    else
                        playerBattlers.Remove(currTarget);

                    StartCoroutine(SafeDestroy(currTarget.BattleVisuals)); // 🔁 Safe destroy
                    bottomText.text = currAttacker.Name + " defeated " + currTarget.Name + "!";

                    if (enemyBattlers.Count <= 0)
                    {
                        state = BattleState.Won;
                        bottomText.text = WIN_MESSAGE;
                        StartCoroutine(DelayedSceneLoad("Farm", TURN_DURATION));
                        return;
                    }

                    if (playerBattlers.Count <= 0)
                    {
                        state = BattleState.Lost;
                        bottomText.text = LOSE_MESSAGE;
                        battleMenu.SetActive(false);
                        skillSelectionMenu.SetActive(false);
                        enemySelectionMenu.SetActive(false);
                        StartCoroutine(DelayedSceneLoad("Farm", TURN_DURATION));
                        return;
                    }
                }
            }
        }
        else if (usedSkill.Heals)
        {
            BattleEntities healTarget = currAttacker;

            healTarget.CurrHealth += usedSkill.HealAmount;
            if (healTarget.CurrHealth > healTarget.MaxHealth)
                healTarget.CurrHealth = healTarget.MaxHealth;

            healTarget.UpdateUI();
            bottomText.text = actionLine + " and heals " + healTarget.Name + " for " + usedSkill.HealAmount + " HP!";
        }
        else
        {
            bottomText.text = actionLine + ", but nothing happens.";
        }
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

    public void SelectAttackAction()
    {
        battleMenu.SetActive(false);
        skillSelectionMenu.SetActive(true);
        ShowBattleMenu();
    }

    private IEnumerator DelayedSceneLoad(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator SafeDestroy(BattleVisuals visuals, float delay = 0.1f)
    {
        yield return new WaitForSeconds(delay);
        if (visuals != null)
            Destroy(visuals.gameObject);
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
    public int CurrSP;
    public int MaxSP;

    public bool IsPlayer;
    public Skill ChosenSkill;

    public BattleVisuals BattleVisuals;
    public int Target;


    public void SetEntityValues(string name, int currHealth, int maxHealth, int initiative, int strenght, int level, bool isPlayer, int currSP = 10, int maxSP = 20)

    {
        Name = name;
        CurrHealth = currHealth;
        MaxHealth = maxHealth;
        Initiative = initiative;
        Strenght = strenght;
        Level = level;
        IsPlayer = isPlayer;
        CurrSP = currSP;
        MaxSP = maxSP;
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









