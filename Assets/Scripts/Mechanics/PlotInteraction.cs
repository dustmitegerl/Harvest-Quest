using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlotInteraction : MonoBehaviour, Interactable
{
    public Plant[] plants;
    [SerializeField]
    LevelLoader levelLoader;
    [SerializeField]
    GameObject battleInteraction;
    [SerializeField] 
    Dialog dialog;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            battleInteraction.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !collision.gameObject != null) // prevents an exception when entering battle
        {
            battleInteraction.SetActive(false);
        }
    }

    public void StartBattle()
    {
        Debug.Log("starting battle");
        levelLoader = GameObject.FindGameObjectWithTag("Level Loader").GetComponent<LevelLoader>();
        levelLoader.LoadLevel("BattleArena");
        
    }

    public void CloseMenu()
    {
        battleInteraction.SetActive(false);
        Debug.Log("closing battle interaction");
    }

    public void Interact()
    {
        //Debug.Log("Interacting with plot");
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
    }

}
