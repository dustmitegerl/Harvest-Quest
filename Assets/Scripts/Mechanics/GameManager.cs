using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public static GameManager instances;

    public TileManager tileManager;

    private void Awake()
    {
        if(instances != null && instances != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instances = this;
        }

        tileManager = GetComponent<TileManager>();
    }

    public InventorySlot inventoryContainer;
}
