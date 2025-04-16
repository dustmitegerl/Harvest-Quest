using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ItemCategory { Fertillizers, Seeds, Weapons}
public class Inventory : MonoBehaviour
{
    [SerializeField] List<ItemSlot> slots;
    [SerializeField] List<ItemSlot> seedSlots;
    [SerializeField] List<ItemSlot> weaponSlots;

    List<List<ItemSlot>> allSlots;

    public event Action OnUpdated;

    private void Awake()
    {
        allSlots = new List<List<ItemSlot>>() { slots, seedSlots, weaponSlots };
    }

    public static List<string> ItemCategories { get; set; } = new List<string>()
    {
        "Fertillizers", "Seeds", "Weapons"
    };

    public List<ItemSlot> GetSlotsByCategory(int categoryIndex)
    {
        return allSlots[categoryIndex];
    }

    //public Item GetItem(int itemIndex, int)

    public static Inventory GetInventory()
    {
        return FindObjectOfType<Unit>().GetComponent<Inventory>();
    }

    public void AddItem(Item item, int count = 1)
    {
        //int category = (int)GetCategoryFromItem(item);
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
