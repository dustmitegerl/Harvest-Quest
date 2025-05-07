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

    private void Update()
    {
        HandleInput();
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

    public void StartBattle()
    {
        Debug.Log("starting battle");
        LevelLoader.Instance.LoadLevel(battleArenaName);
    }

    public void UpdatePlotStatus()
    {
        PlantingSpot[] spotArray = gameObject.GetComponentsInChildren<PlantingSpot>();
        // You can use this to refresh harvesting logic if needed
    }

    public void HandleInput()
    {
        if (inRange && Input.GetKeyDown(GameController.Instance.actionKey))
        {
            Debug.Log("Harvesting!");
            StartBattle();
        }
    }
}
