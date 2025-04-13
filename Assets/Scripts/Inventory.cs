using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<ItemSlot> slots;
}

[Serializable]

public class ItemSlot
{
    [SerializeField] Item item;
    [SerializeField] int count;

    public Item Item => item;
    public int Count => count;
}
