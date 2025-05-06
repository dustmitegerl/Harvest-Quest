using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlotInteraction : MonoBehaviour
{
    public PlantingSpot[] plantingSpots;
    [SerializeField]
    public PlantingSpot[] readyForHarvest;
    [SerializeField]
    LevelLoader levelLoader;
    [SerializeField]
    string battleArenaName = "BattleArena_Test";
    [SerializeField]
    GameObject enemyManagerPrefab;
    //[SerializeField]
    //GameObject partyManagerPrefab;
    bool inRange = false;

    private void Update()
    {
        HandleInput();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("player detected");
            inRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        inRange = false;
    }
    public void StartBattle()
    {
        Debug.Log("starting battle");
        //enemyManagerPrefab = Instantiate(enemyManagerPrefab);
        //EnemyManager enemyManager = enemyManagerPrefab.GetComponent<EnemyManager>();
        //foreach (PlantingSpot spot in plantingSpots)
        //{
        //    if (spot.plantStage == 4)
        //    {
        //        enemyManager.GenerateEnemyByName(spot.currentPlant, spot.level);
        //    }
        //}
        LevelLoader.Instance.LoadLevel(battleArenaName);
        
    }
    public void UpdatePlotStatus()
    {
        PlantingSpot[] spotArray = gameObject.GetComponentsInChildren<PlantingSpot>();
    }

    public void HandleInput() {
        if (inRange && Input.GetKeyDown(GameController.Instance.actionKey))
        {
            Debug.Log("Harvesting!");
            StartBattle();
        }
    }
    [ContextMenu("Load planting spots")]
    public void UpdateSpots()
    {
        plantingSpots = GetComponentsInChildren<PlantingSpot>();
        readyForHarvest = plantingSpots.Where(plantingSpot => plantingSpot.plantStage == 4).ToArray();
    }
    public void Harvest()
    {
        Instantiate(enemyManagerPrefab, null);
        foreach (PlantingSpot plant in readyForHarvest)
        {
            enemyManagerPrefab.GetComponent<EnemyManager>().GenerateEnemyByName(plant.name, 0);
        }
    }
}
