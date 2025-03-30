using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotInteraction : MonoBehaviour
{
    public PlantingSpot[] plantingSpots;
    [SerializeField]
    LevelLoader levelLoader;
    [SerializeField]
    string battleArenaName = "BattleArena_Test";
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
        levelLoader.LoadLevel(battleArenaName);
        
    }

}
