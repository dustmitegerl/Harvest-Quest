using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//Thanks to Coco Code: Unity INVENTORY: A Definitive Tutorial (Youtube)

[CreateAssetMenu(menuName = "Scriptable object/Item")]

public class Item : ScriptableObject
{
    [Header("Only gameplay")]
    public TileBase tile;
    //Define if the inventory is a block or tool
    public ItemType type;
    //If the tool digs or mines
    public ActionType actionType;
    //Range the tool can be used
    public Vector2Int range = new Vector2Int(5, 4);

    [Header("Only UI")]
    public bool stackable = true;

    [Header("Both")] 
    //Sprite shown in the inventory
    public Sprite image;
}

public enum ItemType
{
    BuildingBlock,
    Tool,
    Seed
}

public enum ActionType
{
    Plow,
    Plant
}
