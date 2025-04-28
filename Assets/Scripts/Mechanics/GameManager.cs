using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TileManager tileManager;

    public Vector3 playerLastPositionInFarm;


    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        tileManager = GetComponent<TileManager>();
    }

    //private void FixedUpdate()
    //{
    //    while (Scene.name == "Farm")
    //    {

    //    }
    //}
}
