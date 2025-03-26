using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlotInteraction : MonoBehaviour
{
    public PlantingSpot[] plantingSpots;
    [SerializeField]
    LevelLoader levelLoader;

    void Start()
    {
        levelLoader = GameObject.FindGameObjectWithTag("Level Loader").GetComponent<LevelLoader>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("player detected");
            if (Input.GetKeyDown(KeyCode.H)){
                Debug.Log("Harvesting!");
                StartBattle();
            }
        }
    }
    public void StartBattle()
    {
        Debug.Log("starting battle");
        levelLoader.LoadLevel("BattleArena");
        
    }

}
