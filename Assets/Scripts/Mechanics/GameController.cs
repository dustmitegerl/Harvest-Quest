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
    public KeyCode actionKey;
    public GameState state;
    GameState prevState;
    //MenuController menuController;
    #region player position
    public string currentScene;
    [SerializeField] string lastScene;
    Vector3 lastPos;
    #endregion

    #region making it a singleton
    private static GameController _instance;
    public static GameController Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }
    #endregion

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.name;
        if (scene.name.ToLower().Contains("farm") && lastScene != null)
        {
            GameObject playerPrefab = GameObject.FindGameObjectWithTag("Player");
            if (lastScene.ToLower().Contains("farm"))
            {
                return;
            }
            else if (lastScene.ToLower().Contains("town"))
            {
                Vector3 gateLocation = GameObject.Find("Town Gate").transform.position;
                Vector3 offset = new Vector3(0, 3, 0);
                if (playerPrefab != null)
                {
                    playerPrefab.transform.position = gateLocation - offset;
                }
            }
            else if (lastScene.ToLower().Contains("battle")){
                playerPrefab.transform.position = lastPos;
            }
        }
        else if (lastScene == null || lastScene.ToLower().Contains("menu"))
        {
            return;
        }
    }
    public void SetLastScene()
    {
        lastScene = SceneManager.GetActiveScene().name;
    }
    public void SetLastPos(Vector3 pos)
    {
        lastPos = pos;
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

    private void Update()
    {
        if (worldCamera == null)
        {
            worldCamera = Camera.main;
        }
        if (playerController == null)
        {
            try { 
                playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>(); 
            } 
            catch { 
                return; 
            }
        }
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();

            /*if (Input.GetKeyDown(KeyCode.Escape))
            {
                menuController.OpenMenu();
                state = GameState.Menu;
            }*/
        }
        else if (state == GameState.Battle)
        {
            //battleSystem.HandleUpdate();
        }
        else if (state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
       /* else if (state == GameState.Menu)
        {
            menuController.HandleUpdate();
        }*/
        else if(state == GameState.Inventory)
        {
            Action onBack = () =>
            {
                inventoryUI.gameObject.SetActive(false);
                state = GameState.FreeRoam;
            };

            inventoryUI.HandleUpdate(onBack);
        }
    }

    void OnMenuSelected(int selectedItem)
    {
        if(selectedItem == 0)
        {
            //Instruction
        }
        else if (selectedItem == 1)
        {
            //Option
        }
        else if (state == GameState.Inventory)
        {
            Action onBack = () =>
            {
                inventoryUI.gameObject.SetActive(false);
                state = GameState.FreeRoam;
            };

            inventoryUI.HandleUpdate(onBack);
        }
        else if (selectedItem == 3)
        {
            //Save
        }
        /*else if (state == GameState.Menu)
        {
            menuController.HandleUpdate();
        }*/

        state = GameState.FreeRoam;
    }
}
