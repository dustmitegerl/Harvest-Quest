using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
   [SerializeField] List<ItemSlot> slots;

    public List<ItemSlot> Slots => slots;

    public static Inventory GetInventory()
    {
        return FindObjectOfType<PlayerMovement>().GetComponent<Inventory>();
    }
}

[Serializable]
public class ItemSlot
{
    [SerializeField] Item item;
    [SerializeField] int count;

    public Item Item => item;
    public int Count => count;
}
