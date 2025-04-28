using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlotInteraction : MonoBehaviour
{
    public List<PlantingSpot> plantingSpots;
    [SerializeField]
    public List<PlantingSpot> harvestSelections;
    [SerializeField]
    LevelLoader levelLoader;
    [SerializeField]
    string battleArenaName = "BattleArena_Test";
    //[SerializeField]
    //GameObject enemyManagerPrefab;
    //[SerializeField]
    //GameObject partyManagerPrefab;
    bool inRange = false;
    void Start()
    {
        levelLoader = GameObject.FindGameObjectWithTag("Level Loader").GetComponent<LevelLoader>();
    }
    private void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("Harvesting!");
            StartBattle();
        }
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
        levelLoader.LoadLevel(battleArenaName);
        
    }
    public void UpdatePlotStatus()
    {
        PlantingSpot[] spotArray = gameObject.GetComponentsInChildren<PlantingSpot>();
    }
    //public void Harvest()
    //{
    //    Instantiate(enemyManagerPrefab, null);
    //    foreach (PlantingSpot selectedPlant in harvestSelections)
    //    {
    //        enemyManagerPrefab.GetComponent<EnemyManager>().GenerateEnemyByName(selectedPlant.name, 0);
    //    }
    //}
}
