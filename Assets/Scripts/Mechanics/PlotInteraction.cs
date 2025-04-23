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
    bool inRange = false;
    [SerializeField]
    GameObject enemyManager;
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
        levelLoader.LoadLevel(battleArenaName);
        
    }
    public void UpdatePlotStatus()
    {
        PlantingSpot[] spotArray = gameObject.GetComponentsInChildren<PlantingSpot>();
    }
    public void Harvest()
    {
        Instantiate(enemyManager, null);
        foreach (PlantingSpot selectedPlant in harvestSelections)
        {
            enemyManager.GetComponent<EnemyManager>().GenerateEnemyByName(selectedPlant.name, 0);
        }
    }
}
