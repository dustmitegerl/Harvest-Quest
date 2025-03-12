using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EncounterSystem : MonoBehaviour
{
    [SerializeField] private Encounter[] enemiesInScene;
    [SerializeField] private int maxNumEnemies;
    [SerializeField] private List<Collider2D> triggerColliders; // List of 2D colliders to trigger the battle
    
    private EnemyManager enemyManager;
    private bool isBattleTriggered;

    void Start()
    {
        enemyManager = GameObject.FindFirstObjectByType<EnemyManager>();
        isBattleTriggered = false;

        // Register the OnTriggerEnter2D method for each collider in the list
        foreach (var collider in triggerColliders)
        {
            var trigger = collider.gameObject.AddComponent<TriggerHandler2D>();
            trigger.OnPlayerEnter += TriggerBattle;
        }
    }

    private void TriggerBattle()
    {
        if (!isBattleTriggered)
        {
            isBattleTriggered = true;
            Debug.Log("Battle triggered!");
            SceneManager.LoadScene("BattleArena_Test");  // Load the battle scene
        }
    }
}

public class TriggerHandler2D : MonoBehaviour
{
    public delegate void PlayerEnterHandler();
    public event PlayerEnterHandler OnPlayerEnter;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerEnter?.Invoke();
        }
    }
}

[System.Serializable]
public class Encounter
{
    public EnemyInfo Enemy;
    public int LevelMin;
    public int LevelMax;
}