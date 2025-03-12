using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlotInteraction : MonoBehaviour
{
    public Plant[] plants;
    [SerializeField]
    LevelLoader levelLoader;

    void Start()
    {
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("player detected");
            if (Input.GetKeyDown(KeyCode.H)){
                StartBattle();
            }
        }
    }
    public void StartBattle()
    {
        Debug.Log("starting battle");
        levelLoader = GameObject.FindGameObjectWithTag("Level Loader").GetComponent<LevelLoader>();
        levelLoader.LoadLevel("BattleArena");
        
    }

}
