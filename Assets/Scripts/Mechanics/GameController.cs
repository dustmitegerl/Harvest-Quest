using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { FreeRoam, Battle, Dialog, Menu, Inventory, Paused }

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerMovement playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;
    [SerializeField] InventoryUI inventoryUI;

    public GameState state;
    GameState prevState;

    //MenuController menuController;

    public static GameController Instance { get; private set; }
    private void Awake()
    {
        Instance = this;

        //menuController = GetComponent<MenuController>();
    }

    private void Start()
    {
        worldCamera = Camera.main;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerController.OnEncountered += StartBattle;
        //battleSystem.EndBattle += OverBattle;

        DialogManager.Instance.OnShowDialog += () =>
        {
            prevState = state;
            state = GameState.Dialog;
        };

        DialogManager.Instance.OnCloseDialog += () =>
        {
            if (state == GameState.Dialog)
                state = prevState;
        };

        /*menuController.onBack += () =>
        {
            state = GameState.FreeRoam;
        };

        menuController.onMenuSelected += OnMenuSelected;*/
    }

    public void PauseGame(bool pause)
    {
        if (pause)
        {
            prevState = state;
            state = GameState.Paused;
        }
        else
        {
            state = prevState;
        }
    }

    void StartBattle()
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        //battleSystem.Start();
    }

    void OverBattle(bool WON)
    {
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }
}
