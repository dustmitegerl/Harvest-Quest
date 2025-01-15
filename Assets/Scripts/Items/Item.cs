using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Thanks to Pogle: Inventory Syste | Unity Tutorial (Youtube)

[CreateAssetMenu(menuName = "Data/Item")]
public class Item : ScriptableObject
{
    public Sprite sprites;
    public SlotTag itemTag;

    [Header("If the item can be equipped")]
    public GameObject equipmentPrefab;
}
