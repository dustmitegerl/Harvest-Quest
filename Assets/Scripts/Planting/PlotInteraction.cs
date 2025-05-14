using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlotInteraction : MonoBehaviour
{
    public List<PlantingSpot> plantingSpots;
    [SerializeField] public List<PlantingSpot> readyForHarvest;
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private string battleArenaName = "BattleArena_Test";
    private bool inRange = false;
    [SerializeField] GameObject enemyManager;

    private void Update()
    {
        HandleInput();
    }

    public void LateUpdate()
    {
        UpdatePlotStatus();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("player detected");
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRange = false;
        }
    }

    public IEnumerator StartBattle()
    {
        Instantiate(enemyManager);
        EnemyManager instance = EnemyManager.Instance;
        foreach (PlantingSpot plant in readyForHarvest)
        {
            instance.GenerateEnemyByName(plant.currentPlant, plant.level);
        }
        yield return new WaitForEndOfFrame(); 
        Debug.Log("starting battle");
        LevelLoader.Instance.LoadLevel(battleArenaName);
    }
    [ContextMenu("update plot status")]
    public void UpdatePlotStatus()
    {
        PlantingSpot[] spotArray = gameObject.GetComponentsInChildren<PlantingSpot>();
        foreach (PlantingSpot spot in spotArray)
        {
            if (!readyForHarvest.Contains(spot) && spot.plantStage == 4)
            {
                readyForHarvest.Add(spot);
            }
            else if (readyForHarvest.Contains(spot) && spot.plantStage != 4)
            {
                readyForHarvest.Remove(spot);
            }
        }
        // You can use this to refresh harvesting logic if needed
    }

    public void HandleInput()
    {
        if (inRange && Input.GetKeyDown(GameController.Instance.actionKey))
        {
            Debug.Log("Harvesting!");
            StartCoroutine(StartBattle());
        }
    }
}
